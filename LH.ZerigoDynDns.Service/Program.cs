using System;
using System.ServiceProcess;
using CommandLine;
using NLog;

namespace Net.DynDnsUpdate
{
    static class Program
    {
        private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

        static void Main(string[] cmdLineArgs)
        {
            try
            {
                var args = new Args();
                var parser = new Parser();

                if (!parser.ParseArguments(cmdLineArgs, args))
                {
                    Log.Fatal("Command line args could not be parsed.");
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
                Log.Fatal(ex);
            }
        }

        private static void RunAsService()
        {           
            ServiceBase.Run(new ServiceWrapper());
        }

        private static void RunInConsole()
        {
            var updateService = new DnsUpdateService();
            updateService.Start();
            Log.Info("Service started");

            Console.WriteLine("Press any key to quit...");
            Console.ReadKey(true);

            updateService.Stop();
        }
    }
}