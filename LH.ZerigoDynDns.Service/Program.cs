namespace LH.ZerigoDynDns.Service
{
    using System;
    using System.ServiceProcess;
    using CommandLine;
    using NLog;

    static class Program
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        static void Main(string[] cmdLineArgs)
        {
            try
            {
                var args = new Args();
                var parser = new Parser();

                if (!parser.ParseArguments(cmdLineArgs, args))
                {
                    log.Fatal("Command line args could not be parsed.");
                }
                else
                {
                    if (args.RunInConsole)
                    {
                        RunInConsole();
                    }
                    else
                    {
                        RunAsService();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Fatal(ex);
            }
        }

        private static void RunAsService()
        {           
            ServiceBase.Run(new ServiceWrapper());
        }

        private static void RunInConsole()
        {
            using (var updateService = new DnsUpdateService())
            {
                updateService.Start();
                log.Info("Service started");

                Console.WriteLine("Press any key to quit...");
                Console.ReadKey(true);

                updateService.Stop();
            }
        }
    }
}