namespace LH.ZerigoDynDns.Service
{
    using CommandLine;

    internal class Args
    {
        [Option('c', "console", Required = false, DefaultValue = false, 
            HelpText = "Input files to be processed.")]
        public bool RunInConsole { get; set; }

        [Option("config-check", Required = false, DefaultValue = false,
            HelpText = "Only verifies the configuration is correct and complete.")]
        public bool ConfigCheck { get; set; }
    }
}