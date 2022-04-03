using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JL
{
	public class Player : MonoBehaviour
	{
		Rigidbody2D _rb;

		public float moveSpeed;
		public float acceleration = 10;
		public Transform rotationTransform;
		public Camera cam;
		public Vector3 offsetRot;
		public WeaponController weaponController;
		public LineRenderer aimLine;
		public LayerMask mask;
		public GameObject canInteractIcon;
		public UI_HealthBar healthBar;
		public UI_DeathScreen deathScreen;

		public int health;
		public int maxHealth = 20;

		Vector2 moveInput;
		Vector3 mouseWorldPos;

		List<Collider2D> _colCache = new List<Collider2D>();

		public static System.Action RespawnAction;

		void Start()
		{
			_rb = GetComponent<Rigidbody2D>();

			RespawnAction = Respawn;
		}

		void Update()
		{
			if (health < 0)
			{
				return;
			}
			moveInput = new Vector2(
			Input.GetAxis("Horizontal"),
			Input.GetAxis("Vertical"));

			if (moveInput.magnitude > 1) moveInput.Normalize();

			WeaponInput weaponInput = new WeaponInput()
			{
				lmbDown = Input.GetButtonDown("Fire1"),
				lmbHeld = Input.GetButton("Fire1"),
				lmbUp = Input.GetButtonUp("Fire1"),
			};
			weaponController.GetInput(weaponInput);

			mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);

			// weapon inputs
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				weaponController.SetWeapon(1);
			}
			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				weaponController.SetWeapon(2);
			}
			if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				weaponController.SetWeapon(3);
			}
			// weapon scroll wheel
			if (Input.GetAxis("Mouse ScrollWheel") > 0.01f)
			{
				weaponController.PreviousWeapon();
			}
			if (Input.GetAxis("Mouse ScrollWheel") < -0.01f)
			{
				weaponController.NextWeapon();
			}
			GlobalGameVariables.playerPos = transform.position;

			RotateToMouse();

			CheckInteractables();
		}
		void FixedUpdate()
		{
			Vector2 wantedVeclocity = moveInput * moveSpeed;

			_rb.velocity = Vector2.MoveTowards(_rb.velocity, wantedVeclocity, acceleration * Time.fixedDeltaTime);


			GlobalGameVariables.playerPos = transform.position;
		}

		private void LateUpdate()
		{
			UpdateAimLine();
		}

		void RotateToMouse()
		{
			Vector2 tempMouseWorldPos = mouseWorldPos;
			if (weaponController.currentWeapon && !weaponController.isNoWeapon)
			{
				Vector2 muzzlePos =
					weaponController.currentWeapon.muzzlePos.position - transform.position;
				tempMouseWorldPos -= muzzlePos;
			}

			Vector3 lookVec = tempMouseWorldPos - (Vector2)transform.position;
			lookVec.z = 0;
			lookVec.Normalize();
			rotationTransform.rotation = Quaternion.Euler(0,
				0,
				Vector2.SignedAngle(Vector2.up, lookVec));
		}

		void UpdateAimLine()
		{
			if (weaponController.currentWeapon && !weaponController.isNoWeapon)
			{
				aimLine.enabled = true;
				Vector2 muzzlePos = weaponController.currentWeapon.muzzlePos.position;
				Vector2 endPos = muzzlePos + (Vector2)rotationTransform.up * 20;
				RaycastHit2D aimHit = Physics2D.Raycast(muzzlePos, rotationTransform.up);
				if (aimHit.collider) endPos = aimHit.point;
				aimLine.SetPosition(0, muzzlePos);
				aimLine.SetPosition(1, endPos);
			}
			else
			{
				aimLine.enabled = false;
			}
		}

		void CheckInteractables()
		{
			_colCache.Clear();
			int hitCount = Physics2D.OverlapCircle(transform.position, 2, new ContactFilter2D()
			{
				layerMask = ~0,
				useTriggers = true,
			}, _colCache);

			bool eKeyDown = Input.GetKeyDown(KeyCode.E);
			bool anyInteractable = false;

			for (int i = 0; i < hitCount; i++)
			{
				if (!_colCache[i]) continue;

				if (_colCache[i].TryGetComponent(out InteractableBase interactable))
				{
					anyInteractable = true;
					if (eKeyDown)
					{
						interactable.OnInteract();

						if (_colCache[i].TryGetComponent(out WeaponPickup weaponPickup))
						{
							weaponController.SetWeaponUnlocked(weaponPickup.weaponIndex);
							weaponPickup.Pickup();
						}
					}
				}
			}

			canInteractIcon.SetActive(anyInteractable);
		}

		public void Damage(DamageInfo damageInfo)
		{
			health -= damageInfo.damage;

			if (health < 0)
			{
				deathScreen.Show();
			}

			float normalized = health / (float)maxHealth;
			healthBar.HealthChanged(normalized);
		}

		public void Respawn()
		{
			health = maxHealth;
			healthBar.HealthChanged(1);
			transform.position = Vector3.zero;
			weaponController.LockAll();
		}
	}
}
