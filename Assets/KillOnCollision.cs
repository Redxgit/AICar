using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnCollisionEnter2D(Collision2D other) {
		CarAI c = other.transform.GetComponent<CarAI>();
		if (c != null) {
			c.DieAndReset();
		}
	}
}
