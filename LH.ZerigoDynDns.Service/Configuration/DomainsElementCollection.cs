namespace LH.ZerigoDynDns.Service.Configuration
{
    using System.Configuration;

    public  class DomainsElementCollection : ConfigurationElementCollection
    {
        public DomainElement this[int i]
        {
            get
            {
                return (DomainElement)this.BaseGet(i);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new DomainElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DomainElement)element).Name;
        }
    }
}