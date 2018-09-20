using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Evolution Container", menuName = "EvolutionContainer")]
public class EvolutionContainer : ScriptableObject {
	//public List<AICarParms> parameters;
	public List<AICarParms> succesfulParams;

	//public List<AICarParms> failedParms;

	public MutationParams MutationParams;

	public AICarParameters baseParam;

	public int ChanceToGrabFromOthers;

	public int TopToKeep;

	[ContextMenu("IndexOfBest")]
	public void IndexOfBest() {
		if (succesfulParams.Count > 0) {
			float minTime = Mathf.Infinity;
			int bestIndex = 0;
			for (int i = 0; i < succesfulParams.Count; i++) {
				if (succesfulParams[i].timeToComplete < minTime) {
					minTime = succesfulParams[i].timeToComplete;
					bestIndex = i;
				}
			}

			Debug.Log("Index of best: " + bestIndex);
		}
		else {
			Debug.Log("None Succesful");
		}
	}

	[ContextMenu("FilterToTop")]
	public void FilterToTop() {
		succesfulParams = FilterToTop(succesfulParams, TopToKeep);
	}

	[ContextMenu("FilterErrors")]
	public void FilterErrors() {
		for (int i = succesfulParams.Count - 1; i > 0; i--) {
			if (succesfulParams[i].timeToComplete < 10) {
				succesfulParams.RemoveAt(i);
			}
		}
	}

	[ContextMenu("ResetData")]
	public void ResetData() {
		//parameters = new List<AICarParms>();
		succesfulParams = new List<AICarParms>();
		//failedParms = new List<AICarParms>();
	}

	public void Initialize() {
		//parameters = new List<AICarParms> {baseParam.ToCarParams()};
		if (baseParam.completesTrack) {
			succesfulParams = new List<AICarParms> {baseParam.ToCarParams()};
			//	failedParms = new List<AICarParms>();
		}
		else {
			//	failedParms = new List<AICarParms> {baseParam.ToCarParams()};
			succesfulParams = new List<AICarParms>();
		}
	}

	public AICarParms RandomizeParams() {
		int newResolution = Random.Range(
			MutationParams.resolutionFactorMinMax.y,
			MutationParams.resolutionFactorMinMax.z);

		int newMaxAngle = Random.Range(
			MutationParams.maxAngleFactorMinMax.y,
			MutationParams.maxAngleFactorMinMax.z);

		float newMaxRayDist = Random.Range(
			MutationParams.maxRayDistFactorMinMax.y,
			MutationParams.maxRayDistFactorMinMax.z);

		float newDistanceFactor = Random.Range(
			MutationParams.distanceFactorMinMax.y,
			MutationParams.distanceFactorMinMax.z);

		float newAngleFactor = Random.Range(
			MutationParams.angleFactorMinMax.y,
			MutationParams.angleFactorMinMax.z);

		float newSpeed = Random.Range(
			MutationParams.speedFactorMinMax.y,
			MutationParams.speedFactorMinMax.z);

		float turnSpeed = Random.Range(
			MutationParams.turnSpeedFactorMinMax.y,
			MutationParams.turnSpeedFactorMinMax.z);

		float newDampForward = Random.Range(
			MutationParams.dampForwardFactorMinMax.y,
			MutationParams.dampForwardFactorMinMax.z);

		float newDampRight = Random.Range(
			MutationParams.dampRightFactorMinMax.y,
			MutationParams.dampRightFactorMinMax.z);

		float newCriticalDistance = Random.Range(
			MutationParams.criticalDistanceFactorMinMax.y,
			MutationParams.criticalDistanceFactorMinMax.z);

		float fwdCriticalDamp = Random.Range(
			MutationParams.fwdCriticalDampFactorMinMax.y,
			MutationParams.fwdCriticalDampFactorMinMax.z);


		AICarParms newParam = new AICarParms(newResolution, newMaxAngle, newMaxRayDist, newDistanceFactor,
			newAngleFactor, newSpeed, turnSpeed, newDampForward, newDampRight, newCriticalDistance, fwdCriticalDamp);

		//parameters.Add(newParam);

		return newParam;
	}

