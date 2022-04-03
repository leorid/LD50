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

		public int health = 20;

		Vector2 moveInput;
		Vector3 mouseWorldPos;

		List<Collider2D> _colCache = new List<Collider2D>();

		void Start()
		{
			_rb = GetComponent<Rigidbody2D>();
		}

		void Update()
		{
			moveInput = new Vector2(
				Input.GetAxis("Horizontal"),
				Input.GetAxis("Vertical"));

			WeaponInput weaponInput = new WeaponInput()
			{
				lmbDown = Input.GetButtonDown("Fire1"),
				lmbHeld = Input.GetButton("Fire1"),
				lmbUp = Input.GetButtonUp("Fire1"),
			};
			weaponController.GetInput(weaponInput);

			mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);

			if (Input.GetKeyDown(KeyCode.E))
			{
				_colCache.Clear();
				int hitCount = Physics2D.OverlapCircle(transform.position, 2, new ContactFilter2D()
				{
					layerMask = ~0,
					useTriggers = true,
				}, _colCache);
				for (int i = 0; i < hitCount; i++)
				{
					if (!_colCache[i]) continue;
					_colCache[i].gameObject.SendMessage("Interact",
						SendMessageOptions.DontRequireReceiver);
				}
			}

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
			if (weaponController.currentWeapon)
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
			Vector2 muzzlePos = transform.position + rotationTransform.up;
			if (weaponController.currentWeapon)
			{
				muzzlePos = weaponController.currentWeapon.muzzlePos.position;
			}
			Vector2 endPos = muzzlePos + (Vector2)rotationTransform.up * 20;
			RaycastHit2D aimHit = Physics2D.Raycast(muzzlePos, rotationTransform.up);
			if (aimHit.collider) endPos = aimHit.point;
			aimLine.SetPosition(0, muzzlePos);
			aimLine.SetPosition(1, endPos);
		}
	}
}
