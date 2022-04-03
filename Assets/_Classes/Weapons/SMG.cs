using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JL
{
	public class SMG : WeaponBase
	{

		public override void GetInputsInternal(WeaponInput fireInputs)
		{
			if (fireInputs.lmbHeld)
			{
				Fire();
			}
		}

		protected override void FireInternal()
		{
			SpawnProjectile(muzzlePos.up);
		}
	}
}
