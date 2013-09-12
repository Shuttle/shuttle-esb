using System;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Windows.Forms;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Management.Shell
{
	public class DataStorePresenter : ManagementModulePresenter, IDataStorePresenter
	{
		private readonly IDataStoreView view;

		public DataStorePresenter()
		{
			view = new DataStoreView(this);
		}

		public override string Text
		{
			get { return ManagementResources.Text_DataStores; }
		}

		public override Image Image
		{
			get { return ManagementResources.Image_DataStores; }
		}

		public override UserControl ViewUserControl
		{
			get
			{
				var control = (UserControl)view;

				control.Enabled = ManagementConfiguration.HasDataStoreRepository;

				return control;
			}
		}

		public override void OnViewReady()
		{
			base.OnViewReady();

		    QueueTask("PopulateProviders",
		              () =>
		                  {
		                      using (var providers = DbProviderFactories.GetFactoryClasses())
		                      {
		                          foreach (DataRow row in providers.Rows)
		                          {
		                              view.AddProviderName(Convert.ToString(row["InvariantName"]));
		                          }
		                      }
		                  });

            Refresh();
        }

        public void Remove()
        {
            if (!view.HasSelectedDataStores)
            {
                Log.Warning(string.Format(ManagementResources.NoItemsSelected, ManagementResources.Text_DataStores));

                return;
            }

            if (MessageBox.Show(string.Format(ManagementResources.ConfirmRemoval, ManagementResources.Text_DataStores), ManagementResources.Confirmation, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            QueueTask("Remove",
                      () =>
                      {
                          foreach (var store in view.SelectedDataStores())
                          {
                              ManagementConfiguration.DataStoreRepository().Remove(store.Name);
                          }

                      });

            Refresh();
        }

        public void CheckAll()
        {
            view.CheckAll();
        }

        public void InvertChecks()
        {
            view.InvertChecks();
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(view.NameValue))
            {
                Log.Error(string.Format(ManagementResources.ValueMayNotBeEmpty, ManagementResources.Name));

                return;
            }

            if (string.IsNullOrEmpty(view.ConnectionStringValue))
            {
                Log.Error(string.Format(ManagementResources.ValueMayNotBeEmpty, ManagementResources.ConnectionString));

                return;
            }

            if (string.IsNullOrEmpty(view.ProviderNameValue))
            {
                Log.Error(string.Format(ManagementResources.ValueMayNotBeEmpty, ManagementResources.ProviderName));

                return;
            }

            var name = view.NameValue;
            var connectionString = view.ConnectionStringValue;
            var providerName = view.ProviderNameValue;

            QueueTask("AddDataStore",
                      () => ManagementConfiguration.DataStoreRepository().Save(new DataStore
                      {
                          Name = name,
                          ConnectionString = connectionString,
                          ProviderName = providerName
                      }));

            Refresh();
        }

        public void Refresh()
        {
            QueueTask("Refresh",
                () => view.RefreshDataStores(ManagementConfiguration.DataStoreRepository().All()));
        }

	    public void DataStoreSelected()
	    {
	        var store = view.GetSelectedDataStore();

            if (store == null)
            {
                return;
            }

	        view.NameValue = store.Name;
	        view.ConnectionStringValue = store.ConnectionString;
	        view.ProviderNameValue = store.ProviderName;
	    }
	}
}