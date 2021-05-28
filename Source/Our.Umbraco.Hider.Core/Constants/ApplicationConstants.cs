namespace Our.Umbraco.Hider.Core.Constants
{
    public static class ApplicationConstants
    {
        public static class CacheKeys
        {
            public const string ConfigurationFile = "ConfigurationService_ConfigurationFile";
        }

        public static class ConfigurationFile
        {
            public const string ConfigurationFilePath = "~/Config/umbracoHider.config.js";
        }

        public static class RuleType
        {
            public const string HideButtons = "HideButtons";
            public const string HideContentApps = "HideContentApps";
            public const string HideProperties = "HideProperties";
            public const string HideTabs = "HideTabs";
        }
    }
}