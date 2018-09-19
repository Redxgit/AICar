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
}