	public AICarParms Mutate2(AICarParms prevPara) {
		if (prevPara.completesTrack) {
			succesfulParams = TryToAdd(succesfulParams, prevPara, TopToKeep);
		}

		if (succesfulParams.Count == 0) {
			return Mutate(prevPara);
		}

		AICarParms newParam = new AICarParms();
		if (Random.Range(0, 10) < ChanceToGrabFromOthers) {
			return GrabFromOthers(newParam);
		}

		return CombineParams(succesfulParams[Random.Range(0, succesfulParams.Count)],
			succesfulParams[Random.Range(0, succesfulParams.Count)]);
	}

	private AICarParms CombineParams(AICarParms p1, AICarParms p2) {
		int newResolution = Mathf.Clamp((p1.resolution + p2.resolution) / 2 + Random.Range(
			                                -MutationParams.resolutionFactorMinMax.x,
			                                MutationParams.resolutionFactorMinMax.x),
			MutationParams.resolutionFactorMinMax.y, MutationParams.resolutionFactorMinMax.z);

		int newMaxAngle = Mathf.Clamp((p1.maxAngle + p2.maxAngle) / 2 + Random.Range(
			                              -MutationParams.maxAngleFactorMinMax.x,
			                              MutationParams.maxAngleFactorMinMax.x),
			MutationParams.maxAngleFactorMinMax.y, MutationParams.maxAngleFactorMinMax.z);

		float newMaxRayDist = Mathf.Clamp((p1.maxRayDist + p2.maxRayDist) / 2 + Random.Range(
			                                  -MutationParams.maxRayDistFactorMinMax.x,
			                                  MutationParams.maxRayDistFactorMinMax.x),
			MutationParams.maxRayDistFactorMinMax.y, MutationParams.maxRayDistFactorMinMax.z);

		float newDistanceFactor = Mathf.Clamp((p1.distanceFactor + p2.distanceFactor) / 2 + Random.Range(
			                                      -MutationParams.distanceFactorMinMax.x,
			                                      MutationParams.distanceFactorMinMax.x),
			MutationParams.distanceFactorMinMax.y, MutationParams.distanceFactorMinMax.z);

		float newAngleFactor = Mathf.Clamp((p1.angleFactor + p2.angleFactor) / 2 + Random.Range(
			                                   -MutationParams.angleFactorMinMax.x,
			                                   MutationParams.angleFactorMinMax.x),
			MutationParams.angleFactorMinMax.y, MutationParams.angleFactorMinMax.z);

		float newSpeed = Mathf.Clamp((p1.speed + p2.speed) / 2 + Random.Range(
			                             -MutationParams.speedFactorMinMax.x,
			                             MutationParams.speedFactorMinMax.x),
			MutationParams.speedFactorMinMax.y, MutationParams.speedFactorMinMax.z);

		float turnSpeed = Mathf.Clamp((p1.turnSpeed + p2.turnSpeed) / 2 + Random.Range(
			                              -MutationParams.turnSpeedFactorMinMax.x,
			                              MutationParams.turnSpeedFactorMinMax.x),
			MutationParams.turnSpeedFactorMinMax.y, MutationParams.turnSpeedFactorMinMax.z);

		float newDampForward = Mathf.Clamp((p1.dampForward + p2.dampForward) / 2 + Random.Range(
			                                   -MutationParams.dampForwardFactorMinMax.x,
			                                   MutationParams.dampForwardFactorMinMax.x),
			MutationParams.dampForwardFactorMinMax.y, MutationParams.dampForwardFactorMinMax.z);

		float newDampRight = Mathf.Clamp((p1.dampRight + p2.dampRight) / 2 + Random.Range(
			                                 -MutationParams.dampRightFactorMinMax.x,
			                                 MutationParams.dampRightFactorMinMax.x),
			MutationParams.dampRightFactorMinMax.y, MutationParams.dampRightFactorMinMax.z);

		float newCriticalDistance = Mathf.Clamp((p1.criticalDistance + p2.criticalDistance) / 2 + Random.Range(
			                                        -MutationParams.criticalDistanceFactorMinMax.x,
			                                        MutationParams.criticalDistanceFactorMinMax.x),
			MutationParams.criticalDistanceFactorMinMax.y, MutationParams.criticalDistanceFactorMinMax.z);

		float fwdCriticalDamp = Mathf.Clamp((p1.fwdCriticalDamp + p2.fwdCriticalDamp) / 2 + Random.Range(
			                                    -MutationParams.fwdCriticalDampFactorMinMax.x,
			                                    MutationParams.fwdCriticalDampFactorMinMax.x),
			MutationParams.fwdCriticalDampFactorMinMax.y, MutationParams.fwdCriticalDampFactorMinMax.z);


		AICarParms newParam = new AICarParms(newResolution, newMaxAngle, newMaxRayDist, newDistanceFactor,
			newAngleFactor, newSpeed, turnSpeed, newDampForward, newDampRight, newCriticalDistance, fwdCriticalDamp);

		//parameters.Add(newParam);

		return newParam;
	}

