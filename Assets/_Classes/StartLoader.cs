using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JL
{
	public class StartLoader : MonoBehaviour
	{
		private void Start()
		{
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				if (SceneManager.GetSceneAt(i).buildIndex == 1) return;
			}

			SceneManager.LoadScene(1, LoadSceneMode.Additive);
		}
	}
}
