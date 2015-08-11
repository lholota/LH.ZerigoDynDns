using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Timers;
using NLog;

namespace Net.DynDnsUpdate
{
    class DnsUpdateService
    {
        private readonly Timer _timer = new Timer();
        private readonly ILogger _log = LogManager.GetCurrentClassLogger();

        public void Start()
        {
            _log.Info("Starting the DNS Update service");

            var secondsString = ConfigurationManager.AppSettings[AppSettingsKeys.CheckIntervalSeconds];
            var miliSeconds = double.Parse(secondsString)*1000;

            _timer.Interval = miliSeconds;
            _timer.Elapsed += TimerOnElapsed;
            _timer.Start();

            _log.Info("The DNS Update service has been started successfully");
        }

        public void Stop()
        {
            _log.Info("Stopping the DNS Update service");
            _timer.Stop();
            _log.Info("The DNS Update service has been stopped");
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            try
            {
                using (var client = new WebClient())
                {
                    var publicIpRetriever = new PublicIpRetriever(client);
                    var dnsUpdater = new ZerigoDnsUpdater(client);

                    var publicIp = publicIpRetriever.GetPublicIp(client);
                    var domainIps = ResolveDomainIp()
                        .Select(x => x.MapToIPv4())
                        .Distinct()
                        .ToArray();

                    _log.Info("The public IP address is {0}.", publicIp);

                    var ipCount = domainIps.Count();
                    if (ipCount != 1)
                    {
                        _log.Info("The IP address count was {0}, expected count is 1, updating the DNS records.", ipCount);
                        dnsUpdater.UpdateDnsRecords(publicIp);
                    }

                    var domainIp = domainIps.Single();
                    if (!publicIp.Equals(domainIp))
                    {
                        _log.Info("The domain points to an IP address {0} which is not the expected public IP, updating the DNS records.", domainIp);
                        dnsUpdater.UpdateDnsRecords(publicIp);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }      

        private IPAddress[] ResolveDomainIp()
        {
            var domain = ConfigurationManager.AppSettings[AppSettingsKeys.Domain];
            _log.Info("Resolving DNS entry for the domain {0}", domain);

            var hostEntry = Dns.GetHostEntry(domain);

            return hostEntry.AddressList;
        }
    }
}