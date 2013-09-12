using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Shuttle.Management.Shell
{
    public partial class DataStoreView : UserControl, IDataStoreView
    {
        private readonly IDataStorePresenter presenter;

        public DataStoreView(IDataStorePresenter presenter)
        {
            InitializeComponent();

            this.presenter = presenter;

            DataStoreToolStrip.AddItem(ManagementResources.Text_Remove,
                                       ManagementResources.Image_Remove,
                                       delegate { presenter.Remove(); });
            DataStoreToolStrip.AddItem(ManagementResources.Text_Save,
                                       ManagementResources.Image_Save,
                                       delegate { presenter.Save(); });
            DataStoreToolStrip.AddItem(ManagementResources.Text_CheckAll,
                                       delegate { presenter.CheckAll(); });
            DataStoreToolStrip.AddItem(ManagementResources.Text_InvertChecks,
                                       delegate { presenter.InvertChecks(); });
            DataStoreToolStrip.AddItem(ManagementResources.Text_Refresh,
                                       ManagementResources.Image_RefreshSmall,
                                       delegate { presenter.Refresh(); });
        }

        public string NameValue
        {
            get { return StoreName.Text; }
            set { StoreName.Text = value; }
        }

        public string ConnectionStringValue
        {
            get { return ConnectionString.Text; }
            set { ConnectionString.Text = value; }
        }

        public string ProviderNameValue
        {
            get { return ProviderName.Text; }
            set { ProviderName.Text = value; }
        }

        public void AddProviderName(string providerName)
        {
            this.Invoke(() => ProviderName.Items.Add(providerName));
        }

        public bool HasSelectedDataStores
        {
            get { return DataStoreList.SelectedItems.Count > 0 || DataStoreList.CheckedItems.Count > 0; }
        }

        public void CheckAll()
        {
            this.Invoke(() =>
                            {
                                foreach (ListViewItem item in DataStoreList.Items)
                                {
                                    item.Checked = true;
                                }
                            });
        }

        public void InvertChecks()
        {
            this.Invoke(() =>
                            {
                                foreach (ListViewItem item in DataStoreList.Items)
                                {
                                    item.Checked = !item.Checked;
                                }
                            });
        }

        public void RefreshDataStores(IEnumerable<DataStore> list)
        {
            this.Invoke(
                () =>
                    {
                        DataStoreList.Items.Clear();

                        foreach (var store in list)
                        {
                            var item = DataStoreList.Items.Add(store.Name);

                            item.SubItems.Add(store.ConnectionString);
                            item.SubItems.Add(store.ProviderName);

                            item.Tag = store;
                        }
                    });
        }

        public IEnumerable<DataStore> SelectedDataStores()
        {
            var result = new List<DataStore>();

            this.Invoke(
                () =>
                    {
                        if (DataStoreList.CheckedItems.Count > 0)
                        {
                            result.AddRange(from ListViewItem item in DataStoreList.CheckedItems
                                            select (DataStore) item.Tag);
                        }
                        else
                        {
                            result.AddRange(from ListViewItem item in DataStoreList.SelectedItems
                                            select (DataStore) item.Tag);
                        }
                    });

            return result;
        }

        public DataStore GetSelectedDataStore()
        {
            return DataStoreList.SelectedItems.Count == 0
                       ? null
                       : (DataStore) DataStoreList.SelectedItems[0].Tag;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            presenter.OnViewReady();
        }

        private void DataStoreList_SelectedIndexChanged(object sender, EventArgs e)
        {
            presenter.DataStoreSelected();
        }

		private void Save(object sender, KeyEventArgs e)
		{
			e.OnEnterPressed(presenter.Save);
		}
    }
}