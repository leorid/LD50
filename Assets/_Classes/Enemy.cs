using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JL
{
	public class Enemy : MonoBehaviour
	{
		public int health = 1;

		public float detectRange = 15;
		public Vector2 wantedFireDistance = new Vector2(4, 6);
		public float fireRange = 8;
		public float lookRotSpeed = 10;
		public float acceleration = 10;
		public float moveSpeed = 3;
		public float rotationSpeed = 20;
		public GameObject deadEnemy;
		public WeaponController weaponController;
		public LayerMask mask;

		Rigidbody2D _rb;
		float playerDst;

		Vector2 moveVec;
		Vector2 wantedRotation;

		public float fireRate = 0.5f;
		float lastFireTime;

		[Header("Info")]
		[SerializeField] bool playerSpotted;
		[SerializeField] bool inDetectionRange;
		[SerializeField] bool inFireRange;

		void Awake()
		{
			_rb = GetComponent<Rigidbody2D>();
		}

		void Update()
		{
			moveVec = Vector2.zero;

			Vector2 vec = GlobalGameVariables.playerPos - (Vector2)transform.position;
			playerDst = vec.magnitude;
			CheckRange();
			if (!playerSpotted)
			{
				if (inDetectionRange)
				{
					RaycastHit2D hit =
						Physics2D.Raycast(transform.position,
							vec,
							playerDst,
							mask);
					if (hit.collider && hit.collider.CompareTag("Player"))
					{
						playerSpotted = true;
					}
				}
				lastFireTime = Time.time;
			}
			else
			{
				if (inFireRange && Time.time - lastFireTime > fireRate)
				{
					lastFireTime = Time.time;
					WeaponInput weaponInput = new WeaponInput()
					{
						lmbDown = true,
						lmbHeld = true,
					};
					weaponController.GetInput(weaponInput);
				}

				// go to wanted distance
				if (playerDst < wantedFireDistance.x)
				{
					moveVec = -vec.normalized;
				}
				else if(playerDst > wantedFireDistance.y)
				{
					moveVec = vec.normalized;
				}

				// out of range
				if (!inDetectionRange)
				{
					playerSpotted = false;
				}
			}
		}

		void CheckRange()
		{
			inDetectionRange = playerDst < detectRange;
			inFireRange = playerDst < fireRange;
		}

		void FixedUpdate()
		{
			Vector2 wantedVeclocity = moveVec * moveSpeed;

			_rb.velocity = Vector2.MoveTowards(_rb.velocity, wantedVeclocity, acceleration * Time.fixedDeltaTime);

			if (playerSpotted)
			{
				Vector2 vec = GlobalGameVariables.playerPos - (Vector2)transform.position;
				RotateTo(vec);
			}
			else if (_rb.velocity.magnitude > 0.01f)
			{
				RotateTo(_rb.velocity);
			}
		}

		void RotateTo(Vector2 lookVec)
		{
			lookVec.Normalize();
			float wantedRotation = Vector2.SignedAngle(Vector2.up, lookVec);

			_rb.rotation = Mathf.MoveTowardsAngle(
				  _rb.rotation,
				  wantedRotation,
				  rotationSpeed * Time.fixedDeltaTime);
		}

		public void Damage(int dmg)
		{
			health -= dmg;
			if (health <= 0)
			{
				// die
				Destroy(gameObject);
				Instantiate(deadEnemy,
					transform.position,
					Quaternion.Euler(0, 0, 180) * transform.rotation,
					HolderManager.Get(deadEnemy));
			}
		}

		private void OnDrawGizmosSelected()
		{
			var col = Gizmos.color;
			Gizmos.color = Color.white;
			Gizmos.DrawWireSphere(transform.position, detectRange);
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, fireRange);
			Gizmos.color = col;
		}
	}
}
