using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

namespace JL
{
	public static class GenericPool
	{
		private static Dictionary<Type, ICollection> _genericPool = 
			new Dictionary<Type, ICollection>();

		public static T Get<T>()
		{
			ICollection value;
			if (_genericPool.TryGetValue(typeof(T), out value))
			{
				var pooledObjects = value as Stack<T>;
				if (pooledObjects.Count > 0)
				{
					return pooledObjects.Pop();
				}
			}
			return Activator.CreateInstance<T>();
		}

		public static void Return<T>(T obj)
		{
			if (obj == null)
			{
				return;
			}

			if(obj is IPoolable)
			{
				(obj as IPoolable).OnReturnToPool();
			}

			ICollection value;
			if (_genericPool.TryGetValue(typeof(T), out value))
			{
				var pooledObjects = value as Stack<T>;
				pooledObjects.Push(obj);
			}
			else
			{
				var pooledObjects = new Stack<T>();
				pooledObjects.Push(obj);
				_genericPool.Add(typeof(T), pooledObjects);
			}
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void DomainReset()
		{
			if (_genericPool != null) _genericPool.Clear();
		}
	}

	public interface IPoolable
	{
		void OnReturnToPool();
	}
}
