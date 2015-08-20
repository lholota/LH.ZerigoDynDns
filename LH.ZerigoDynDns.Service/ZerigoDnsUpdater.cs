using System.Configuration;
using System.Net;
using NLog;

namespace Net.DynDnsUpdate
{
    class ZerigoDnsUpdater
    {
        private readonly ILogger _log = LogManager.GetCurrentClassLogger();
        private readonly WebClient _client;

        public ZerigoDnsUpdater(WebClient client)
        {
            _client = client;
        }

        public void UpdateDnsRecords(IPAddress newPublicIp)
        {
            var domain = ConfigurationManager.AppSettings[AppSettingsKeys.Domain];
            var uriFormat = ConfigurationManager.AppSettings[AppSettingsKeys.ZerigoUpdateUriFormat];

            var uri = uriFormat
                .Replace("$DOMAIN$", domain)
                .Replace("$IP$", newPublicIp.ToString())
                .Replace("$USERNAME$", ConfigurationManager.AppSettings[AppSettingsKeys.ZerigoUserName])
                .Replace("$APIKEY$", ConfigurationManager.AppSettings[AppSettingsKeys.ZerigoApiKey]);

            _log.Info("Updating the DNS record for {0} to {1} using the following uri: {2}.", 
                domain, newPublicIp, uri);

            _client.DownloadString(uri);

            _log.Info("DNS record update was successful.");
        }
    }
}