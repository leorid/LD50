using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JL
{
	public abstract class ProjectileBase : MonoBehaviour
	{
		Vector2 lastPosition;

		public object sender;
		public float maxLifetime = 20;
		public int damage = 1;
		public GameObject impactEffectWall;
		public GameObject impactEffectBlood;
		public CinemachineImpulseSource impulseSource;
		public LayerMask mask;
		public AudioClip hitEnemySound;
		public AudioClip hitPlayerSound;
		public AudioClip hitWallSound;

		float startTime;

		protected virtual void Awake()
		{
			impulseSource = GetComponent<CinemachineImpulseSource>();
		}

		protected virtual void OnBeforeAddToPool()
		{

		}
		protected virtual void OnAfterGetFromPool()
		{
			ResetValues();
		}

		public void ResetValues()
		{
			lastPosition = transform.position;
		}

		protected bool CheckLifetime() => Time.time - startTime < maxLifetime;

		protected bool ContinuousRaycastCheck(out RaycastHit2D hit)
		{
			Vector2 vec = (Vector2)transform.position - lastPosition;
			hit = Physics2D.Raycast(lastPosition,
								vec,
								vec.magnitude,
								mask);

			lastPosition = transform.position;

			return hit.collider;
		}

		protected void SpawnImactEffect(Vector2 pos, Vector2 fwd, Collider2D col)
		{
			GameObject effect;
			if (col.CompareTag("Enemy") && hitEnemySound)
			{
				AudioSource.PlayClipAtPoint(hitEnemySound, transform.position);
			}
			if (col.CompareTag("Enemy") || col.CompareTag("Player"))
			{
				effect = impactEffectBlood;
			}
			else
			{
				effect = impactEffectWall;
			}
			GameObject go = ObjectPool.Get(effect, pos,
				Quaternion.identity, HolderManager.Particles);
		}
		public void ReturnToPool()
		{
			ObjectPool.Return(gameObject);
		}
	}
}