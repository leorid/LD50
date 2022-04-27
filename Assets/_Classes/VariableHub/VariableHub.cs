using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace JL
{
	public static class VariableHub
	{
		public static class Global
		{
			static Dictionary<string, VariableHolder> _globalVars = new();

			static void GetOrCreate<T>(string key, out T holder)
				where T : VariableHolder, new()
			{
				if (_globalVars.TryGetValue(key, out VariableHolder temp))
				{
					holder = temp as T;
					return;
				}
				else
				{
					holder = GenericPool<T>.Get();
					_globalVars.Add(key, holder);
					return;
				}
			}
			static void PoolAndRemove<T>(string key) where T : VariableHolder, new()
			{
				if (_globalVars.TryGetValue(key, out VariableHolder holder))
				{
					holder.Reset();
					_globalVars.Remove(key);
					GenericPool<T>.Release(holder as T);
				}
			}
			static bool TryGet<T>(string key, out T holder) where T : VariableHolder
			{
				holder = null;
				if (_globalVars.TryGetValue(key, out VariableHolder temp))
				{
					holder = temp as T;
					return true;
				}
				return false;
			}

			public static void Set<T1>(string key, T1 arg1)
			{
				GetOrCreate(key, out VariableHolder<T1> holder);
				holder.SetValues(arg1);
			}
			public static void Set<T1, T2>(string key, T1 arg1, T2 arg2)
			{
				GetOrCreate(key, out VariableHolder<T1, T2> holder);
				holder.SetValues(arg1, arg2);
			}
			public static void Set<T1, T2, T3>(string key, T1 arg1, T2 arg2, T3 arg3)
			{
				GetOrCreate(key, out VariableHolder<T1, T2, T3> holder);
				holder.SetValues(arg1, arg2, arg3);
			}

			public static void SetFunc<T1>(string key, Func<T1> func1)
			{
				GetOrCreate(key, out VariableHolderFunc<T1> holder);
				holder.SetValues(func1);
			}
			public static void SetFunc<T1, T2>(string key,
				Func<T1> func1, Func<T2> func2)
			{
				GetOrCreate(key, out VariableHolderFunc<T1, T2> holder);
				holder.SetValues(func1, func2);
			}
			public static void SetFunc<T1, T2, T3>(string key,
				Func<T1> func1, Func<T2> func2, Func<T3> func3)
			{
				GetOrCreate(key, out VariableHolderFunc<T1, T2, T3> holder);
				holder.SetValues(func1, func2, func3);
			}

			public static void UnSet<T1>(string key)
			{
				PoolAndRemove<VariableHolder<T1>>(key);
			}
			public static void UnSet<T1, T2>(string key)
			{
				PoolAndRemove<VariableHolder<T1, T2>>(key);
			}
			public static void UnSet<T1, T2, T3>(string key)
			{
				PoolAndRemove<VariableHolder<T1, T2, T3>>(key);
			}

			public static bool TryRead<T1>(string key, out ValueTuple<T1> value)
			{
				bool result = TryGet(key, out VariableHolder<T1> holder);
				value = holder != null ? holder.Value : default;
				return result;
			}
			public static bool TryRead<T1, T2>(string key, out ValueTuple<T1, T2> value)
			{
				bool result = TryGet(key, out VariableHolder<T1, T2> holder);
				value = holder != null ? holder.Value : default;
				return result;
			}
			public static bool TryRead<T1, T2, T3>(string key, out ValueTuple<T1, T2, T3> value)
			{
				bool result = TryGet(key, out VariableHolder<T1, T2, T3> holder);
				value = holder != null ? holder.Value : default;
				return result;
			}
		}
		public static class Local
		{
			static Dictionary<string, Dictionary<object, VariableHolder>> _localVars = new();
			// TryRead(GameObject, string)
			// TryReadAll(string)

			static Dictionary<object, VariableHolder> GetOrCreateDict(string key)
			{
				Dictionary<object, VariableHolder> dict;
				if (_localVars.TryGetValue(key, out dict))
				{
					return dict;
				}
				else
				{
					dict = GenericPool<Dictionary<object, VariableHolder>>.Get();
					_localVars.Add(key, dict);
					return dict;
				}
			}
			static void GetOrCreate<T>(object obj, string key, out T holder)
				where T : VariableHolder, new()
			{
				Dictionary<object, VariableHolder> dict = GetOrCreateDict(key);
				if (dict.TryGetValue(obj, out VariableHolder temp))
				{
					holder = temp as T;
					return;
				}
				else
				{
					holder = GenericPool<T>.Get();
					dict.Add(obj, holder);
					return;
				}
			}

			static void PoolAndRemove<T>(object obj, string key) where T : VariableHolder, new()
			{
				if (_localVars.TryGetValue(key, out var dict))
				{
					if (dict.TryGetValue(key, out VariableHolder holder))
					{
						holder.Reset();
						dict.Remove(key);
						GenericPool<T>.Release(holder as T);
					}

					if (dict.Count == 0)
					{
						GenericPool<Dictionary<object, VariableHolder>>.Release(dict);
						_localVars.Remove(key);
					}
				}
			}
			static bool TryGet<T>(object obj, string key, out T holder) where T : VariableHolder
			{
				holder = null;
				Dictionary<object, VariableHolder> dict;
				if (_localVars.TryGetValue(key, out dict))
				{
					if (dict.TryGetValue(obj, out VariableHolder temp))
					{
						holder = temp as T;
						return true;
					}
				}
				return false;
			}


			public static void Set<T1>(GameObject obj, string key, T1 arg1)
				=> Set(obj as object, key, arg1);
			public static void Set<T1>(ScriptableObject obj, string key, T1 arg1)
				=> Set(obj as object, key, arg1);
			static void Set<T1>(object obj, string key, T1 arg1)
			{
				GetOrCreate(obj, key, out VariableHolder<T1> holder);
				holder.SetValues(arg1);
			}
			public static void Set<T1, T2>(GameObject obj, string key, T1 arg1, T2 arg2)
				=> Set(obj as object, key, arg1, arg2);
			public static void Set<T1, T2>(ScriptableObject obj, string key, T1 arg1, T2 arg2)
				=> Set(obj as object, key, arg1, arg2);
			static void Set<T1, T2>(object obj, string key, T1 arg1, T2 arg2)
			{
				GetOrCreate(obj, key, out VariableHolder<T1, T2> holder);
				holder.SetValues(arg1, arg2);
			}
			public static void Set<T1, T2, T3>(GameObject obj, string key, T1 arg1, T2 arg2, T3 arg3)
				=> Set(obj as object, key, arg1, arg2, arg3);
			public static void Set<T1, T2, T3>(ScriptableObject obj, string key, T1 arg1, T2 arg2, T3 arg3)
				=> Set(obj as object, key, arg1, arg2, arg3);
			static void Set<T1, T2, T3>(object obj, string key, T1 arg1, T2 arg2, T3 arg3)
			{
				GetOrCreate(obj, key, out VariableHolder<T1, T2, T3> holder);
				holder.SetValues(arg1, arg2, arg3);
			}

			public static void SetFunc<T1>(GameObject obj, string key, Func<T1> func1)
				=> SetFunc(obj as object, key, func1);
			public static void SetFunc<T1>(ScriptableObject obj, string key, Func<T1> func1)
				=> SetFunc(obj as object, key, func1);
			static void SetFunc<T1>(object obj, string key, Func<T1> func1)
			{
				GetOrCreate(obj, key, out VariableHolderFunc<T1> holder);
				holder.SetValues(func1);
			}
			public static void SetFunc<T1, T2>(GameObject obj, string key,
				Func<T1> func1, Func<T2> func2)
				=> SetFunc(obj as object, key, func1, func2);
			public static void SetFunc<T1, T2>(ScriptableObject obj, string key,
				Func<T1> func1, Func<T2> func2)
				=> SetFunc(obj as object, key, func1, func2);
			static void SetFunc<T1, T2>(object obj, string key,
				Func<T1> func1, Func<T2> func2)
			{
				GetOrCreate(obj, key, out VariableHolderFunc<T1, T2> holder);
				holder.SetValues(func1, func2);
			}
			public static void SetFunc<T1, T2, T3>(GameObject obj, string key,
				Func<T1> func1, Func<T2> func2, Func<T3> func3)
				=> SetFunc(obj as object, key, func1, func2, func3);
			public static void SetFunc<T1, T2, T3>(ScriptableObject obj, string key,
				Func<T1> func1, Func<T2> func2, Func<T3> func3)
				=> SetFunc(obj as object, key, func1, func2, func3);
			static void SetFunc<T1, T2, T3>(object obj, string key,
				Func<T1> func1, Func<T2> func2, Func<T3> func3)
			{
				GetOrCreate(obj, key, out VariableHolderFunc<T1, T2, T3> holder);
				holder.SetValues(func1, func2, func3);
			}


			public static void UnSet<T1>(GameObject obj, string key)
				=> UnSet<T1>(obj as object, key);
			public static void UnSet<T1>(ScriptableObject obj, string key)
				=> UnSet<T1>(obj as object, key);
			static void UnSet<T1>(object obj, string key)
			{
				PoolAndRemove<VariableHolder<T1>>(obj, key);
			}
			public static void UnSet<T1, T2>(GameObject obj, string key)
				=> UnSet<T1, T2>(obj as object, key);
			public static void UnSet<T1, T2>(ScriptableObject obj, string key)
				=> UnSet<T1, T2>(obj as object, key);
			static void UnSet<T1, T2>(object obj, string key)
			{
				PoolAndRemove<VariableHolder<T1, T2>>(obj, key);
			}
			public static void UnSet<T1, T2, T3>(GameObject obj, string key)
				=> UnSet<T1, T2, T3>(obj as object, key);
			public static void UnSet<T1, T2, T3>(ScriptableObject obj, string key)
				=> UnSet<T1, T2, T3>(obj as object, key);
			static void UnSet<T1, T2, T3>(object obj, string key)
			{
				PoolAndRemove<VariableHolder<T1, T2, T3>>(obj, key);
			}

			public static bool TryRead<T1>(GameObject obj, string key,
				out ValueTuple<T1> value)
				=> TryRead(obj as object, key, out value);
			public static bool TryRead<T1>(ScriptableObject obj, string key,
				out ValueTuple<T1> value)
				=> TryRead(obj as object, key, out value);
			static bool TryRead<T1>(object obj, string key, out ValueTuple<T1> value)
			{
				bool result = TryGet(obj, key, out VariableHolder<T1> holder);
				value = holder != null ? holder.Value : default;
				return result;
			}
			public static bool TryRead<T1, T2>(GameObject obj, string key,
				out ValueTuple<T1, T2> value)
				=> TryRead(obj as object, key, out value);
			public static bool TryRead<T1, T2>(ScriptableObject obj, string key,
				out ValueTuple<T1, T2> value)
				=> TryRead(obj as object, key, out value);
			static bool TryRead<T1, T2>(object obj, string key, out ValueTuple<T1, T2> value)
			{
				bool result = TryGet(obj, key, out VariableHolder<T1, T2> holder);
				value = holder != null ? holder.Value : default;
				return result;
			}
			public static bool TryRead<T1, T2, T3>(GameObject obj, string key,
				out ValueTuple<T1, T2, T3> value)
				=> TryRead(obj as object, key, out value);
			public static bool TryRead<T1, T2, T3>(ScriptableObject obj, string key,
				out ValueTuple<T1, T2, T3> value)
				=> TryRead(obj as object, key, out value);
			static bool TryRead<T1, T2, T3>(object obj, string key, out ValueTuple<T1, T2, T3> value)
			{
				bool result = TryGet(obj, key, out VariableHolder<T1, T2, T3> holder);
				value = holder != null ? holder.Value : default;
				return result;
			}
		}
	}
}

