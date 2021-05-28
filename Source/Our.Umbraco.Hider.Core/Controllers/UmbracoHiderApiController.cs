using Our.Umbraco.Hider.Core.Models;
using Our.Umbraco.Hider.Core.Services;
using System.Web.Http;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace Our.Umbraco.Hider.Core.Controllers
{
    [IsBackOffice]
    [PluginController("UmbracoHider")]
    public class UmbracoHiderApiController : UmbracoAuthorizedApiController
    {
        private readonly IConfigurationService _configurationService;

        public UmbracoHiderApiController(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        [HttpGet]
        public UmbracoHiderConfigModel GetConfig()
        {
            var configurationFile = _configurationService.LoadConfigurationFile();
            return configurationFile;
        }

        [HttpPost]
        public UmbracoHiderConfigModel SaveRules([FromBody] UmbracoHiderConfigModel config)
        {
            var configurationFile = _configurationService.SaveConfigurationFile(config);
            return configurationFile;
        }

        [HttpGet]
        public bool IsActionsButtonHidden()
        {
            var isActionsButtonHidden = _configurationService.IsActionsButtonHidden();
            return isActionsButtonHidden;
        }
    }
}