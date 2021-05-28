using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Our.Umbraco.Hider.Core.Models
{
    public class UmbracoHiderConfigModel
    {
        public UmbracoHiderConfigModel()
        {
            Rules = Enumerable.Empty<Rule>();
        }

        [JsonProperty]
        public IEnumerable<Rule> Rules { get; set; }
    }
}