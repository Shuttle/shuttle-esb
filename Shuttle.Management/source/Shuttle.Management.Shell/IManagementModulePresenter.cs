using System;
using System.Drawing;
using System.Windows.Forms;

namespace Shuttle.Management.Shell
{
	public interface IManagementModulePresenter
	{
		TaskQueue TaskQueue { get; }
		IManagementConfiguration ManagementConfiguration { get; }
		string Text { get; }
		Image Image { get; }
		UserControl ViewUserControl { get; }
		void QueueTask(string name, Action action);
		void OnViewReady();
		void HideView();
		void ShowView();
	}
}