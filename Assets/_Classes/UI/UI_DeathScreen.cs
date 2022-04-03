using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace JL
{
	public class UI_DeathScreen : MonoBehaviour
	{
		UIDocument uiDocument;

		VisualElement deathScreenRoot;
		Button respawnButton;

		void Awake()
		{
			uiDocument = GetComponentInParent<UIDocument>();
			deathScreenRoot = uiDocument.rootVisualElement.Q("DeathScreen");
			respawnButton = deathScreenRoot.Q<Button>("RespawnButton");

			respawnButton.clickable.clicked -= Respawn;
			respawnButton.clickable.clicked += Respawn;
		}

		public void Show()
		{
			deathScreenRoot.style.display = DisplayStyle.Flex;
		}

		public void Respawn()
		{
			Scene reloadScene = default;
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				Scene scene = SceneManager.GetSceneAt(i);
				if (scene.name == "PlayerScene") continue;
				reloadScene = scene;
				break;
			}

			SceneManager.UnloadSceneAsync(reloadScene);
			SceneManager.LoadScene(reloadScene.buildIndex, LoadSceneMode.Additive);
			Player.RespawnAction.Invoke();

			deathScreenRoot.style.display = DisplayStyle.None;
		}
	}
}