	private AICarParms GrabFromOthers(AICarParms param) {
		param.resolution = succesfulParams[Random.Range(0, succesfulParams.Count)].resolution;
		param.maxAngle = succesfulParams[Random.Range(0, succesfulParams.Count)].maxAngle;
		param.maxRayDist = succesfulParams[Random.Range(0, succesfulParams.Count)].maxRayDist;
		param.distanceFactor = succesfulParams[Random.Range(0, succesfulParams.Count)].distanceFactor;
		param.angleFactor = succesfulParams[Random.Range(0, succesfulParams.Count)].angleFactor;
		param.speed = succesfulParams[Random.Range(0, succesfulParams.Count)].speed;
		param.turnSpeed = succesfulParams[Random.Range(0, succesfulParams.Count)].turnSpeed;
		param.dampForward = succesfulParams[Random.Range(0, succesfulParams.Count)].dampForward;
		param.dampRight = succesfulParams[Random.Range(0, succesfulParams.Count)].dampRight;
		param.criticalDistance = succesfulParams[Random.Range(0, succesfulParams.Count)].criticalDistance;
		param.fwdCriticalDamp = succesfulParams[Random.Range(0, succesfulParams.Count)].fwdCriticalDamp;

		return param;
	}


	public AICarParms Mutate(AICarParms previousIteration) {
		if (previousIteration.completesTrack) {
			//succesfulParams.Add(previousIteration);
		}
		else {
			//failedParms.Add(previousIteration);
		}

		int newResolution = Mathf.Clamp(previousIteration.resolution + Random.Range(
			                                -MutationParams.resolutionFactorMinMax.x,
			                                MutationParams.resolutionFactorMinMax.x),
			MutationParams.resolutionFactorMinMax.y, MutationParams.resolutionFactorMinMax.z);

		int newMaxAngle = Mathf.Clamp(previousIteration.maxAngle + Random.Range(
			                              -MutationParams.maxAngleFactorMinMax.x,
			                              MutationParams.maxAngleFactorMinMax.x),
			MutationParams.maxAngleFactorMinMax.y, MutationParams.maxAngleFactorMinMax.z);

		float newMaxRayDist = Mathf.Clamp(previousIteration.maxRayDist + Random.Range(
			                                  -MutationParams.maxRayDistFactorMinMax.x,
			                                  MutationParams.maxRayDistFactorMinMax.x),
			MutationParams.maxRayDistFactorMinMax.y, MutationParams.maxRayDistFactorMinMax.z);

		float newDistanceFactor = Mathf.Clamp(previousIteration.distanceFactor + Random.Range(
			                                      -MutationParams.distanceFactorMinMax.x,
			                                      MutationParams.distanceFactorMinMax.x),
			MutationParams.distanceFactorMinMax.y, MutationParams.distanceFactorMinMax.z);

		float newAngleFactor = Mathf.Clamp(previousIteration.angleFactor + Random.Range(
			                                   -MutationParams.angleFactorMinMax.x,
			                                   MutationParams.angleFactorMinMax.x),
			MutationParams.angleFactorMinMax.y, MutationParams.angleFactorMinMax.z);

		float newSpeed = Mathf.Clamp(previousIteration.speed + Random.Range(
			                             -MutationParams.speedFactorMinMax.x,
			                             MutationParams.speedFactorMinMax.x),
			MutationParams.speedFactorMinMax.y, MutationParams.speedFactorMinMax.z);

		float turnSpeed = Mathf.Clamp(previousIteration.turnSpeed + Random.Range(
			                              -MutationParams.turnSpeedFactorMinMax.x,
			                              MutationParams.turnSpeedFactorMinMax.x),
			MutationParams.turnSpeedFactorMinMax.y, MutationParams.turnSpeedFactorMinMax.z);

		float newDampForward = Mathf.Clamp(previousIteration.dampForward + Random.Range(
			                                   -MutationParams.dampForwardFactorMinMax.x,
			                                   MutationParams.dampForwardFactorMinMax.x),
			MutationParams.dampForwardFactorMinMax.y, MutationParams.dampForwardFactorMinMax.z);

		float newDampRight = Mathf.Clamp(previousIteration.dampRight + Random.Range(
			                                 -MutationParams.dampRightFactorMinMax.x,
			                                 MutationParams.dampRightFactorMinMax.x),
			MutationParams.dampRightFactorMinMax.y, MutationParams.dampRightFactorMinMax.z);

		float newCriticalDistance = Mathf.Clamp(previousIteration.criticalDistance + Random.Range(
			                                        -MutationParams.criticalDistanceFactorMinMax.x,
			                                        MutationParams.criticalDistanceFactorMinMax.x),
			MutationParams.criticalDistanceFactorMinMax.y, MutationParams.criticalDistanceFactorMinMax.z);

		float fwdCriticalDamp = Mathf.Clamp(previousIteration.fwdCriticalDamp + Random.Range(
			                                    -MutationParams.fwdCriticalDampFactorMinMax.x,
			                                    MutationParams.fwdCriticalDampFactorMinMax.x),
			MutationParams.fwdCriticalDampFactorMinMax.y, MutationParams.fwdCriticalDampFactorMinMax.z);


		AICarParms newParam = new AICarParms(newResolution, newMaxAngle, newMaxRayDist, newDistanceFactor,
			newAngleFactor, newSpeed, turnSpeed, newDampForward, newDampRight, newCriticalDistance, fwdCriticalDamp);

		//parameters.Add(newParam);

		return newParam;
	}

