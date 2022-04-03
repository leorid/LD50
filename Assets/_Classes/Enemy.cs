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
		Camera cam;

		public float fireRate = 0.5f;
		float lastFireTime;

		[Header("Info")]
		[SerializeField] bool playerSpotted;
		[SerializeField] bool inDetectionRange;
		[SerializeField] bool inFireRange;
		[SerializeField] bool isOnScreen;

		List<Collider2D> _colliders = new List<Collider2D>();


		void Awake()
		{
			_rb = GetComponent<Rigidbody2D>();
			GetComponentsInChildren(_colliders);
		}

		void Update()
		{
			if (!cam) cam = Camera.main;

			moveVec = Vector2.zero;
			Vector2 vec = GlobalGameVariables.playerPos - (Vector2)transform.position;

			bool seeingPlayer = false;
			RaycastHit2D hit;
			SetCollidersEnabled(false);
			{
				hit = Physics2D.Raycast(transform.position,
						vec,
						playerDst,
						mask);
			}
			SetCollidersEnabled(true);
			if (hit.collider && hit.collider.CompareTag("Player"))
			{
				seeingPlayer = true;
			}

			playerDst = vec.magnitude;
			CheckRange();
			if (!playerSpotted)
			{
				if (inDetectionRange && seeingPlayer)
				{
					playerSpotted = true;
				}
				lastFireTime = Time.time;
			}
			else
			{
				if (inFireRange && Time.time - lastFireTime > fireRate && seeingPlayer)
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
				else if (playerDst > wantedFireDistance.y)
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
			if (!cam) return;

			isOnScreen = false;
			Vector2 camSpace = cam.WorldToViewportPoint(transform.position);
			if (camSpace.x < 1 && camSpace.x > 0 && camSpace.y < 1 && camSpace.y > 0)
			{
				isOnScreen = true;
			}
			//inDetectionRange = playerDst < detectRange;
			inDetectionRange = isOnScreen;
			inFireRange = playerDst < fireRange && isOnScreen;
		}

		void SetCollidersEnabled(bool enable)
		{
			foreach (var col in _colliders)
			{
				col.enabled = enable;
			}
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

		public void Damage(DamageInfo damageInfo)
		{
			health -= damageInfo.damage;
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
