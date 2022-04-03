using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JL
{
	public class SwitchToLevel : MonoBehaviour
	{
		public string levelName;

		public void Execute()
		{
			SceneManager.UnloadSceneAsync(gameObject.scene.buildIndex);
			SceneManager.LoadScene(levelName, LoadSceneMode.Additive);

			// execute respawn
			Player.RespawnAction?.Invoke();
		}
	}
}
