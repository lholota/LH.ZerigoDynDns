namespace LH.ZerigoDynDns.Service
{
    using CommandLine;

    internal class Args
    {
        [Option('c', "console", Required = false, DefaultValue = false, 
            HelpText = "Run the application in console mode.")]
        public bool RunInConsole { get; set; }

        [Option("check-config", Required = false, DefaultValue = false,
            HelpText = "Only verifies the configuration is correct and complete. The application will run in console mode.")]
        public bool ConfigCheck { get; set; }
    }
}