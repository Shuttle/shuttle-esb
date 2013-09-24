using System.Configuration;

namespace Shuttle.ESB.Core
{
    public class TripleDESSection : ConfigurationSection
    {
        public static ServiceBusSection Open(string file)
        {
            return ConfigurationManager
                       .OpenMappedMachineConfiguration(new ConfigurationFileMap(file))
                       .GetSection("tripleDES") as ServiceBusSection;
        }

        private static readonly ConfigurationProperty key =
            new ConfigurationProperty("key", typeof (string), null,ConfigurationPropertyOptions.None);


        public TripleDESSection()
        {
            base.Properties.Add(key);
        }

        [ConfigurationProperty("key", IsRequired = false)]
        public string Key
        {
            get { return (string) this[key]; }
        }
    }
}