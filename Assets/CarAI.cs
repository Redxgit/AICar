using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class CarAI : MonoBehaviour {
	[SerializeField] private Transform rayOrigin;

	[SerializeField] private Transform rayDir;
	[SerializeField] private LayerMask mask;

	private int resolution;
	private int maxAngle;
	private float maxRayDist;
	private float distanceFactor;
	private float angleFactor;
	private float speed = 1f;
	private float turnSpeed = 1f;
	private float dampForward;
	private float dampRight;
	private float criticalDistance;
	private float fwdCriticalDamp;

	[SerializeField] private AICarParameters parameters;

	[SerializeField] private Rigidbody2D rBody;

	private float generatedHorizontal;
	private float generatedVertical;

	private Vector3 initPos;
	private Quaternion initRot;

	private RayInfo[] rayInfoStruct;
	// Use this for initialization

	private void LoadValues() {
		resolution = parameters.resolution;
		maxAngle = parameters.maxAngle;

		maxRayDist = parameters.maxRayDist;
		distanceFactor = parameters.distanceFactor;
		angleFactor = parameters.angleFactor;
		speed = parameters.speed;
		turnSpeed = parameters.turnSpeed;
		dampForward = parameters.dampForward;
		dampRight = parameters.dampRight;
		criticalDistance = parameters.criticalDistance;
		fwdCriticalDamp = parameters.fwdCriticalDamp;
	}

	private void OnDrawGizmo() {
		Gizmos.color = Color.cyan;
		RaycastHit2D hit;
		rayDir.localEulerAngles = new Vector3(0, 0, -maxAngle);
		for (int i = 0; i < resolution; i++) {
			//rayOrigin.Rotate(Vector3.up, maxAngle + maxAngle / resolution);
			//Vector2 dir = new Vector2( 0, rayOrigin.eulerAngles.z - maxAngle + maxAngle / resolution * i);
			//Vector2 dir = Vector2.one;
			hit = Physics2D.Raycast(rayOrigin.position, rayDir.right, maxRayDist, mask);
			if (hit.point != Vector2.zero) {
				Gizmos.DrawLine(rayOrigin.position, hit.point);
			}
			else {
				Gizmos.DrawLine(rayOrigin.position, rayDir.right * maxRayDist);
			}

			rayDir.Rotate(Vector3.up, maxAngle * 2 / (float) resolution, Space.Self);
		}
	}

	[ContextMenu("asdf")]
	private void DrawLines() {
		RaycastHit2D hit;
		rayDir.localEulerAngles = new Vector3(0, 0, -maxAngle);
		for (int i = 0; i < resolution; i++) {
			//rayOrigin.Rotate(Vector3.up, maxAngle + maxAngle / resolution);
			//Vector2 dir = new Vector2( 0, rayOrigin.eulerAngles.z - maxAngle + maxAngle / resolution * i);
			//Vector2 dir = Vector2.one;
			hit = Physics2D.Raycast(rayOrigin.position, rayDir.up, maxRayDist, mask);
			if (hit.point != Vector2.zero) {
				Debug.DrawLine(rayOrigin.position, hit.point, Color.cyan, 3f);
			}
			else {
				Debug.DrawLine(rayOrigin.position, rayDir.up * maxRayDist, Color.cyan, 3f);
			}

			Debug.Log(maxAngle * 2 / (float) resolution);
			rayDir.localEulerAngles =
				new Vector3(0, 0, rayDir.localEulerAngles.z + (maxAngle * 2 / (float) resolution));
			//	rayDir.Rotate(rayDir.up, maxAngle*2 / (float)resolution);
		}
	}

	void Start() {
		LoadValues();
		initPos = transform.position;
		initRot = transform.rotation;
		rayInfoStruct = new RayInfo[resolution];
	}

	// Update is called once per frame
	void Update() {
		RaycastHit2D hit;
		rayDir.localEulerAngles = new Vector3(0, 0, -maxAngle);
		for (int i = 0; i < resolution; i++) {
			//rayOrigin.Rotate(Vector3.up, maxAngle + maxAngle / resolution);
			//Vector2 dir = new Vector2( 0, rayOrigin.eulerAngles.z - maxAngle + maxAngle / resolution * i);
			//Vector2 dir = Vector2.one;
			hit = Physics2D.Raycast(rayOrigin.position, rayDir.up, maxRayDist, mask);
			if (hit.point != Vector2.zero) {
				Debug.DrawLine(rayOrigin.position, hit.point, Color.cyan);
				rayInfoStruct[i].Distance = hit.distance;
			}
			else {
				rayInfoStruct[i].Distance = -1f;
				Debug.DrawLine(rayOrigin.position, rayOrigin.position + rayDir.up * maxRayDist, Color.cyan);
			}

			//rayInfoStruct[i].Angle = -maxAngle + maxAngle / resolution * i;
			//rayInfoStruct[i].Angle = rayDir.localEulerAngles.z;
			rayInfoStruct[i].Angle = -maxAngle + (maxAngle * 2 / (resolution - 1) * i);
			//rayDir.localEulerAngles = new Vector3(0,0, rayDir.localEulerAngles.z + (maxAngle*2 / (float)resolution));
			//	rayDir.Rotate(rayDir.up, maxAngle*2 / (float)resolution);
			rayDir.Rotate(0, 0, maxAngle * 2 / (float) (resolution - 1));
		}
	}

	private void FixedUpdate() {
		ProcessRayInfo();
		rBody.AddForce(transform.up * generatedVertical * speed);
		//rBody.AddForce(transform.right * horizontal * horizontalVelocity);
		//rBody.AddTorque(horizontal * turnSpeed * -1f);
		rBody.angularVelocity = generatedHorizontal * turnSpeed * -1f;

		DampVelocity();
	}

	private void DampVelocity() {
		rBody.velocity = ForwardVelocity() * dampForward + RightVelocity() * dampRight;
	}

	private Vector2 ForwardVelocity() {
		return Vector2.Dot(rBody.velocity, transform.up) * transform.up;
	}

	private Vector2 RightVelocity() {
		return Vector2.Dot(rBody.velocity, transform.right) * transform.right;
	}

	private void ProcessRayInfo() {
		generatedHorizontal = 0;
		generatedVertical = 1f;
		bool alreadyCritical = false;
		for (int i = 0; i < resolution; i++) {
			if (rayInfoStruct[i].Distance > 0f) {
				generatedHorizontal += Mathf.Sign(rayInfoStruct[i].Angle) * (
					                       distanceFactor / rayInfoStruct[i].Distance +
					                       angleFactor * Mathf.Abs(rayInfoStruct[i].Angle));


				if (!alreadyCritical && rayInfoStruct[i].Distance < criticalDistance) {
					alreadyCritical = true;
					generatedVertical /= fwdCriticalDamp;
				}
			}
		}

		//generatedHorizontal = Mathf.Clamp(generatedHorizontal, -1, 1);
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.layer == mask) {
			DieAndReset();
		}
	}

	public void DieAndReset() {
		transform.SetPositionAndRotation(initPos, initRot);
	}

	[System.Serializable]
	struct RayInfo {
		public float Distance;
		public float Angle;
	}
}