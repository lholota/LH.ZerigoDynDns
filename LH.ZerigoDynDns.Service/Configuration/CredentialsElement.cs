namespace LH.ZerigoDynDns.Service.Configuration
{
    using System.Configuration;

    public class CredentialsElement : ConfigurationElement
    {
        [ConfigurationProperty("userName", IsRequired = true)]
        public string UserName
        {
            get
            {
                return (string)this["userName"];
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
                return (string)this["apiKey"];
            }

            set
            {
                this["apiKey"] = value;
            }
        }
    }
}