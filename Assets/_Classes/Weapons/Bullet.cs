using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JL
{
	public class Bullet : ProjectileBase
	{
		public int damage = 1;
		public float speed = 50;
		public TrailRenderer trailRenderer;
		public float screenshake = 0.05f;

		public void Update()
		{
			transform.Translate(
				 transform.up * speed * Time.deltaTime,
				 Space.World);

			if (ContinuousRaycastCheck(out RaycastHit2D hit))
			{
				hit.collider.gameObject.SendMessage("Damage",
					damage, SendMessageOptions.DontRequireReceiver);
				SpawnImactEffect(transform.position, transform.up, hit.collider);
				impulseSource.GenerateImpulseAt(transform.position, 
					 transform.up * screenshake);
				ReturnToPool();
			}


		}

		protected override void OnAfterGetFromPool()
		{
			base.OnAfterGetFromPool();
			trailRenderer.Clear();
		}
	}
}