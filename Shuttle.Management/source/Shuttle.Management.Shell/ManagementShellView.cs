using System;
using System.Drawing;
using System.Windows.Forms;
using log4net.Core;

namespace Shuttle.Management.Shell
{
	public partial class ManagementShellView : Form, IManagementShellView
	{
		private ManagementModulePresenter managementModulePresenter;
		private readonly IManagementShellPresenter presenter;
		private bool disposing;

		public ManagementShellView()
		{
			InitializeComponent();

			presenter = new ManagementShellPresenter(this);
		}

		private void ManagementShellView_Load(object sender, EventArgs e)
		{
			presenter.OnViewReady();
		}

		public void LogMessage(LoggingEvent loggingEvent)
		{
			if (disposing)
			{
				return;
			}

			LogContainer.BeginInvoke(
				() =>
				{
					LogContainer.SelectionStart = LogContainer.TextLength;
					LogContainer.SelectionLength = 0;

					if (loggingEvent.Level == Level.Fatal)
					{
						LogContainer.SelectionColor = Color.DarkRed;
					}
					else if (loggingEvent.Level == Level.Error)
					{
						LogContainer.SelectionColor = Color.Red;
					}
					else if (loggingEvent.Level == Level.Warn)
					{
						LogContainer.SelectionColor = Color.Yellow;
					}
					else if (loggingEvent.Level == Level.Debug)
					{
						LogContainer.SelectionColor = Color.Gray;
					}
					else
					{
						LogContainer.SelectionColor = Color.LightGreen;
					}

					LogContainer.AppendText(string.Format("[{0}]: {1} - {2}\r\n",
														  DateTime.Now.ToString("HH:mm:ss.ffff"),
														  loggingEvent.LoggerName,
														  loggingEvent.RenderedMessage));
					LogContainer.SelectionColor = LogContainer.ForeColor;
					LogContainer.SelectionLength = 0;
					LogContainer.SelectionStart = LogContainer.Text.Length;
					LogContainer.ScrollToCaret();
				});
		}

		public void AddManagementModulePresenter(ManagementModulePresenter modulePresenter)
		{
			PresenterStrip.Items
				.Add(modulePresenter.Text,
					 modulePresenter.Image,
					 ManagementModulePresenterSelected)
				.Tag = modulePresenter;

			var control = modulePresenter.ViewUserControl;

			control.Dock = DockStyle.Fill;
			control.Visible = false;

			SplitManagementView.Panel1.Controls.Add(control);
		}

		private void ManagementModulePresenterSelected(object sender, EventArgs e)
		{
			if (managementModulePresenter != null)
			{
				managementModulePresenter.HideView();
			}

			var item = (ToolStripItem)sender;

			managementModulePresenter = (ManagementModulePresenter)item.Tag;

			managementModulePresenter.ShowView();
		}

		private void ManagementShellView_FormClosing(object sender, FormClosingEventArgs e)
		{
			disposing = true;

			presenter.Dispose();
		}
	}
}