	public static List<AICarParms> TryToAdd(List<AICarParms> origen, AICarParms newParam, int maxItems) {
		List<AICarParms> tempList = origen;
		//Debug.Log(maxItems + " " + tempList.Count);

		if (maxItems > tempList.Count) {
			tempList.Add(newParam);
			Debug.Log("AddUntil20");
			return tempList;
		}

		Debug.Log("Check if better");
		int indexOfWorst = 0;
		float timeOfWorst = 0;

		for (int i = 0; i < tempList.Count; i++) {
			if (tempList[i].timeToComplete > timeOfWorst) {
				indexOfWorst = i;
				timeOfWorst = tempList[i].timeToComplete;
			}
		}

		if (newParam.timeToComplete < timeOfWorst) {
			tempList.RemoveAt(indexOfWorst);
			tempList.Add(newParam);
			Debug.Log("was good");
		}
		else {
			Debug.Log("Sucked");
		}

		return tempList;
	}

	public static List<AICarParms> FilterToTop(List<AICarParms> origen, int numberOfItemsToKeep) {
		int indexOfWorst = 0;
		float timeOfWorst = 0;
		int elemsKept = 0;

		for (int i = origen.Count - 1; i > 0; i--) {
			if (origen[i].timeToComplete > timeOfWorst) {
				if (elemsKept >= numberOfItemsToKeep) {
					timeOfWorst = origen[indexOfWorst].timeToComplete;
					origen.RemoveAt(i);
				}
				else {
					indexOfWorst = i;
				}
			}
			else {
				elemsKept++;
			}
		}

		return origen;
	}
}

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