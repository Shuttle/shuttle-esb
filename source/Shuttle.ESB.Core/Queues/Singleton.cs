using System;

namespace Shuttle.ESB.Core.Queues
{
	public class Singleton<TClass>
		where TClass : class, new()
	{
		private static readonly object _padlock = new object();
		private static TClass _instance;

		public static TClass Instance
		{
			get
			{
				lock (_padlock)
				{
					if (_instance == null)
					{
						_instance = new TClass();
						AppDomain.CurrentDomain.ProcessExit += (sender, args) =>
							{
								if (_instance as IDisposable != null)
									(_instance as IDisposable).Dispose();
							};						
					}
				}
				return _instance;
			}
		}

	}
}