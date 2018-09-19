using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour {
	[SerializeField] private int numberOfSimulations;
	[SerializeField] private GameObject evolutionPrefab;

	[SerializeField] private float simulationSize;

	[SerializeField] private CameraFollow cf;

	// Use this for initialization
	void Start() {
		for (int i = 0; i < numberOfSimulations; i++) {
			if (i == 0) {
				GameObject go = Instantiate(evolutionPrefab, Vector3.zero, Quaternion.identity, transform);
				cf.target = go.transform.GetChild(0);
				cf.enabled = true;
			}
			else {
				Instantiate(evolutionPrefab, Vector3.zero, Quaternion.identity, transform);
			}
		}
	}
}