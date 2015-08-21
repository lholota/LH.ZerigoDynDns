namespace LH.ZerigoDynDns.Service.Configuration
{
    using System.Configuration;

    public class DomainElement : ConfigurationElement
    {

        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        //[ConfigurationProperty("updateIpv4", IsRequired = true)]
        //public bool UpdateIpv4
        //{
        //    get
        //    {
        //        return (bool)this["updateIpv4"];
        //    }
        //    set
        //    {
        //        this["updateIpv4"] = value;
        //    }
        //}

        //[ConfigurationProperty("updateIpv6", IsRequired = false)]
        //public bool UpdateIpv6
        //{
        //    get
        //    {
        //        return (bool)this["updateIpv6"];
        //    }
        //    set
        //    {
        //        this["updateIpv6"] = value;
        //    }
        //}
    }
}