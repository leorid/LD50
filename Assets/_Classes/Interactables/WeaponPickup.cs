using System.Collections;
using UnityEngine;

namespace JL
{
	public class WeaponPickup : MonoBehaviour
	{
		public int weaponIndex;

		public void Pickup()
		{
			gameObject.SetActive(false);
		}
	}
}