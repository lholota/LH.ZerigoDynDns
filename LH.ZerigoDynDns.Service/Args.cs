using CommandLine;

namespace Net.DynDnsUpdate
{
    class Args
    {
        [Option('c', "console", Required = false, DefaultValue = false, 
            HelpText = "Input files to be processed.")]
        public bool RunInConsole { get; set; }
    }
}