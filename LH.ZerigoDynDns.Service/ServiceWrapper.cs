namespace LH.ZerigoDynDns.Service
{
    using System.ServiceProcess;

    public partial class ServiceWrapper : ServiceBase
    {
        private readonly DnsUpdateService updateService;

        public ServiceWrapper()
        {
            this.InitializeComponent();
            this.updateService = new DnsUpdateService();
        }

        protected override void OnStart(string[] args)
        {
            this.updateService.Start();
        }

        protected override void OnStop()
        {
            this.updateService.Stop();
        }
    }
}
