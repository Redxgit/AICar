using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car2DController : MonoBehaviour {
	[SerializeField] private Rigidbody2D rbody;
	private float horizontal, vertical;

	[SerializeField] private float speed = 1f;
	[SerializeField] private float turnSpeed = 1f;

	[SerializeField] private float dampForward;
	[SerializeField] private float dampRight;

	// Use this for initialization
	void Start() { }

	// Update is called once per frame
	void Update() {
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");
	}

	private void FixedUpdate() {
		rbody.AddForce(transform.up * vertical * speed);
		//rbody.AddForce(transform.right * horizontal * horizontalVelocity);
		//rbody.AddTorque(horizontal * turnSpeed * -1f);
		rbody.angularVelocity = horizontal * turnSpeed * -1f;


		DampVelocity();
	}

	private void DampVelocity() {
		rbody.velocity = ForwardVelocity() * dampForward + RightVelocity() * dampRight;
	}

	private Vector2 ForwardVelocity() {
		return Vector2.Dot(rbody.velocity, transform.up) * transform.up;
	}

	private Vector2 RightVelocity() {
		return Vector2.Dot(rbody.velocity, transform.right) * transform.right;
	}

	private void OnTriggerExit2D(Collider2D other) {
		Debug.Log("Should die");

		RaycastHit2D hitInfo;
		RaycastHit hitInfo2;
		
	}
}