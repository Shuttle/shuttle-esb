using System;
using System.Windows.Forms;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Management.Shell
{
	public static class ControlExtensions
	{
		public static void Drop(this ComboBox combobox)
		{
			combobox.DroppedDown = true;
		}

		public static void Invoke(this Control control, Action action)
		{
			Guard.AgainstNull(control, "control");
			Guard.AgainstNull(action, "action");

			if (control.InvokeRequired)
			{
				control.Invoke(new MethodInvoker(action), null);
			}
			else
			{
				action.Invoke();
			}
		}

		public static void BeginInvoke(this Control control, Action action)
		{
			Guard.AgainstNull(control, "control");
			Guard.AgainstNull(action, "action");

			if (control.InvokeRequired)
			{
				control.BeginInvoke(new MethodInvoker(action), null);
			}
			else
			{
				action.Invoke();
			}
		}
	}
}