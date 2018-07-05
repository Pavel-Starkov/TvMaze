using System;
using System.Configuration;
using System.Reflection;

namespace PageProcessor.Settings
{
    public class DefaultSettings : ISettings
    {
        public DefaultSettings SetFromConfig()
        {
            var configSettings = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location)?.AppSettings?.Settings;
            if (configSettings != null)
            {
                RetryTimes = Convert.ToInt32(configSettings[Constants.RetryTimes].Value);
                DelayInSeconds = Convert.ToInt32(configSettings[Constants.DelayInSeconds].Value);
                LiteDbConnectionString = configSettings[Constants.LiteDbConnectionString].Value;
                CosmosDbConnectionString = configSettings[Constants.CosmosDbConnectionString].Value;
                ShowsApiUrl = configSettings[Constants.ShowsApiUrl].Value;
                ShowCastApiUrl = configSettings[Constants.ShowCastApiUrl].Value;
                DateTimeFormat = configSettings[Constants.DateTimeFormat].Value;
            }

            return this;
        }

        public int RetryTimes { get; private set; }
        public int DelayInSeconds { get; private set; }
        public string LiteDbConnectionString { get; private set; }
        public string CosmosDbConnectionString { get; private set; }
        public string ShowsApiUrl { get; private set; }
        public string ShowCastApiUrl { get; private set; }
        public string DateTimeFormat { get; private set; }
    }
}
