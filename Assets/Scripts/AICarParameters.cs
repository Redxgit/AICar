using UnityEngine;

[CreateAssetMenu(fileName = "New AICarParameters", menuName = "AICarParameters")]
public class AICarParameters : ScriptableObject {
	public int resolution;

	public int maxAngle;

	public float maxRayDist;

	public float distanceFactor;
	public float angleFactor;

	public float speed = 1f;
	public float turnSpeed = 1f;

	public float dampForward;
	public float dampRight;
	public float criticalDistance;
	public float fwdCriticalDamp;

	public string Description;
	public int ID;

	public bool completesTrack;
	public float timeToComplete;

	public AICarParms ToCarParams() {
		AICarParms ap = new AICarParms(resolution, maxAngle, maxRayDist, distanceFactor, angleFactor, speed, turnSpeed,
			dampForward, dampRight, criticalDistance, fwdCriticalDamp, completesTrack, timeToComplete);
		return ap;
	}
}

[System.Serializable]
public class AICarParms {
	public int resolution;

	public int maxAngle;

	public float maxRayDist;

	public float distanceFactor;
	public float angleFactor;

	public float speed;
	public float turnSpeed;

	public float dampForward;
	public float dampRight;
	public float criticalDistance;
	public float fwdCriticalDamp;

	public string Description;
	public int ID;

	public bool completesTrack;
	public float timeToComplete;

	public AICarParms(int resolution, int maxAngle, float maxRayDist, float distanceFactor, float angleFactor,
		float speed, float turnSpeed,
		float dampForward, float dampRight, float criticalDistance, float fwdCriticalDamp, bool completesTrack,
		float timeToComplete) {
		this.resolution = resolution;
		this.maxAngle = maxAngle;
		this.maxRayDist = maxRayDist;
		this.distanceFactor = distanceFactor;
		this.angleFactor = angleFactor;
		this.dampForward = dampForward;
		this.dampRight = dampRight;
		this.criticalDistance = criticalDistance;
		this.fwdCriticalDamp = fwdCriticalDamp;
		this.completesTrack = completesTrack;
		this.timeToComplete = timeToComplete;
		this.speed = speed;
		this.turnSpeed = turnSpeed;
	}

	public AICarParms() { }

	public AICarParms(int resolution, int maxAngle, float maxRayDist, float distanceFactor, float angleFactor,
		float speed, float turnSpeed, float dampForward, float dampRight, float criticalDistance,
		float fwdCriticalDamp) {
		this.resolution = resolution;
		this.maxAngle = maxAngle;
		this.maxRayDist = maxRayDist;
		this.distanceFactor = distanceFactor;
		this.angleFactor = angleFactor;
		this.speed = speed;
		this.turnSpeed = turnSpeed;
		this.dampForward = dampForward;
		this.dampRight = dampRight;
		this.criticalDistance = criticalDistance;
		this.fwdCriticalDamp = fwdCriticalDamp;
	}
}