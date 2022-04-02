using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	Rigidbody2D _rb;
	public float moveSpeed;
	public Transform playerVisuals;
	public Camera cam;
	public Vector3 offsetRot;

	void Start()
	{
		_rb = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		Vector2 input = new Vector2(
			Input.GetAxis("Horizontal"),
			Input.GetAxis("Vertical"));

		_rb.velocity = input * moveSpeed;

		Vector3 worldPos = cam.ScreenToWorldPoint(Input.mousePosition);

		Vector3 lookVec = worldPos-transform.position;
		lookVec.z = 0;
		lookVec.Normalize();
		playerVisuals.up = lookVec;
	}
}
