namespace LH.ZerigoDynDns.Service.Configuration
{
    using System.Collections.Generic;
    using System.Configuration;

    public sealed class ZerigoDynDnsSection : ConfigurationSection
    {
        [ConfigurationProperty("checkIntervalInSeconds", IsRequired = true)]
        public int CheckIntervalInSeconds
        {
            get
            {
                return (int)this["checkIntervalInSeconds"];
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
                return (CredentialsElement)this["credentials"];
            }
        }

        [ConfigurationProperty("domains")]
        [ConfigurationCollection(typeof(DomainElement), AddItemName = "domain")]
        public DomainsElementCollection Domains
        {
            get
            {
                return (DomainsElementCollection)this["domains"];
            }
        }

        public IEnumerable<string> ValidateConfiguration()
        {
            if (this.CheckIntervalInSeconds == 0)
            {
                yield return "The CheckIntervalInSeconds has to be configured.";
            }

            if (this.Credentials == null)
            {
                yield return "The credentials element is missing.";
            }
            else
            {
                if (string.IsNullOrEmpty(this.Credentials.UserName))
                {
                    yield return "The Credentials/UserName is not set.";
                }

                if (string.IsNullOrEmpty(this.Credentials.ApiKey))
                {
                    yield return "The Credentials/ApiKey is not set.";
                }
            }

            if (this.Domains == null)
            {
                yield return "The domains element is missing.";
            }
            else
            {
                if (this.Domains.Count == 0)
                {
                    yield return "No domain elements have been configured.";
                }
            }
        }
    }
}
