using System;
using System.Reflection;
using UnityEngine;

namespace JL
{
	/// <summary>
	/// Variable List
	/// </summary>
	public static partial class VL
	{

#if UNITY_EDITOR
		[UnityEditor.InitializeOnLoadMethod]
#endif
		[RuntimeInitializeOnLoadMethod]
		static void Construct()
		{
			Type mlType = typeof(VL);
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
					if (hubEntryObj == null)
					{
						hubEntryObj = Activator.CreateInstance(hubEntryType);
						fieldInfo.SetValue(null, hubEntryObj);
					}
					idField.SetValue(hubEntryObj, idString);
				}
			}
		}

		public static class Enemy
		{
			public static readonly VariableEntryGlobal<float> detection = new();
			public static readonly VariableEntryGlobal<Vector3> position = new();
		}
		public static class UI
		{
			public static readonly HubEntryGlobal<float> something = new();
		}
	}

	public class VariableEntryGlobal<T1>
	{
		string id;
		public string ID => id;

		public void Set(object sender, string key, T1 arg1)
		{
			VariableHub.Global.Set<T1>(key, arg1);
		}
		public void SetFunc(object sender, string key, Func<T1> arg1)
		{
			VariableHub.Global.SetFunc<T1>(key,arg1);
		}
		public void UnSet(object sender, string key)
		{
			VariableHub.Global.UnSet<T1>(key);
		}
		public bool TryRead(object sender, string key, out T1 value)
		{
			return VariableHub.Global.TryRead<T1>(key, out value);
		}
	}
}
