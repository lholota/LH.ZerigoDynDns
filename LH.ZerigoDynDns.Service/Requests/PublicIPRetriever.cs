namespace LH.ZerigoDynDns.Service.Requests
{
    using System;
    using System.Configuration;
    using System.Net;
    using System.Xml.Linq;
    using LH.ZerigoDynDns.Service;
    using NLog;

    internal class PublicIpRetriever
    {
        private readonly WebClient client;
        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        public PublicIpRetriever(WebClient client)
        {
            this.client = client;
        }

        public IPAddress GetPublicIp()
        {
            var uri = ConfigurationManager.AppSettings[AppSettingsKeys.ZerigoWhatsMyIpUri];
            this.log.Info("Retrieving the public IP from {0}", uri);

            var response = this.client.DownloadString(new Uri(uri));

            return this.ParseWhatsMyIpResponse(response);
        }

        private IPAddress ParseWhatsMyIpResponse(string response)
        {
            var element = XElement.Parse(response);
            return IPAddress.Parse(element.Value);
        }
    }
}