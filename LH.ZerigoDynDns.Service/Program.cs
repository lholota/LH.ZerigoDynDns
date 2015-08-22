namespace LH.ZerigoDynDns.Service
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.ServiceProcess;
    using CommandLine;
    using Configuration;
    using NLog;

    public static class Program
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public static void Main(string[] cmdLineArgs)
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
                    if (args.ConfigCheck)
                    {
                        VerifyConfiguration();
                    }
                    else if (args.RunInConsole)
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

        private static void VerifyConfiguration()
        {
            var section = ZerigoDynDnsSection.LoadFromConfig();

            if (section == null)
            {
                var assemblyName = Path.GetFileName(Assembly.GetExecutingAssembly().Location);
                Console.WriteLine("The configuration/zerigoDynDns element is missing in the {0}.exe.config.", assemblyName);
            }
            else
            {
                var errors = section
                    .ValidateConfiguration()
                    .ToArray();

                if (!errors.Any())
                {
                    Console.WriteLine("The configuration validation was successful.");
                }
                else
                {
                    foreach (var error in errors)
                    {
                        Console.WriteLine(error);
                    }
                }
            }
        }
    }
}