using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour
{
	Vector2 lastPosition;
	public ContactFilter2D contactFilter2D;
	RaycastHit[] hits = new RaycastHit[64];

	public GameObject impactEffect;

	public void ResetValues()
	{
		lastPosition = transform.position;
	}

	protected bool ContinuousRaycastCheck(out RaycastHit2D hit)
	{
		//if()

		Vector2 vec = (Vector2)transform.position - lastPosition;
		hit = Physics2D.Raycast(lastPosition, 
							vec, 
							vec.magnitude);

		lastPosition = transform.position;

		return hit.collider;
	}
}