abstract class VariableHolder
{
	public abstract void Reset();
}

class VariableHolder<T1> : VariableHolder
{
	ValueTuple<T1> _value;
	public void SetValues(T1 arg1)
	{
		_value.Item1 = arg1;
	}
	public override void Reset()
	{
		_value.Item1 = default;
	}
	public virtual ValueTuple<T1> Value => _value;
}
class VariableHolderFunc<T1> : VariableHolder<T1>
{
	Func<T1> func1;
	public void SetValues(Func<T1> func1)
	{
		this.func1 = func1;
	}
	public override void Reset()
	{
		base.Reset();
		func1 = null;
	}
	public override ValueTuple<T1> Value => new ValueTuple<T1>(func1.Invoke());
}

class VariableHolder<T1, T2> : VariableHolder
{
	ValueTuple<T1, T2> _value;
	public void SetValues(T1 arg1, T2 arg2)
	{
		_value.Item1 = arg1;
		_value.Item2 = arg2;
	}
	public override void Reset()
	{
		_value.Item1 = default;
		_value.Item2 = default;
	}
	public virtual ValueTuple<T1, T2> Value => _value;
}
class VariableHolderFunc<T1, T2> : VariableHolder<T1, T2>
{
	Func<T1> func1;
	Func<T2> func2;
	public void SetValues(Func<T1> func1, Func<T2> func2)
	{
		this.func1 = func1;
		this.func2 = func2;
	}
	public override void Reset()
	{
		base.Reset();
		func1 = null;
		func2 = null;
	}
	public override ValueTuple<T1, T2> Value => (func1.Invoke(), func2.Invoke());
}

class VariableHolder<T1, T2, T3> : VariableHolder
{
	ValueTuple<T1, T2, T3> _value;
	public void SetValues(T1 arg1, T2 arg2, T3 arg3)
	{
		_value.Item1 = arg1;
		_value.Item2 = arg2;
		_value.Item3 = arg3;
	}
	public override void Reset()
	{
		_value.Item1 = default;
		_value.Item2 = default;
		_value.Item3 = default;
	}
	public virtual ValueTuple<T1, T2, T3> Value => _value;
}
class VariableHolderFunc<T1, T2, T3> : VariableHolder<T1, T2, T3>
{
	Func<T1> func1;
	Func<T2> func2;
	Func<T3> func3;
	public void SetValues(Func<T1> func1, Func<T2> func2, Func<T3> func3)
	{
		this.func1 = func1;
		this.func2 = func2;
		this.func3 = func3;
	}
	public override void Reset()
	{
		base.Reset();
		func1 = null;
		func2 = null;
		func3 = null;
	}
	public override ValueTuple<T1, T2, T3> Value =>
		(func1.Invoke(), func2.Invoke(), func3.Invoke());
}
