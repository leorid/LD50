using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JL
{
	public class ShootOpenDoor : MonoBehaviour
	{
		public Door door;
		public void Damage(DamageInfo damageInfo)
		{
			door.SetOpen(true);
		}
	}
}
