using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	[SerializeField] private Transform target;

	[SerializeField] private Vector3 offset;

	private Vector3 currentVelocity;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		//transform.position = Vector3.SmoothDamp(transform.position , target.position + offset, ref currentVelocity, Time.deltaTime);
		transform.position = target.position + offset;
	}
}
