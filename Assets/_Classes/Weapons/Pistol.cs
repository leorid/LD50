using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JL
{
	public class Pistol : WeaponBase
	{

		public override void GetInputsInternal(WeaponInput fireInputs)
		{
			if (fireInputs.lmbDown)
			{
				Fire();
			}
		}

		protected override void FireInternal()
		{
			SpawnProjectile(transform.up);
		}
	}
}