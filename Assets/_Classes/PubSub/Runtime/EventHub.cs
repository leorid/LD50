using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace JL
{
	public static class EventHub
	{
		public static class Global
		{
			static Dictionary<string, List<MessageBase>> _globalEvents =
				new Dictionary<string, List<MessageBase>>();

			[RuntimeInitializeOnLoadMethod(
				RuntimeInitializeLoadType.SubsystemRegistration)]
			static void DomainReload()
			{
				_globalEvents.Clear();
			}

			static List<MessageBase> GetOrCreateList(string eventName)
			{
				List<MessageBase> list;
				if (!_globalEvents.TryGetValue(eventName, out list))
				{
					list = GenericPool<List<MessageBase>>.Get();
					_globalEvents.Add(eventName, list);
				}
				return list;
			}
			public static void Subscribe(object sender, string eventName, Action action)
			{
				List<MessageBase> list = GetOrCreateList(eventName);
				Message message = GenericPool<Message>.Get();
				message.Init(action);
				list.Add(message);
			}
			public static void Subscribe<T1>(object sender, string eventName, Action<T1> action)
			{
				List<MessageBase> list = GetOrCreateList(eventName);
				Message<T1> message = GenericPool<Message<T1>>.Get();
				message.Init(action);
				list.Add(message);
			}

			static void CheckRemoveList(string eventName, List<MessageBase> list)
			{
				if (list.Count == 0)
				{
					_globalEvents.Remove(eventName);
					GenericPool<List<MessageBase>>.Release(list);
				}
			}
			static List<MessageBase> TryGetList(string eventName)
			{
				List<MessageBase> list;
				if (!_globalEvents.TryGetValue(eventName, out list));
				return list;
			}
			public static void UnSubscribe(object sender, string eventName, Action action)
			{
				List<MessageBase> list = TryGetList(eventName);

				foreach (MessageBase message in list)
				{
					if (message.HasSameAction(action))
					{
						list.Remove(message);
						break;
					}
				}
				CheckRemoveList(eventName, list);
			}
			public static void UnSubscribe<T1>(object sender, string eventName, Action<T1> action)
			{
				List<MessageBase> list = TryGetList(eventName);

				foreach (MessageBase message in list)
				{
					if (message.HasSameAction(action))
					{
						list.Remove(message);
						break;
					}
				}
				CheckRemoveList(eventName, list);
			}

			public static void Execute(object sender, string eventName)
			{
				List<MessageBase> list;
				if (!_globalEvents.TryGetValue(eventName, out list)) return;

				foreach (Message action in list)
				{
					action.Invoke();
				}
			}

			public static void Execute<T1>(object sender, string eventName, T1 arg1)
			{
				List<MessageBase> list;
				if (!_globalEvents.TryGetValue(eventName, out list)) return;

				foreach (Message<T1> action in list)
				{
					action.Invoke(arg1);
				}
			}
		}

		public static class Local
		{
			static Dictionary<object, Dictionary<string, List<MessageBase>>> _localEvents =
				new Dictionary<object, Dictionary<string, List<MessageBase>>>();

			[RuntimeInitializeOnLoadMethod(
				RuntimeInitializeLoadType.SubsystemRegistration)]
			static void DomainReload()
			{
				_localEvents.Clear();
			}


			public static void Subscribe(object sender, GameObject receiver,
				string eventName, Action action)
				=> Subscribe(sender, receiver as object, eventName, action);
			public static void Subscribe(object sender, ScriptableObject receiver,
				string eventName, Action action)
				=> Subscribe(sender, receiver as object, eventName, action);
			public static void Subscribe(object sender, object receiver,
				string eventName, Action action)
			{

			}


			public static void UnSubscribe(object sender, GameObject receiver,
				string eventName, Action action)
			{

			}


			public static void Execute(object sender, string eventName)
			{

			}

		}

	}

	public abstract class MessageBase
	{
		public abstract Delegate Delegate { get; }
		public abstract bool HasSameAction(object otherAction);
	}
	public class Message : MessageBase
	{
		Action _action;
		public override Delegate Delegate => _action;

		public void Init(Action action)
		{
			_action = action;
		}
		public void Invoke()
		{
			_action.Invoke();
		}
		public override bool HasSameAction(object action)
		{
			return _action.Equals(action);
		}
	}
	public class Message<T1> : MessageBase
	{
		Action<T1> _action;
		public override Delegate Delegate => _action;

		public void Init(Action<T1> action)
		{
			_action = action;
		}
		public void Invoke(T1 arg1)
		{
			_action.Invoke(arg1);
		}
		public override bool HasSameAction(object action)
		{
			return _action.Equals(action);
		}
	}

	public class Message<T1, T2> : MessageBase
	{
		Action<T1, T2> _action;
		public override Delegate Delegate => _action;

		public void Init(Action<T1, T2> action)
		{
			_action = action;
		}
		public void Invoke(T1 arg1, T2 arg2)
		{
			_action.Invoke(arg1, arg2);
		}
		public override bool HasSameAction(object action)
		{
			return _action.Equals(action);
		}
	}




	public static partial class ML
	{

	}
}
