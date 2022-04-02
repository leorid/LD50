using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JL
{
	public class WeaponController : MonoBehaviour
	{
		public LayerMask mask;
		public WeaponBase currentWeapon;

		public void GetInput(WeaponInput weaponInput)
		{
			if (currentWeapon) currentWeapon.GetInputs(weaponInput);
		}
	}
}