using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Management.Shell
{
	public class SecureXmlDataStoreRepository : IDataStoreRepository
	{
		private readonly IObjectSerializer serializer;
		private readonly string dataStoreRepositoryPath;
		private readonly string key;
		private readonly XmlDataStoreCollection dataStores;
		private readonly ICryptographyService cryptography = new CryptographyService();

		public SecureXmlDataStoreRepository()
		{
			key = ConfigurationItem<string>.ReadSetting("SecureXmlDataStoreRepositoryKey").GetValue();
			dataStoreRepositoryPath = ConfigurationItem<string>.ReadSetting("SecureXmlDataStoreRepositoryPath").GetValue();

			serializer = new XmlObjectSerializer();

			if (!File.Exists(dataStoreRepositoryPath))
			{
				dataStores = new XmlDataStoreCollection();

				File.WriteAllText(dataStoreRepositoryPath, serializer.Serialize(dataStores));

				return;
			}

			dataStores = serializer.Deserialize<XmlDataStoreCollection>(File.ReadAllText(dataStoreRepositoryPath));

			PersistStore(); // force encryption for any insecure connection strings
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

			EncryptStoresInMemory();

			File.WriteAllText(dataStoreRepositoryPath, serializer.Serialize(dataStores));

			DecryptStoresInMemory();
		}

		private void EncryptStoresInMemory()
		{
			foreach (var dataStore in dataStores.Where(dataStore => !dataStore.Secure))
			{
				dataStore.ConnectionString = cryptography.TripleDESEncrypt(dataStore.ConnectionString, key);
				dataStore.Secure = true;
			}
		}

		private void DecryptStoresInMemory()
		{
			foreach (var dataStore in dataStores)
			{
				dataStore.ConnectionString = cryptography.TripleDESDecrypt(dataStore.ConnectionString, key);
				dataStore.Secure = false;
			}
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