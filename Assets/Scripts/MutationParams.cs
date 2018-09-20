using UnityEngine;

[CreateAssetMenu(fileName = "New Mutation Params", menuName = "MutationParams")]
public class MutationParams : ScriptableObject {
	public Vector3Int resolutionFactorMinMax;
	public Vector3Int maxAngleFactorMinMax;
	public Vector3 maxRayDistFactorMinMax;
	public Vector3 distanceFactorMinMax;
	public Vector3 angleFactorMinMax;
	public Vector3 speedFactorMinMax;
	public Vector3 turnSpeedFactorMinMax;
	public Vector3 dampForwardFactorMinMax;
	public Vector3 dampRightFactorMinMax;
	public Vector3 criticalDistanceFactorMinMax;
	public Vector3 fwdCriticalDampFactorMinMax;
}