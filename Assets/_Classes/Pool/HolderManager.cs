using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JL
{
	public static class HolderManager
	{
		static Transform _holderHead;
		public static Transform HolderHead
		{
			get
			{
				if (!_holderHead)
				{
					_holderHead = new GameObject("Holder Head").transform;
					Scene mainScene = SceneManager.GetSceneByName("Forest_Main");
					if (mainScene.isLoaded)
					{
						SceneManager.MoveGameObjectToScene(
							_holderHead.gameObject, mainScene);
					}
				}
				return _holderHead;
			}
		}

		static Dictionary<string, Transform> _stringDB =
			new Dictionary<string, Transform>();

		static Dictionary<int, Transform> _prefabDB =
			new Dictionary<int, Transform>();

		public static Transform Get(string name)
		{
			Transform result;
			if (!_stringDB.TryGetValue(name, out result) || !result)
			{
				result = new GameObject(name).transform;
				result.SetParent(HolderHead);
				_stringDB[name] = result;
			}
			return result;
		}
		public static Transform Get(GameObject prefab)
		{
			Transform result;
			int ID = prefab.GetInstanceID();
			if (!_prefabDB.TryGetValue(ID, out result) || !result)
			{
				result = new GameObject(prefab.name + " Holder").transform;
				result.SetParent(HolderHead);
				_prefabDB[ID] = result;
			}
			return result;
		}

		static Transform _particles;
		public static Transform Particles
		{
			get
			{
				if (!_particles)
				{
					_particles = new GameObject("Particles").transform;
					_particles.SetParent(HolderHead);
				}
				return _particles;
			}
		}
		static Transform _pickups;
		public static Transform Pickups
		{
			get
			{
				if (!_pickups)
				{
					_pickups = new GameObject("Pickups").transform;
					_pickups.SetParent(HolderHead);
				}
				return _pickups;
			}
		}
		static Transform _pooled;
		public static Transform Pooled
		{
			get
			{
				if (!_pooled)
				{
					_pooled = new GameObject("Pooled").transform;
					_pooled.SetParent(HolderHead);
				}
				return _pooled;
			}
		}
	}
}
