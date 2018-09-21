using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnCollision2 : MonoBehaviour {
	private void OnCollisionEnter2D(Collision2D other) {
		CarAI2 c = other.transform.GetComponent<CarAI2>();
		if (c != null) {
			c.TellMngerDied();
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		CarAI2 c = other.transform.GetComponent<CarAI2>();
		if (c != null) {
			c.TellMngerDied();
		}
	}
}
