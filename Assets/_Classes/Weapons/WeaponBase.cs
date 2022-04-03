using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JL
{
	public abstract class WeaponBase : MonoBehaviour
	{
		public GameObject projectile;
		public int damage = 1;
		protected CinemachineImpulseSource impulseSource;
		public float shootImpulse = 0.05f;
		protected Rigidbody2D playerRB;
		public Transform muzzlePos;
		[HideInInspector]
		public WeaponController weaponController;

		public float fireRate = 0.1f;
		float lastFireTime = 0;

		public virtual void Init()
		{
			impulseSource = GetComponentInParent<CinemachineImpulseSource>();
			playerRB = GetComponentInParent<Rigidbody2D>();
			weaponController = GetComponentInParent<WeaponController>();
		}

		public void GetInputs(WeaponInput fireInputs)
		{
			GetInputsInternal(fireInputs);
		}

		public abstract void GetInputsInternal(WeaponInput fireInputs);

		public void Fire()
		{
			//impulseSource.GenerateImpulseAt(transform.position,
			//-transform.up * shootImpulse);
			if (Time.time - lastFireTime >= fireRate)
			{
				lastFireTime = Time.time;

				playerRB.AddForce(-transform.up * shootImpulse, ForceMode2D.Impulse);
				FireInternal();
			}
		}
		protected abstract void FireInternal();

		protected void SpawnProjectile(Vector2 direction)
		{
			GameObject instance = ObjectPool.Get(projectile,
				muzzlePos.position,
				muzzlePos.rotation,
				HolderManager.Get(projectile));
			instance.transform.up = direction;
			ProjectileBase projectileClass = instance.GetComponent<ProjectileBase>();
			projectileClass.sender = this;
			projectileClass.mask = weaponController.mask;
			projectileClass.damage = damage;
		}
	}

	public struct WeaponInput
	{
		public bool lmbDown;
		public bool lmbUp;
		public bool lmbHeld;
	}
}
