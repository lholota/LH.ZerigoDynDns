using System.Configuration;

namespace LH.ZerigoDynDns.Service.Configuration
{
    public class ZerigoDynDns
    {
        private static ZerigoDynDnsSection _config;

        static ZerigoDynDns()
        {
            _config = ((ZerigoDynDnsSection)(ConfigurationManager.GetSection("zerigoDynDns")));
        }

        private ZerigoDynDns()
        {
        }

        public static ZerigoDynDnsSection Config
        {
            get
            {
                return _config;
            }
        }
    }

    public sealed class ZerigoDynDnsSection : ConfigurationSection
    {

        [ConfigurationProperty("checkIntervalInSeconds", IsRequired = true)]
        public long CheckIntervalInSeconds
        {
            get
            {
                return ((long)(this["checkIntervalInSeconds"]));
            }
            set
            {
                this["checkIntervalInSeconds"] = value;
            }
        }

        [ConfigurationProperty("credentials")]
        public CredentialsElement Credentials
        {
            get
            {
                return ((CredentialsElement)(this["credentials"]));
            }
        }

        [ConfigurationProperty("domains")]
        [ConfigurationCollection(typeof(DomainsElementCollection.DomainElement), AddItemName = "domain")]
        public DomainsElementCollection Domains
        {
            get
            {
                return ((DomainsElementCollection)(this["domains"]));
            }
        }

        public  class CredentialsElement : ConfigurationElement
        {

            [ConfigurationProperty("userName", IsRequired = true)]
            public string UserName
            {
                get
                {
                    return ((string)(this["userName"]));
                }
                set
                {
                    this["userName"] = value;
                }
            }

            [ConfigurationProperty("apiKey", IsRequired = true)]
            public string ApiKey
            {
                get
                {
                    return ((string)(this["apiKey"]));
                }
                set
                {
                    this["apiKey"] = value;
                }
            }
        }

        public  class DomainsElementCollection : ConfigurationElementCollection
        {

            public DomainElement this[int i]
            {
                get
                {
                    return ((DomainElement)(this.BaseGet(i)));
                }
            }

            protected override ConfigurationElement CreateNewElement()
            {
                return new DomainElement();
            }

            protected override object GetElementKey(ConfigurationElement element)
            {
                return ((DomainElement)(element)).Name;
            }

            public  class DomainElement : ConfigurationElement
            {

                [ConfigurationProperty("name", IsRequired = true)]
                public string Name
                {
                    get
                    {
                        return ((string)(this["name"]));
                    }
                    set
                    {
                        this["name"] = value;
                    }
                }

                [ConfigurationProperty("updateIpv4", IsRequired = true)]
                public bool UpdateIpv4
                {
                    get
                    {
                        return ((bool)(this["updateIpv4"]));
                    }
                    set
                    {
                        this["updateIpv4"] = value;
                    }
                }

                [ConfigurationProperty("updateIpv6", IsRequired = false)]
                public bool UpdateIpv6
                {
                    get
                    {
                        return ((bool)(this["updateIpv6"]));
                    }
                    set
                    {
                        this["updateIpv6"] = value;
                    }
                }
            }
        }
    }

}
