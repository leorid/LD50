using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JL
{
	public class Shotgun : WeaponBase
	{
		public int burstAmount = 6;
		public float burstAngle = 45;

		public override void GetInputsInternal(WeaponInput fireInputs)
		{
			if (fireInputs.lmbDown)
			{
				Fire();
			}
		}

		protected override void FireInternal()
		{
			float offsetAngle = -burstAngle * 0.5f;
			float angleStep = burstAngle / burstAmount;

			for (int i = 0; i < burstAmount; i++)
			{
				float currentAngle = offsetAngle + angleStep * i;

				currentAngle *= Mathf.Deg2Rad;
				Vector2 vec = muzzlePos.right * Mathf.Sin(currentAngle) +
					muzzlePos.up* Mathf.Cos(currentAngle);

				SpawnProjectile(vec);
			}
		}
	}
}
