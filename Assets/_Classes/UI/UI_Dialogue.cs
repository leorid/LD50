using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace JL
{
	public class UI_Dialogue : MonoBehaviour
	{
		static DialogueEntry currentDialogue;

		public static void GetDialogue(DialogueEntry dialogueEntry)
		{
			if(dialogueEntry == null)
			{
				dialogueBox.style.display = DisplayStyle.None;
				return;
			}
			dialogueBox.style.display = DisplayStyle.Flex;

			dialogueLabel.text = dialogueEntry.text;

			currentDialogue = dialogueEntry;
		}

		static VisualElement dialogueBox;
		static Label dialogueLabel;

		UIDocument uiDocument;

		static Camera cam;

		void Awake()
		{
			uiDocument = GetComponentInParent<UIDocument>();
			VisualElement dialogueRoot = uiDocument.rootVisualElement.Q("DialogueRoot");
			dialogueBox = dialogueRoot.Q("DialogueBox");
			dialogueLabel = dialogueRoot.Q<Label>("DialogueLabel");
		}

		private void Start()
		{
			GetDialogue(null);
		}

		private void Update()
		{
			if(currentDialogue != null)
			{
				if (!cam) cam = Camera.main;
				Vector3 pos = cam.WorldToScreenPoint(currentDialogue.target.position);
				pos.z = 0;
				pos.y = Screen.height - pos.y;
				dialogueBox.transform.position = pos;
			}
		}
	}
}
