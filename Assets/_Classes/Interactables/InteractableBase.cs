using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace JL
{
	public class InteractableBase : MonoBehaviour
	{
		public UnityEvent onInteract;

		public void OnInteract()
		{
			onInteract?.Invoke();
		}
	}
}