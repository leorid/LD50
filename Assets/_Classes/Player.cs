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
		public Transform playerVisuals;
		public Camera cam;
		public Vector3 offsetRot;
		public WeaponController weaponController;
		public LineRenderer aimLine;
		public LayerMask mask;

		public int health = 20;

		Vector2 moveInput;
		Vector3 mouseWorldPos;

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

			GlobalGameVariables.playerPos = transform.position;
		}
		void FixedUpdate()
		{
			Vector2 wantedVeclocity = moveInput * moveSpeed;

			_rb.velocity = Vector2.MoveTowards(_rb.velocity, wantedVeclocity, acceleration * Time.fixedDeltaTime);

			RotateToMouse();

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
			_rb.rotation = Vector2.SignedAngle(Vector2.up, lookVec);
		}

		void UpdateAimLine()
		{
			Vector2 muzzlePos = transform.position + transform.up;
			if (weaponController.currentWeapon)
			{
				muzzlePos = weaponController.currentWeapon.muzzlePos.position;
			}
			Vector2 endPos = muzzlePos + (Vector2)transform.up * 20;
			RaycastHit2D aimHit = Physics2D.Raycast(muzzlePos, transform.up);
			if (aimHit.collider) endPos = aimHit.point;
			aimLine.SetPosition(0, muzzlePos);
			aimLine.SetPosition(1, endPos);
		}
	}
}
