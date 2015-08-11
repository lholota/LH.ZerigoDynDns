using System;
using System.Configuration;
using System.Net;
using System.Xml.Linq;
using NLog;

namespace Net.DynDnsUpdate
{
    class PublicIpRetriever
    {
        private readonly WebClient _client;
        private readonly ILogger _log = LogManager.GetCurrentClassLogger();

        public PublicIpRetriever(WebClient client)
        {
            _client = client;
        }

        public IPAddress GetPublicIp(WebClient client)
        {
            var uri = ConfigurationManager.AppSettings[AppSettingsKeys.ZerigoWhatsMyIpUri];
            _log.Info("Retrieving the public IP from {0}", uri);

            var response = client.DownloadString(new Uri(uri));

            return ParseWhatsMyIpResponse(response);
        }

        private IPAddress ParseWhatsMyIpResponse(string response)
        {
            var element = XElement.Parse(response);
            return IPAddress.Parse(element.Value);
        }
    }
}