using System;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Management.Shell
{
	public class ManagementConfiguration : IManagementConfiguration
	{
		private IQueueRepository queueRepository;
		private readonly ConfigurationItem<string> queueRepositoryType;

		private IDataStoreRepository dataStoreRepository;
		private readonly ConfigurationItem<string> dataStoreRepositoryType;

		public ManagementConfiguration()
		{
			queueRepositoryType = ConfigurationItem<string>.ReadSetting("QueueRepositoryType", string.Empty);
			queueRepository = new NullQueueRepository();
			HasQueueRepository = false;

			dataStoreRepositoryType = ConfigurationItem<string>.ReadSetting("DataStoreRepositoryType", string.Empty);
			dataStoreRepository = new NullDataStoreRepository();
			HasDataStoreRepository = false;
		}

		public string QueueRepositoryType
		{
			get { return queueRepositoryType.GetValue(); }
		}

		public bool HasQueueRepository { get; private set; }

		public IQueueRepository QueueRepository()
		{
			return queueRepository;
		}

		public string DataStoreRepositoryType
		{
			get { return dataStoreRepositoryType.GetValue(); }
		}

		public bool HasDataStoreRepository { get; private set; }

		public IDataStoreRepository DataStoreRepository()
		{
			return dataStoreRepository;
		}

		public void Initialize()
		{
			InitializeQueueRepository();
			InitializeDataStoreRepository();
		}

		private void InitializeQueueRepository()
		{
			Log.Debug("Initializing QueueRepositoryType.");

			if (string.IsNullOrEmpty(QueueRepositoryType))
			{
				Log.Warning(ManagementResources.NoQueueRepositoryTypeSpecified);

				return;
			}

			Log.Information(string.Format(ManagementResources.AttemptCreationOfQueueRepository, QueueRepositoryType));

			try
			{
				var type = new ReflectionService().GetType(
					candidate =>
					candidate.Name.Equals(QueueRepositoryType, StringComparison.InvariantCultureIgnoreCase)
					||
					candidate.AssemblyQualifiedName.Equals(QueueRepositoryType, StringComparison.InvariantCultureIgnoreCase));

				if (type == null)
				{
					Log.Error(string.Format(ManagementResources.QueueRepositoryTypeNotFound, QueueRepositoryType));

					return;
				}

				queueRepository = (IQueueRepository)Activator.CreateInstance(type);

				HasQueueRepository = true;

				Log.Information(string.Format(ManagementResources.QueueRepositoryCreationSuccessful, QueueRepositoryType));
			}
			catch (Exception ex)
			{
				Log.Error(string.Format(ManagementResources.QueueRepositoryCreationException, QueueRepositoryType, ex.CompactMessages()));
			}
		}

		private void InitializeDataStoreRepository()
		{
			Log.Debug("Initializing DataStoreRepositoryType.");

			if (string.IsNullOrEmpty(DataStoreRepositoryType))
			{
				Log.Warning(ManagementResources.NoDataStoreRepositoryTypeSpecified);

				return;
			}

			Log.Information(string.Format(ManagementResources.AttemptCreationOfDataStoreRepository, DataStoreRepositoryType));

			try
			{
				var type = new ReflectionService().GetType(
					candidate =>
					candidate.Name.Equals(DataStoreRepositoryType, StringComparison.InvariantCultureIgnoreCase)
					||
					candidate.AssemblyQualifiedName.Equals(DataStoreRepositoryType, StringComparison.InvariantCultureIgnoreCase));

				if (type == null)
				{
					Log.Error(string.Format(ManagementResources.DataStoreRepositoryTypeNotFound, DataStoreRepositoryType));

					return;
				}

				dataStoreRepository = (IDataStoreRepository)Activator.CreateInstance(type);

				HasDataStoreRepository = true;

				Log.Information(string.Format(ManagementResources.DataStoreRepositoryCreationSuccessful, DataStoreRepositoryType));
			}
			catch (Exception ex)
			{
				Log.Error(string.Format(ManagementResources.DataStoreRepositoryCreationException, DataStoreRepositoryType, ex.CompactMessages()));
			}
		}
	}
}