using Newtonsoft.Json;
using Our.Umbraco.Hider.Core.Constants;
using Our.Umbraco.Hider.Core.Extensions;
using Our.Umbraco.Hider.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models.Membership;
using Umbraco.Web;

namespace Our.Umbraco.Hider.Core.Services
{
    public interface IConfigurationService
    {
        IEnumerable<Rule> GetRulesForUser(IUser user);

        UmbracoHiderConfigModel LoadConfigurationFile();

        UmbracoHiderConfigModel SaveConfigurationFile(UmbracoHiderConfigModel configurationFile);

        bool IsActionsButtonHidden();
    }

    public class ConfigurationService : IConfigurationService
    {
        private readonly ILogger _logger;
        private readonly IUmbracoContextFactory _umbracoContextFactory;

        public ConfigurationService(ILogger logger, IUmbracoContextFactory umbracoContextFactory)
        {
            _logger = logger;
            _umbracoContextFactory = umbracoContextFactory;
        }

        public IEnumerable<Rule> GetRulesForUser(IUser user)
        {
            var result = new List<Rule>();

            if (user == null) { return result; }

            var currentUsername = user.Username.ToLower();
            var currentUserGroups = user.Groups.ToList();

            // If the current user is admin then omit all rules
            if (currentUserGroups.Any(x => x.Alias.Equals("administrators", StringComparison.InvariantCultureIgnoreCase)))
            {
                return result;
            }

            // Get rules from the cache
            var configurationFile = HttpContext.Current.Cache.Get(ApplicationConstants.CacheKeys.ConfigurationFile) as UmbracoHiderConfigModel
                                    ?? LoadConfigurationFile();

            // Filter rules to apply for the current user
            result = configurationFile.Rules.Where(x =>
                string.IsNullOrWhiteSpace(x.Users) && string.IsNullOrWhiteSpace(x.UserGroups)
                || !string.IsNullOrWhiteSpace(x.Users) && string.IsNullOrWhiteSpace(x.UserGroups) && x.Users.ToLower().ToDelimitedList().Contains(currentUsername)
                || string.IsNullOrWhiteSpace(x.UserGroups) && !string.IsNullOrWhiteSpace(x.UserGroups) && x.UserGroups.ToLower().ToDelimitedList().Contains(currentUserGroups.FirstOrDefault()?.Alias)
                || x.Users.ToLower().ToDelimitedList().Contains(currentUsername) || x.UserGroups.ToLower().ToDelimitedList().Contains(currentUserGroups.FirstOrDefault()?.Alias)

            ).ToList();

            result = result.Where(rule => rule.Enabled).ToList();

            return result;
        }

        public UmbracoHiderConfigModel LoadConfigurationFile()
        {
            var configurationFile = HttpContext.Current.Cache.Get(ApplicationConstants.CacheKeys.ConfigurationFile) as UmbracoHiderConfigModel;

            try
            {
                if (configurationFile == null)
                {
                    var configurationFilePath = GetConfigurationFilePath();

                    if (!File.Exists(configurationFilePath) || string.IsNullOrWhiteSpace(File.ReadAllText(configurationFilePath)))
                    {
                        return SaveConfigurationFile(new UmbracoHiderConfigModel());
                    }

                    using (var reader = new StreamReader(configurationFilePath))
                    {
                        configurationFile = JsonConvert.DeserializeObject<UmbracoHiderConfigModel>(reader.ReadToEnd());

                        // Cache the result for a year but with a dependency on the config file
                        HttpContext.Current.Cache.Add(ApplicationConstants.CacheKeys.ConfigurationFile, configurationFile,
                            new CacheDependency(configurationFilePath),
                            DateTime.Now.AddYears(1), Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);

                        return configurationFile;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error<ConfigurationService>($"Could not load the file in this path {ApplicationConstants.ConfigurationFile.ConfigurationFilePath}", ex);
            }

            return configurationFile;
        }

        public UmbracoHiderConfigModel SaveConfigurationFile(UmbracoHiderConfigModel configurationFile)
        {
            try

            {
                var configurationFilePath = GetConfigurationFilePath();

                using (var writer = new StreamWriter(configurationFilePath))
                {
                    var serializer = new JsonSerializer { Formatting = Formatting.Indented };
                    serializer.Serialize(writer, configurationFile);
                }

                return configurationFile;
            }
            catch (Exception e)
            {
                _logger.Error<IConfigurationService>($"Could not save the configuration file in this path {ApplicationConstants.ConfigurationFile.ConfigurationFilePath}", e);
            }

            return null;
        }

        public bool IsActionsButtonHidden()
        {
            var hideButtons = new List<string>();

            using (var umbracoContext = _umbracoContextFactory.EnsureUmbracoContext())
            {
                var user = umbracoContext.UmbracoContext.Security.CurrentUser;

                var rules = GetRulesForUser(user);

                //var g = umbracoContext.UmbracoContext.PublishedRequest.PublishedContent;

                //rules = rules.Where(rule =>
                //        (string.IsNullOrWhiteSpace(rule.ContentTypes) || rule.ContentTypes.ToDelimitedList().InvariantContains(g.ContentType.Alias))
                //        && (string.IsNullOrWhiteSpace(rule.ContentIds) || rule.ContentIds.ToDelimitedList().InvariantContains(g.Id.ToString()))
                //        && (string.IsNullOrWhiteSpace(rule.ParentContentIds) || rule.ParentContentIds.ToDelimitedList().InvariantContains(g.Parent.Id.ToString())))
                //    .ToList();

                var buttonRules = rules.Where(r => r.Type.InvariantEquals(ApplicationConstants.RuleType.HideButtons) && !string.IsNullOrWhiteSpace(r.Names));

                foreach (var buttonRule in buttonRules)
                {
                    hideButtons.AddRangeUnique(buttonRule.Names.ToDelimitedList().ToList());
                }
            }

            return hideButtons.Contains("actions");
        }

        private static string GetConfigurationFilePath()
        {
            return HttpContext.Current.Server.MapPath(ApplicationConstants.ConfigurationFile.ConfigurationFilePath);
        }
    }
}