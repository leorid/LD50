using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JL
{
	public class GenericDialogue : MonoBehaviour
	{
		public int index = -1;
		public List<DialogueEntry> entries = new List<DialogueEntry>();

		public Transform dialoguePos;

		public void Next()
		{
			index++;
			if (index >= entries.Count)
			{
			UI_Dialogue.GetDialogue(null);
				return;
			}
			DialogueEntry current = entries[index];

			current.target = dialoguePos;

			current.unityEvent?.Invoke();

			UI_Dialogue.GetDialogue(current);
		}
	}
}

namespace JL
{

	[System.Serializable]
	public class DialogueEntry
	{
		[TextArea(5, 10)]
		public string text;
		public float maxDuration;
		public Transform target;
		public UnityEvent unityEvent;
	}
}
