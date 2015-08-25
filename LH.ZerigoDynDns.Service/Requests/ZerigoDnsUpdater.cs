namespace LH.ZerigoDynDns.Service.Requests
{
    using System.Configuration;
    using System.Net;
    using Configuration;
    using LH.ZerigoDynDns.Service;
    using NLog;

    internal class ZerigoDnsUpdater
    {
        private readonly WebClient client;
        private readonly ZerigoDynDnsSection configSection;
        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        public ZerigoDnsUpdater(WebClient client, ZerigoDynDnsSection configSection)
        {
            this.client = client;
            this.configSection = configSection;
        }

        public void UpdateDnsRecords(string domain, IPAddress newPublicIp)
        {
            var uriFormat = ConfigurationManager.AppSettings[AppSettingsKeys.ZerigoUpdateUriFormat];

            var uri = uriFormat
                .Replace("$DOMAIN$", domain)
                .Replace("$IP$", newPublicIp.ToString())
                .Replace("$USERNAME$", this.configSection.Credentials.UserName)
                .Replace("$APIKEY$", this.configSection.Credentials.ApiKey);

            this.log.Info(
                "Updating the DNS record for {0} to {1} using the following uri: {2}.", 
                domain, 
                newPublicIp, 
                uri);

            this.client.DownloadString(uri);

            this.log.Info("DNS record update was successful.");
        }
    }
}