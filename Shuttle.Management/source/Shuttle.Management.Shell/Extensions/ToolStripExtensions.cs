using System;
using System.Drawing;
using System.Windows.Forms;

namespace Shuttle.Management.Shell
{
	public static class ToolStripExtensions
	{
		public static void AddItem(this ToolStrip toolstrip, string text, EventHandler onClick)
		{
			toolstrip.AddItem(text, null, onClick);
		}

		public static void AddItem(this ToolStrip toolstrip, string text, Image image, EventHandler onClick)
		{
			toolstrip.Items.Add(text, image, onClick).RightToLeft = RightToLeft.No;
		}
	}
}