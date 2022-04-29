using System;
using System.Reflection;
using UnityEngine;

namespace JL
{
	/// <summary>
	/// Message List
	/// </summary>
	public static partial class ML
	{

#if UNITY_EDITOR
		[UnityEditor.InitializeOnLoadMethod]
#endif
		[RuntimeInitializeOnLoadMethod]
		static void Construct()
		{
			Type mlType = typeof(ML);
			Type[] nestedTypes = mlType.GetNestedTypes();
			foreach (Type nestedType in nestedTypes)
			{
				foreach (FieldInfo fieldInfo in nestedType.GetFields())
				{
					string idString = $"{mlType.Name}.{nestedType.Name}.{fieldInfo.Name}";

					Type hubEntryType = fieldInfo.FieldType;
					FieldInfo idField = hubEntryType.GetField("id",
						 BindingFlags.NonPublic | BindingFlags.Instance);

					object hubEntryObj = fieldInfo.GetValue(null);
					idField.SetValue(hubEntryObj, idString);
				}
			}
		}

		public static class Enemy
		{
			public static readonly HubEntryGlobal<float> detection = new();
		}
		public static class UI
		{
			public static readonly HubEntryGlobal<float> something = new();
		}
	}

	#region Global
	public class HubEntryGlobal
	{
		string id;
		public string ID => id;

		public void Subscribe(object sender, Action action)
		{
			EventHub.Global.Subscribe(sender, id, action);
		}
		public void Unsubscribe(object sender, Action action)
		{
			EventHub.Global.UnSubscribe(sender, id, action);
		}
		public void Execute(object sender)
		{
			EventHub.Global.Execute(sender, id);
		}
	}
	public class HubEntryGlobal<T1>
	{
		string id;
		public string ID => id;

		public void Subscribe(object sender, Action<T1> action)
		{
			EventHub.Global.Subscribe(sender, id, action);
		}
		public void Unsubscribe(object sender, Action<T1> action)
		{
			EventHub.Global.UnSubscribe(sender, id, action);
		}
		public void Execute(object sender, T1 arg1)
		{
			EventHub.Global.Execute(sender, id, arg1);
		}
	}
	public class HubEntryGlobal<T1, T2>
	{
		string id;
		public string ID => id;

		public void Subscribe(object sender, Action<T1, T2> action)
		{
			EventHub.Global.Subscribe(sender, id, action);
		}
		public void Unsubscribe(object sender, Action<T1, T2> action)
		{
			EventHub.Global.UnSubscribe(sender, id, action);
		}
		public void Execute(object sender, T1 arg1, T2 arg2)
		{
			EventHub.Global.Execute(sender, id, arg1, arg2);
		}
	}
	public class HubEntryGlobal<T1, T2, T3>
	{
		string id;
		public string ID => id;

		public void Subscribe(object sender, Action<T1, T2, T3> action)
		{
			EventHub.Global.Subscribe(sender, id, action);
		}
		public void Unsubscribe(object sender, Action<T1, T2, T3> action)
		{
			EventHub.Global.UnSubscribe(sender, id, action);
		}
		public void Execute(object sender, T1 arg1, T2 arg2, T3 arg3)
		{
			EventHub.Global.Execute(sender, id, arg1, arg2, arg3);
		}
	}
	#endregion

	#region Local
	public class HubEntryLocal
	{
		string id;
		public string ID => id;

		public void Subscribe(object sender, GameObject receiver, Action action)
		{
			EventHub.Local.Subscribe(sender, receiver, id, action);
		}
		public void Subscribe(object sender, ScriptableObject receiver, Action action)
		{
			EventHub.Local.Subscribe(sender, receiver, id, action);
		}
		public void Unsubscribe(object sender, GameObject receiver, Action action)
		{
			EventHub.Local.UnSubscribe(sender, receiver, id, action);
		}
		public void Unsubscribe(object sender, ScriptableObject receiver, Action action)
		{
			EventHub.Local.UnSubscribe(sender, receiver, id, action);
		}
		public void Execute(object sender, GameObject receiver)
		{
			EventHub.Local.Execute(sender, receiver, id);
		}
		public void Execute(object sender, ScriptableObject receiver)
		{
			EventHub.Local.Execute(sender, receiver, id);
		}
	}
	public class HubEntryLocal<T1>
	{
		string id;
		public string ID => id;

