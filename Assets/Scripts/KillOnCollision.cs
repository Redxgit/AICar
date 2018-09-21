using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnCollision : MonoBehaviour {
	private void OnCollisionEnter2D(Collision2D other) {
		CarAI c = other.transform.GetComponent<CarAI>();
		if (c != null) {
			c.TellMngerDied();
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		CarAI c = other.transform.GetComponent<CarAI>();
		if (c != null) {
			c.TellMngerDied();
		}
	}
}