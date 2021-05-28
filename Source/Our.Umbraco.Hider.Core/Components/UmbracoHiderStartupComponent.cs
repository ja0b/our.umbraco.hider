using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };

            // Load rules and create config file if it doesn't exist
            _configurationService.LoadConfigurationFile();
        }

        public void Terminate()
        {
        }
    }
}