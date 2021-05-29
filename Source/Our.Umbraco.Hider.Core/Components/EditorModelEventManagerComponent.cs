using Our.Umbraco.Hider.Core.Constants;
using Our.Umbraco.Hider.Core.Extensions;
using Our.Umbraco.Hider.Core.Models;
using Our.Umbraco.Hider.Core.Services;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Filters;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Web.Editors;
using Umbraco.Web.Models.ContentEditing;

namespace Our.Umbraco.Hider.Core.Components
{
    public class EditorModelEventManagerComponent : IComponent
    {
        private readonly IConfigurationService _configurationService;

        public EditorModelEventManagerComponent(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public void Initialize()
        {
            EditorModelEventManager.SendingContentModel += EditorModelEventManagerOnSendingContentModel;
        }

        public void Terminate()
        {
            EditorModelEventManager.SendingContentModel -= EditorModelEventManagerOnSendingContentModel;
        }

        private void EditorModelEventManagerOnSendingContentModel(HttpActionExecutedContext sender, EditorModelEventArgs<ContentItemDisplay> e)
        {
            var contentItemDisplay = e.Model;
            var rules = _configurationService.GetRules();

            if (!rules.Any()) { return; }

            HiddenItems(contentItemDisplay, rules);
        }

        private static void HiddenItems(ContentItemDisplay contentItemDisplay, IEnumerable<Rule> rules)
        {
            HideProperties(contentItemDisplay, rules);

            HideTabs(contentItemDisplay, rules);

            HideButtons(contentItemDisplay, rules);

            HideContentApps(contentItemDisplay, rules);
        }

        private static void HideButtons(ContentItemDisplay contentItemDisplay, IEnumerable<Rule> rules)
        {
            var buttonRules = rules.Where(r => r.Type.InvariantEquals(ApplicationConstants.RuleType.HideButtons) && !string.IsNullOrWhiteSpace(r.Names));
            var actionsToRemoveForSave = new List<string> { "A" };
            var actionsToRemoveForPublish = new List<string> { "U" };

            foreach (var buttonRule in buttonRules)
            {
                var buttonsToHide = buttonRule.Names.ToDelimitedList();

                foreach (var buttonToHide in buttonsToHide)
                {
                    if (buttonToHide.InvariantContains("preview"))
                    {
                        contentItemDisplay.AllowPreview = false;
                    }

                    if (buttonToHide.InvariantContains("save"))
                    {
                        contentItemDisplay.AllowedActions = contentItemDisplay.AllowedActions.Where(x => !actionsToRemoveForSave.Contains(x));
                    }

                    if (buttonToHide.InvariantContains("publish"))
                    {
                        contentItemDisplay.AllowedActions = contentItemDisplay.AllowedActions.Where(x => !actionsToRemoveForPublish.Contains(x));
                    }
                }
            }
        }

        private static void HideContentApps(ContentItemDisplay contentItemDisplay, IEnumerable<Rule> rules)
        {
            var hideContentApps = new List<string>();
            var contentAppsRules = rules.Where(r => r.Type.InvariantEquals(ApplicationConstants.RuleType.HideContentApps) && !string.IsNullOrWhiteSpace(r.Names));

            foreach (var contentAppRule in contentAppsRules)
            {
                var contentAppsToHide = contentAppRule.Names.ToDelimitedList();

                hideContentApps.AddRangeUnique(contentAppsToHide);
            }

            contentItemDisplay.ContentApps = contentItemDisplay.ContentApps.Where(x => !hideContentApps.InvariantContains(x.Name)).ToList();
        }

        private static void HideProperties(ContentItemDisplay contentItemDisplay, IEnumerable<Rule> rules)
        {
            var propertyRules = rules.Where(r => r.Type.InvariantEquals(ApplicationConstants.RuleType.HideProperties) && !string.IsNullOrWhiteSpace(r.Names));

            foreach (var propertyRule in propertyRules)
            {
                var propertiesToHide = propertyRule.Names.ToDelimitedList();

                foreach (var contentVariant in contentItemDisplay.Variants)
                {
                    foreach (var tab in contentVariant.Tabs)
                    {
                        tab.Properties = tab.Properties.Where(x => !propertiesToHide.InvariantContains(x.Alias));
                    }
                }
            }
        }

        private static void HideTabs(ContentItemDisplay contentItemDisplay, IEnumerable<Rule> rules)
        {
            var tabRules = rules.Where(r => r.Type.InvariantEquals(ApplicationConstants.RuleType.HideTabs) && !string.IsNullOrWhiteSpace(r.Names));

            foreach (var tabRule in tabRules)
            {
                var tabsToHide = tabRule.Names.ToDelimitedList();

                foreach (var contentVariant in contentItemDisplay.Variants)
                {
                    contentVariant.Tabs = contentVariant.Tabs.Where(x => !tabsToHide.Contains(x.Alias));
                }
            }
        }
    }
}