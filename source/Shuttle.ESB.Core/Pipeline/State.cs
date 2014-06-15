using System.Collections.Generic;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class State<TOwner>
	{
		private readonly TOwner owner;
		private readonly Dictionary<string, object> state = new Dictionary<string, object>();

		public State(TOwner owner)
		{
			this.owner = owner;
		}

		public void Clear()
		{
			state.Clear();
		}

		public TOwner Add(object value)
		{
			Guard.AgainstNull(value, "value");

			state.Add(value.GetType().FullName, value);

			return owner;
		}

		public TOwner Add(string key, object value)
		{
			Guard.AgainstNull(key, "key");

			state.Add(key, value);

			return owner;
		}

		public TOwner Add<TItem>(TItem value)
		{
			state.Add(typeof (TItem).FullName, value);

			return owner;
		}

		public TOwner Add<TItem>(string key, TItem value)
		{
			Guard.AgainstNull(key, "key");

			state.Add(key, value);

			return owner;
		}

		public TOwner Replace(object value)
		{
			Guard.AgainstNull(value, "value");

			var key = value.GetType().FullName;

			state.Remove(key);
			state.Add(key, value);

			return owner;
		}

		public TOwner Replace(string key, object value)
		{
			Guard.AgainstNull(key, "key");

			state.Remove(key);
			state.Add(key, value);

			return owner;
		}

		public TOwner Replace<TItem>(TItem value)
		{
			var key = typeof (TItem).FullName;

			state.Remove(key);
			state.Add(key, value);

			return owner;
		}

		public TOwner Replace<TItem>(string key, TItem value)
		{
			Guard.AgainstNull(key, "key");

			state.Remove(key);
			state.Add(key, value);

			return owner;
		}

		public TItem Get<TItem>()
		{
			return Get<TItem>(typeof (TItem).FullName);
		}

		public TItem Get<TItem>(string key)
		{
			Guard.AgainstNull(key, "key");

			if (!Contains(key))
			{
				return default(TItem);
			}

			return (TItem)state[key];
		}

		public bool Contains(string key)
		{
			Guard.AgainstNull(key, "key");

			return state.ContainsKey(key);
		}
	}

	public class State : State<object>
	{
		public State() : base(null)
		{
		}
	}
}