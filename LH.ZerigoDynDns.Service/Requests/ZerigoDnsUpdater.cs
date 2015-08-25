namespace LH.ZerigoDynDns.Service.Requests
{
    using System.Configuration;
    using System.Net;
    using System.Net.Http;
    using Configuration;
    using LH.ZerigoDynDns.Service;
    using NLog;

    internal class ZerigoDnsUpdater
    {
        private readonly HttpClient client;
        private readonly ZerigoDynDnsSection configSection;
        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        public ZerigoDnsUpdater(HttpClient client, ZerigoDynDnsSection configSection)
        {
            this.configSection = configSection;
            this.client = client;
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

            var response = this.client.GetAsync(uri).Result;

            switch (response.StatusCode)
            {
                case HttpStatusCode.Forbidden:
                case HttpStatusCode.Unauthorized:
                    this.log.Error("The server returned authentication error, please verify your user name and the API key.");
                    break;

                case HttpStatusCode.InternalServerError:
                    this.log.Error("There seems to be an issue with the zerigo.com website. The application will retry in the defined interval.");
                    break;
            }

            response.EnsureSuccessStatusCode();
            this.log.Info("DNS record update was successful.");
        }
    }
}