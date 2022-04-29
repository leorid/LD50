using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace JL
{
	public static class EventHub
	{
		public static class Global
		{
			static Dictionary<string, List<MessageBase>> _globalEvents = new();

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
			public static void Subscribe<T1, T2>(object sender, string eventName, Action<T1, T2> action)
			{
				List<MessageBase> list = GetOrCreateList(eventName);
				Message<T1, T2> message = GenericPool<Message<T1, T2>>.Get();
				message.Init(action);
				list.Add(message);
			}
			public static void Subscribe<T1, T2, T3>(object sender, string eventName, Action<T1, T2, T3> action)
			{
				List<MessageBase> list = GetOrCreateList(eventName);
				Message<T1, T2, T3> message = GenericPool<Message<T1, T2, T3>>.Get();
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
				if (!_globalEvents.TryGetValue(eventName, out list)) ;
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
			public static void UnSubscribe<T1, T2>(object sender, string eventName, Action<T1, T2> action)
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
			public static void UnSubscribe<T1, T2, T3>(object sender, string eventName, Action<T1, T2, T3> action)
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
			public static void Execute<T1, T2>(object sender, string eventName, T1 arg1, T2 arg2)
			{
				List<MessageBase> list;
				if (!_globalEvents.TryGetValue(eventName, out list)) return;

				foreach (Message<T1, T2> action in list)
				{
					action.Invoke(arg1, arg2);
				}
			}
			public static void Execute<T1, T2, T3>(object sender, string eventName, T1 arg1, T2 arg2, T3 arg3)
			{
				List<MessageBase> list;
				if (!_globalEvents.TryGetValue(eventName, out list)) return;

				foreach (Message<T1, T2, T3> action in list)
				{
					action.Invoke(arg1, arg2, arg3);
				}
			}


		}

		public static class Local
		{
			static Dictionary<object, Dictionary<string, List<MessageBase>>> _localEvents = new();

			[RuntimeInitializeOnLoadMethod(
				RuntimeInitializeLoadType.SubsystemRegistration)]
			static void DomainReload()
			{
				_localEvents.Clear();
			}


			static Dictionary<string, List<MessageBase>> GetOrCreateDict(object receiver)
			{
				Dictionary<string, List<MessageBase>> result;
				if (!_localEvents.TryGetValue(receiver, out result))
				{
					result = GenericPool<Dictionary<string, List<MessageBase>>>.Get();
					_localEvents.Add(receiver, result);
				}
				return result;
			}
			static List<MessageBase> GetOrCreateList(object receiver, string eventName)
			{
				Dictionary<string, List<MessageBase>> dict = GetOrCreateDict(receiver);

				List<MessageBase> list;
				if (!dict.TryGetValue(eventName, out list))
				{
					list = GenericPool<List<MessageBase>>.Get();
					dict.Add(eventName, list);
				}
				return list;
			}
			static void CheckRemoveList(object receiver, string eventName, List<MessageBase> list)
			{
				if (list.Count == 0)
				{
					_localEvents[receiver].Remove(eventName);
					GenericPool<List<MessageBase>>.Release(list);

					Dictionary<string, List<MessageBase>> dict = _localEvents[receiver];
					if (dict.Count == 0)
					{
						_localEvents.Remove(receiver);
						GenericPool<Dictionary<string, List<MessageBase>>>.Release(dict);
					}
				}
			}
			static bool TryGetList(object receiver, string eventName, out List<MessageBase> list)
			{
				list = null;
				Dictionary<string, List<MessageBase>> dict;
				if (_localEvents.TryGetValue(receiver, out dict))
				{
					if (dict.TryGetValue(eventName, out list))
					{
						return true;
					}
				}
				return false;
			}

			public static void Subscribe(object sender, GameObject receiver,
				string eventName, Action action)
				=> Subscribe(sender, receiver as object, eventName, action);
			public static void Subscribe(object sender, ScriptableObject receiver,
				string eventName, Action action)
				=> Subscribe(sender, receiver as object, eventName, action);
			static void Subscribe(object sender, object receiver,
				string eventName, Action action)
			{
				List<MessageBase> list = GetOrCreateList(receiver, eventName);
				Message evt = GenericPool<Message>.Get();
				evt.Init(action);
				list.Add(evt);
			}
			public static void Subscribe<T1>(object sender, GameObject receiver,
				string eventName, Action<T1> action)
				=> Subscribe(sender, receiver as object, eventName, action);
			public static void Subscribe<T1>(object sender, ScriptableObject receiver,
				string eventName, Action<T1> action)
				=> Subscribe(sender, receiver as object, eventName, action);
			static void Subscribe<T1>(object sender, object receiver,
				string eventName, Action<T1> action)
			{
				List<MessageBase> list = GetOrCreateList(receiver, eventName);
				Message<T1> evt = GenericPool<Message<T1>>.Get();
				evt.Init(action);
				list.Add(evt);
			}
			public static void Subscribe<T1, T2>(object sender, GameObject receiver,
				string eventName, Action<T1, T2> action)
				=> Subscribe(sender, receiver as object, eventName, action);
			public static void Subscribe<T1, T2>(object sender, ScriptableObject receiver,
				string eventName, Action<T1, T2> action)
				=> Subscribe(sender, receiver as object, eventName, action);
			static void Subscribe<T1, T2>(object sender, object receiver,
				string eventName, Action<T1, T2> action)
			{
				List<MessageBase> list = GetOrCreateList(receiver, eventName);
				Message<T1, T2> evt = GenericPool<Message<T1, T2>>.Get();
				evt.Init(action);
				list.Add(evt);
			}
			public static void Subscribe<T1, T2, T3>(object sender, GameObject receiver,
				string eventName, Action<T1, T2, T3> action)
				=> Subscribe(sender, receiver as object, eventName, action);
			public static void Subscribe<T1, T2, T3>(object sender, ScriptableObject receiver,
				string eventName, Action<T1, T2, T3> action)
				=> Subscribe(sender, receiver as object, eventName, action);
			static void Subscribe<T1, T2, T3>(object sender, object receiver,
				string eventName, Action<T1, T2, T3> action)
			{
				List<MessageBase> list = GetOrCreateList(receiver, eventName);
				Message<T1, T2, T3> evt = GenericPool<Message<T1, T2, T3>>.Get();
				evt.Init(action);
				list.Add(evt);
			}


			public static void UnSubscribe(object sender, GameObject receiver,
				string eventName, Action action)
				=> UnSubscribe(sender, receiver as object, eventName, action);
			public static void UnSubscribe(object sender, ScriptableObject receiver,
				string eventName, Action action)
				=> UnSubscribe(sender, receiver as object, eventName, action);
			static void UnSubscribe(object sender, object receiver,
				string eventName, Action action)
			{
				if (TryGetList(receiver, eventName, out List<MessageBase> list))
				{
					foreach (MessageBase message in list)
					{
						if (message.HasSameAction(action))
						{
							list.Remove(message);
							GenericPool<Message>.Release(message as Message);
							break;
						}
					}
					CheckRemoveList(receiver, eventName, list);
				}
			}
			public static void UnSubscribe<T1>(object sender, GameObject receiver,
				string eventName, Action<T1> action)
				=> UnSubscribe(sender, receiver as object, eventName, action);
			public static void UnSubscribe<T1>(object sender, ScriptableObject receiver,
				string eventName, Action<T1> action)
				=> UnSubscribe(sender, receiver as object, eventName, action);
			static void UnSubscribe<T1>(object sender, object receiver,
				string eventName, Action<T1> action)
			{
				if (TryGetList(receiver, eventName, out List<MessageBase> list))
				{
					foreach (MessageBase message in list)
					{
						if (message.HasSameAction(action))
						{
							list.Remove(message);
							GenericPool<Message<T1>>.Release(message as Message<T1>);
							break;
						}
					}
					CheckRemoveList(receiver, eventName, list);
				}
			}
			public static void UnSubscribe<T1, T2>(object sender, GameObject receiver,
				string eventName, Action<T1, T2> action)
				=> UnSubscribe(sender, receiver as object, eventName, action);
			public static void UnSubscribe<T1, T2>(object sender, ScriptableObject receiver,
				string eventName, Action<T1, T2> action)
				=> UnSubscribe(sender, receiver as object, eventName, action);
			static void UnSubscribe<T1, T2>(object sender, object receiver,
				string eventName, Action<T1, T2> action)
			{
				if (TryGetList(receiver, eventName, out List<MessageBase> list))
				{
					foreach (MessageBase message in list)
					{
						if (message.HasSameAction(action))
						{
							list.Remove(message);
							GenericPool<Message<T1, T2>>.Release(message as Message<T1, T2>);
							break;
						}
					}
					CheckRemoveList(receiver, eventName, list);
				}
			}
			public static void UnSubscribe<T1, T2, T3>(object sender, GameObject receiver,
				string eventName, Action<T1, T2, T3> action)
				=> UnSubscribe(sender, receiver as object, eventName, action);
			public static void UnSubscribe<T1, T2, T3>(object sender, ScriptableObject receiver,
				string eventName, Action<T1, T2, T3> action)
				=> UnSubscribe(sender, receiver as object, eventName, action);
			static void UnSubscribe<T1, T2, T3>(object sender, object receiver,
				string eventName, Action<T1, T2, T3> action)
			{
				if (TryGetList(receiver, eventName, out List<MessageBase> list))
				{
					foreach (MessageBase message in list)
					{
						if (message.HasSameAction(action))
						{
							list.Remove(message);
							GenericPool<Message<T1, T2, T3>>.Release(message as Message<T1, T2, T3>);
							break;
						}
					}
					CheckRemoveList(receiver, eventName, list);
				}
			}



			public static void Execute(object sender, GameObject receiver, string eventName)
				=> Execute(sender, receiver as object, eventName);
			public static void Execute(object sender, ScriptableObject receiver, string eventName)
				=> Execute(sender, receiver as object, eventName);
			static void Execute(object sender, object receiver, string eventName)
			{
				if (TryGetList(receiver, eventName, out List<MessageBase> list))
				{
					foreach (Message message in list)
					{
						message.Invoke();
					}
				}
			}
			public static void Execute<T1>(object sender, GameObject receiver, string eventName, T1 arg1)
					=> Execute(sender, receiver as object, eventName, arg1);
			public static void Execute<T1>(object sender, ScriptableObject receiver, string eventName, T1 arg1)
				=> Execute(sender, receiver as object, eventName, arg1);
			static void Execute<T1>(object sender, object receiver, string eventName, T1 arg1)
			{
				if (TryGetList(receiver, eventName, out List<MessageBase> list))
				{
					foreach (Message<T1> message in list)
					{
						message.Invoke(arg1);
					}
				}
			}
			public static void Execute<T1, T2>(object sender, GameObject receiver, string eventName, T1 arg1, T2 arg2)
				=> Execute(sender, receiver as object, eventName, arg1, arg2);
			public static void Execute<T1, T2>(object sender, ScriptableObject receiver, string eventName, T1 arg1, T2 arg2)
				=> Execute(sender, receiver as object, eventName, arg1, arg2);
			static void Execute<T1, T2>(object sender, object receiver, string eventName, T1 arg1, T2 arg2)
			{
				if (TryGetList(receiver, eventName, out List<MessageBase> list))
				{
					foreach (Message<T1, T2> message in list)
					{
						message.Invoke(arg1, arg2);
					}
				}
			}
			public static void Execute<T1, T2, T3>(object sender, GameObject receiver, string eventName, T1 arg1, T2 arg2, T3 arg3)
				=> Execute(sender, receiver as object, eventName, arg1, arg2, arg3);
			public static void Execute<T1, T2, T3>(object sender, ScriptableObject receiver, string eventName, T1 arg1, T2 arg2, T3 arg3)
				=> Execute(sender, receiver as object, eventName, arg1, arg2, arg3);
			static void Execute<T1, T2, T3>(object sender, object receiver, string eventName, T1 arg1, T2 arg2, T3 arg3)
			{
				if (TryGetList(receiver, eventName, out List<MessageBase> list))
				{
					foreach (Message<T1, T2, T3> message in list)
					{
						message.Invoke(arg1, arg2, arg3);
					}
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

		public class Message<T1, T2, T3> : MessageBase
		{
			Action<T1, T2, T3> _action;
			public override Delegate Delegate => _action;

			public void Init(Action<T1, T2, T3> action)
			{
				_action = action;
			}
			public void Invoke(T1 arg1, T2 arg2, T3 arg3)
			{
				_action.Invoke(arg1, arg2, arg3);
			}
			public override bool HasSameAction(object action)
			{
				return _action.Equals(action);
			}
		}

	}
}
