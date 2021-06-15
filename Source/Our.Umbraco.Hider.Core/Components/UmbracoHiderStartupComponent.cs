using Our.Umbraco.Hider.Core.Services;
using System.Net;
using Umbraco.Core.Composing;

namespace Our.Umbraco.Hider.Core.Components
{
    public class UmbracoHiderStartupComponent : IComponent
    {
        private readonly IConfigurationService _configurationService;

        public UmbracoHiderStartupComponent(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public void Initialize()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;

            // Load rules and create config file if it doesn't exist
            _configurationService.LoadConfigurationFile();
        }

        public void Terminate()
        {
        }
    }
}