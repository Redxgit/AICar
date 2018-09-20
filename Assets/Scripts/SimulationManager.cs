using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class SimulationManager : MonoBehaviour {
	[SerializeField] private int numberOfSimulations;
	[SerializeField] private GameObject evolutionPrefab;

	[SerializeField] private CameraFollow cf;

	[SerializeField] public float targetTimeScale;

	[SerializeField] private Text bestTime;
	[SerializeField] private Text carsReaching;
	[SerializeField] private Text carsCrashed;
	[SerializeField] private Text carsSpawned;
	[SerializeField] private GraphController gc;
	[SerializeField] private EvolutionContainer cont;

	public bool ResetOnStart;

	private float bestTimef = Mathf.Infinity;
	private int carsRint;
	private int carsCint;
	private int carsSpawne;

	// Use this for initialization
	void Start() {

		if (ResetOnStart) {
			cont.ResetData();
		}
		
		for (int i = 0; i < numberOfSimulations; i++) {
			if (i == 0) {
				GameObject go = Instantiate(evolutionPrefab, Vector3.zero, Quaternion.identity, transform);
				EvolutionManager mn = go.GetComponentInChildren<EvolutionManager>();
				//mn.smthHappened += ProcessCarEnded;
				mn.sm = this;
				//cf.target = go.transform.GetChild(0);
				//cf.enabled = true;
			}
			else {
				Instantiate(evolutionPrefab, Vector3.zero, Quaternion.identity, transform);
			}

			carsSpawne++;
		}

		carsSpawned.text = carsSpawne.ToString();
		Time.timeScale = targetTimeScale;
	}

	public void CarDied() {
		carsCint++;
		carsCrashed.text = carsCint.ToString();

		carsSpawne++;
		carsSpawned.text = carsSpawne.ToString();
	}

	public void CarReached(float time) {
		carsRint++;
		carsReaching.text = carsRint.ToString();
		if (time < bestTimef) {
			bestTimef = time;
			bestTime.text = bestTimef.ToString(CultureInfo.InvariantCulture);
		}

		carsSpawne++;
		carsSpawned.text = carsSpawne.ToString();
		if (gc != null && gc.enabled)
			gc.AddTime(time);
	}

	private void ProcessCarEnded(AICarParms p) {
		Debug.Log("Invoked");
		if (p.completesTrack) {
			carsRint++;
			carsReaching.text = carsRint.ToString();
			if (p.timeToComplete < bestTimef) {
				bestTimef = p.timeToComplete;
				bestTime.text = bestTimef.ToString(CultureInfo.InvariantCulture);
			}
		}
		else {
			carsCint++;
			carsCrashed.text = carsCint.ToString();
		}

		carsSpawne++;
		carsSpawned.text = carsSpawne.ToString();
	}
}