using System;
using System.Drawing;
using System.Windows.Forms;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Management.Shell
{
	public abstract class ManagementModulePresenter : IManagementModulePresenter
	{
        public TaskQueue TaskQueue { get; internal set; }
        public IManagementConfiguration ManagementConfiguration { get; internal set; }
        public abstract string Text { get; }
        public abstract Image Image { get; }
        public abstract UserControl ViewUserControl { get; }

        public void QueueTask(string name, Action action)
        {
            TaskQueue.QueueTask(name, action);
        }

        public virtual void OnViewReady()
        {
        }

        public void HideView()
        {
			if (ViewUserControl == null)
			{
				Log.Error(string.Format(ManagementResources.ViewUserControlNull, GetType().FullName));

				return;
			}

			ViewUserControl.Visible = false;
        }

        public void ShowView()
        {
			if (ViewUserControl == null)
			{
				Log.Error(string.Format(ManagementResources.ViewUserControlNull, GetType().FullName));

				return;
			}

        	ViewUserControl.Visible = true;
        }
    }
}