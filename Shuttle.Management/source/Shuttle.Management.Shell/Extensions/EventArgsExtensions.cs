using System;
using System.Windows.Forms;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Management.Shell
{
	public static class EventArgsExtensions
	{
		public static void OnEnterPressed(this KeyEventArgs args, Action action)
		{
            args.OnKey(Keys.Enter, action);
        }

        public static void OnKeyDown(this KeyEventArgs args, Action action)
		{
            args.OnKey(Keys.Down, action);
        }

        public static void OnF4(this KeyEventArgs args, Action action)
        {
            args.OnKey(Keys.F4, action);
        }

        public static void OnEscape(this KeyEventArgs args, Action action)
        {
            args.OnKey(Keys.Escape, action);
        }

	    public static void OnKey(this KeyEventArgs args, Keys key, Action action)
        {
            Guard.AgainstNull(args, "args");
            Guard.AgainstNull(action, "action");

            if (args.KeyCode != key)
            {
                return;
            }

            action.Invoke();
        }
	}
}