		public void Subscribe(object sender, GameObject receiver, Action<T1> action)
		{
			EventHub.Local.Subscribe(sender, receiver, id, action);
		}
		public void Subscribe(object sender, ScriptableObject receiver, Action<T1> action)
		{
			EventHub.Local.Subscribe(sender, receiver, id, action);
		}
		public void Unsubscribe(object sender, GameObject receiver, Action<T1> action)
		{
			EventHub.Local.UnSubscribe(sender, receiver, id, action);
		}
		public void Unsubscribe(object sender, ScriptableObject receiver, Action<T1> action)
		{
			EventHub.Local.UnSubscribe(sender, receiver, id, action);
		}
		public void Execute(object sender, GameObject receiver, T1 arg1)
		{
			EventHub.Local.Execute(sender, receiver, id, arg1);
		}
		public void Execute(object sender, ScriptableObject receiver, T1 arg1)
		{
			EventHub.Local.Execute(sender, receiver, id, arg1);
		}
	}
	public class HubEntryLocal<T1, T2>
	{
		string id;
		public string ID => id;

		public void Subscribe(object sender, GameObject receiver, Action<T1, T2> action)
		{
			EventHub.Local.Subscribe(sender, receiver, id, action);
		}
		public void Subscribe(object sender, ScriptableObject receiver, Action<T1, T2> action)
		{
			EventHub.Local.Subscribe(sender, receiver, id, action);
		}
		public void Unsubscribe(object sender, GameObject receiver, Action<T1, T2> action)
		{
			EventHub.Local.UnSubscribe(sender, receiver, id, action);
		}
		public void Unsubscribe(object sender, ScriptableObject receiver, Action<T1, T2> action)
		{
			EventHub.Local.UnSubscribe(sender, receiver, id, action);
		}
		public void Execute(object sender, GameObject receiver, T1 arg1, T2 arg2)
		{
			EventHub.Local.Execute(sender, receiver, id, arg1, arg2);
		}
		public void Execute(object sender, ScriptableObject receiver, T1 arg1, T2 arg2)
		{
			EventHub.Local.Execute(sender, receiver, id, arg1, arg2);
		}
	}
	public class HubEntryLocal<T1, T2, T3>
	{
		string id;
		public string ID => id;

		public void Subscribe(object sender, GameObject receiver, Action<T1, T2, T3> action)
		{
			EventHub.Local.Subscribe(sender, receiver, id, action);
		}
		public void Subscribe(object sender, ScriptableObject receiver, Action<T1, T2, T3> action)
		{
			EventHub.Local.Subscribe(sender, receiver, id, action);
		}
		public void Unsubscribe(object sender, GameObject receiver, Action<T1, T2, T3> action)
		{
			EventHub.Local.UnSubscribe(sender, receiver, id, action);
		}
		public void Unsubscribe(object sender, ScriptableObject receiver, Action<T1, T2, T3> action)
		{
			EventHub.Local.UnSubscribe(sender, receiver, id, action);
		}
		public void Execute(object sender, GameObject receiver, T1 arg1, T2 arg2, T3 arg3)
		{
			EventHub.Local.Execute(sender, receiver, id, arg1, arg2, arg3);
		}
		public void Execute(object sender, ScriptableObject receiver, T1 arg1, T2 arg2, T3 arg3)
		{
			EventHub.Local.Execute(sender, receiver, id, arg1, arg2, arg3);
		}
	}
	#endregion
}
