using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
	public GameObject Projectile;

	public void Fire() => FireInternal();
	protected abstract void FireInternal();
}
