using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerOpenDoor : MonoBehaviour
{
	public Door door;

	private void OnTriggerEnter2D(Collider2D other)
	{
		door.SetOpen(true);
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		door.SetOpen(false);
	}
}
