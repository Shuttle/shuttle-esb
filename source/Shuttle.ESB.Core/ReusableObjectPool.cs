using System;
using System.Collections.Generic;
using Shuttle.Core.Infrastructure;

namespace Shuttle.ESB.Core
{
	public class ReusableObjectPool<TReusableObject>
		where TReusableObject : class
	{
		private static readonly object _padlock = new object();
		private readonly Dictionary<Type, List<TReusableObject>> _pool = new Dictionary<Type, List<TReusableObject>>();
		private readonly Func<Type, TReusableObject> _factoryMethod;

		public ReusableObjectPool()
		{
		}

		public ReusableObjectPool(Func<Type, TReusableObject> factoryMethod)
		{
			Guard.AgainstNull(factoryMethod, "factoryMethod");

			_factoryMethod = factoryMethod;
		}

		public TReusableObject Get(Type key)
		{
			Guard.AgainstNull(key, "key");

			lock (_padlock)
			{
				if (!_pool.ContainsKey(key))
				{
					_pool.Add(key, new List<TReusableObject>());
				}

				if (_pool.Count > 0)
				{
					var reusableObjects = _pool[key];

					if (reusableObjects.Count > 0)
					{
						var reusableObject = reusableObjects[0];

						reusableObjects.RemoveAt(0);

						return reusableObject;
					}
				}

				return _factoryMethod == null ? null : _factoryMethod(key);
			}
		}

		public void Release(TReusableObject instance)
		{
			lock (_padlock)
			{
				if (!_pool.ContainsKey(instance.GetType()))
				{
					_pool.Add(instance.GetType(), new List<TReusableObject>());
				}

				_pool[instance.GetType()].Add(instance);
			}
		}
	}
}