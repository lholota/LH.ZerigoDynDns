namespace LH.ZerigoDynDns.Service
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Timers;
    using Configuration;
    using NLog;
    using Requests;

    internal class DnsUpdateService : IDisposable
    {
        private readonly Timer timer = new Timer();
        private readonly ILogger log = LogManager.GetCurrentClassLogger();
        private readonly ZerigoDynDnsSection configSection;

        public DnsUpdateService()
        {
            this.configSection = (ZerigoDynDnsSection)ConfigurationManager.GetSection("zerigoDynDns");
        }

        public void Start()
        {
            this.log.Info("Starting the DNS Update service");

            var miliSeconds = this.configSection.CheckIntervalInSeconds * 1000;

            this.timer.Interval = miliSeconds;
            this.timer.Elapsed += this.TimerOnElapsed;
            this.timer.Start();

            this.log.Info("The DNS Update service has been started successfully");
        }

        public void Stop()
        {
            this.log.Info("Stopping the DNS Update service");
            this.timer.Stop();
            this.log.Info("The DNS Update service has been stopped");
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            try
            {
                using (var client = new WebClient())
                {
                    var publicIpRetriever = new PublicIpRetriever(client);
                    var dnsUpdater = new ZerigoDnsUpdater(client, this.configSection);

                    var publicIp = publicIpRetriever.GetPublicIp();
                    this.log.Info("The public IP address is {0}.", publicIp);

                    foreach (DomainElement domain in this.configSection.Domains)
                    {
                        var domainIps = this.ResolveDomainIp(domain.Name)
                            .Select(x => x.MapToIPv4())
                            .Distinct()
                            .ToArray();

                        var ipCount = domainIps.Count();
                        if (ipCount != 1)
                        {
                            this.log.Info("The IP address count was {0}, expected count is 1, updating the DNS records.", ipCount);
                            dnsUpdater.UpdateDnsRecords(domain.Name, publicIp);
                        }

                        var domainIp = domainIps.Single();
                        if (!publicIp.Equals(domainIp))
                        {
                            this.log.Info("The domain points to an IP address {0} which is not the expected public IP, updating the DNS records.", domainIp);
                            dnsUpdater.UpdateDnsRecords(domain.Name, publicIp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.log.Error(ex);
            }
        }

        private IPAddress[] ResolveDomainIp(string domain)
        {
            this.log.Info("Resolving DNS entry for the domain {0}", domain);
            var hostEntry = Dns.GetHostEntry(domain);

            return hostEntry.AddressList;
        }

        public void Dispose()
        {
            this.timer.Dispose();
        }
    }
}