using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Management.Shell
{
    public class XmlDataStoreRepository : IDataStoreRepository
    {
        private readonly IObjectSerializer serializer;
        private readonly string dataStoreRepositoryPath;
        private readonly XmlDataStoreCollection dataStores;

        public XmlDataStoreRepository()
        {
            dataStoreRepositoryPath = ConfigurationItem<string>.ReadSetting("XmlDataStoreRepositoryPath").GetValue();

            serializer = new XmlObjectSerializer();

            if (!File.Exists(dataStoreRepositoryPath))
            {
                dataStores = new XmlDataStoreCollection();

                File.WriteAllText(dataStoreRepositoryPath, serializer.Serialize(dataStores));

                return;
            }

            dataStores = serializer.Deserialize<XmlDataStoreCollection>(File.ReadAllText(dataStoreRepositoryPath));
        }

        public IEnumerable<DataStore> All()
        {
            return new ReadOnlyCollection<DataStore>(dataStores);
        }

        public void Save(DataStore dataStore)
        {
            Guard.AgainstNull(dataStore, "dataStore");

            var store = Get(dataStore.Name);

            if (store != null)
            {
                dataStores.Remove(store);
            }

            dataStores.Add(dataStore);

            PersistStore();
        }

        private void PersistStore()
        {
            dataStores.Sort();

            File.WriteAllText(dataStoreRepositoryPath, serializer.Serialize(dataStores));
        }

        public void Remove(string name)
        {
            var dataStore = Get(name);

            if (dataStore == null)
            {
                return;
            }

            dataStores.Remove(dataStore);

            PersistStore();
        }

        public bool Contains(string name)
        {
            return Get(name) != null;
        }

        public DataStore Get(string name)
        {
            return dataStores.Find(dataStore => dataStore.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}