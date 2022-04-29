using System;
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

			public static void Set<T1>(string key, T1 arg1)
			{
				GetOrCreate(key, out VariableHolder<T1> holder);
				holder.SetValues(arg1);
			}

			public static void SetFunc<T1>(string key, Func<T1> func1)
			{
				GetOrCreate(key, out VariableHolderFunc<T1> holder);
				holder.SetValues(func1);
			}

			public static void UnSet<T1>(string key)
			{
				if (_globalVars.TryGetValue(key, out VariableHolder holder))
				{
					holder.Reset();
					_globalVars.Remove(key);
					GenericPool<VariableHolder<T1>>.Release(holder as VariableHolder<T1>);
				}
			}

			public static bool TryRead<T1>(string key, out T1 value)
			{
				value = default;
				if (_globalVars.TryGetValue(key, out VariableHolder temp))
				{
					VariableHolder<T1> holder = temp as VariableHolder<T1>;
					value = holder.Value;
					return true;
				}
				return false;
			}
		}
		public static class Local
		{
			static Dictionary<string, Dictionary<object, VariableHolder>> _localVars = new();

			static HashSet<object> _setA = new();
			static HashSet<object> _setB = new();
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

			public static void SetFunc<T1>(GameObject obj, string key, Func<T1> func1)
				=> SetFunc(obj as object, key, func1);
			public static void SetFunc<T1>(ScriptableObject obj, string key, Func<T1> func1)
				=> SetFunc(obj as object, key, func1);
			static void SetFunc<T1>(object obj, string key, Func<T1> func1)
			{
				GetOrCreate(obj, key, out VariableHolderFunc<T1> holder);
				holder.SetValues(func1);
			}

			public static void UnSet<T1>(GameObject obj, string key)
				=> UnSet<T1>(obj as object, key);
			public static void UnSet<T1>(ScriptableObject obj, string key)
				=> UnSet<T1>(obj as object, key);
			static void UnSet<T1>(object obj, string key)
			{
				if (_localVars.TryGetValue(key, out var dict))
				{
					if (dict.TryGetValue(obj, out VariableHolder holder))
					{
						holder.Reset();
						dict.Remove(key);
						GenericPool<VariableHolder<T1>>.Release(holder as VariableHolder<T1>);
					}

					if (dict.Count == 0)
					{
						GenericPool<Dictionary<object, VariableHolder>>.Release(dict);
						_localVars.Remove(key);
					}
				}
			}

			public static bool TryRead<T1>(GameObject obj, string key,
				out T1 value)
				=> TryRead(obj as object, key, out value);
			public static bool TryRead<T1>(ScriptableObject obj, string key,
				out T1 value)
				=> TryRead(obj as object, key, out value);
			static bool TryRead<T1>(object obj, string key, out T1 value)
			{
				bool result = TryGet(obj, key, out VariableHolder<T1> holder);
				value = holder != null ? holder.Value : default;
				return result;
			}

			static void FillHashSet(string key, HashSet<object> set)
			{
				set.Clear();
				Dictionary<object, VariableHolder> dict;
				if (_localVars.TryGetValue(key, out dict))
				{
					foreach (KeyValuePair<object, VariableHolder> pair in dict)
					{
						set.Add(pair.Key);
					}
				}
			}

			public static void Fetch<T1>(string key, List<(object, T1)> list)
			{
				Dictionary<object, VariableHolder> dict;
				if (_localVars.TryGetValue(key, out dict))
				{
					foreach (KeyValuePair<object, VariableHolder> pair in dict)
					{
						list.Add((pair.Key, (pair.Value as VariableHolder<T1>).Value));
					}
				}
			}
			public static void Fetch<T1, T2>(string key1, string key2, List<(object, T1, T2)> list)
			{
				FillHashSet(key1, _setA);
				FillHashSet(key2, _setB);
				_setA.IntersectWith(_setB);

				foreach (object obj in _setA)
				{
					TryRead(obj, key1, out T1 val1);
					TryRead(obj, key2, out T2 val2);
					list.Add((obj, val1, val2));
				}
			}
			public static void Fetch<T1, T2, T3>(
				string key1, string key2, string key3, List<(object, T1, T2, T3)> list)
			{
				FillHashSet(key1, _setA);
				FillHashSet(key2, _setB);
				_setA.IntersectWith(_setB);
				FillHashSet(key3, _setB);
				_setA.IntersectWith(_setB);

				foreach (object obj in _setA)
				{
					TryRead(obj, key1, out T1 val1);
					TryRead(obj, key2, out T2 val2);
					TryRead(obj, key3, out T3 val3);
					list.Add((obj, val1, val2, val3));
				}
			}
			public static void Fetch<T1, T2, T3, T4>(
				string key1, string key2, string key3, string key4,
				List<(object, T1, T2, T3, T4)> list)
			{
				FillHashSet(key1, _setA);
				FillHashSet(key2, _setB);
				_setA.IntersectWith(_setB);
				FillHashSet(key3, _setB);
				_setA.IntersectWith(_setB);
				FillHashSet(key4, _setB);
				_setA.IntersectWith(_setB);

				foreach (object obj in _setA)
				{
					TryRead(obj, key1, out T1 val1);
					TryRead(obj, key2, out T2 val2);
					TryRead(obj, key3, out T3 val3);
					TryRead(obj, key4, out T4 val4);
					list.Add((obj, val1, val2, val3, val4));
				}
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
	T1 _value;
	public void SetValues(T1 arg1)
	{
		_value = arg1;
	}
	public override void Reset()
	{
		_value = default;
	}
	public virtual T1 Value => _value;
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
	public override T1 Value => func1.Invoke();
}
