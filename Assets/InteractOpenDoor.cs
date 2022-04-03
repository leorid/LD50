using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractOpenDoor : MonoBehaviour
{
	public Door door;
    public void Interact()
	{
		door.SetOpen(true);
	}
}
