using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionManager : MonoBehaviour {

	[SerializeField] private CarAI car;

	[SerializeField] private EvolutionContainer evo;

	private float timeToComplete;

	private float startingTime;

	[SerializeField] private float newTimeScale;

	[SerializeField] private float timeToSuicide;
	[SerializeField] private Transform p1;
	[SerializeField] private Transform p2;

	public SimulationManager sm;

	public bool RandomizeInit;

	//public Action<AICarParms> smthHappened = delegate {  };

	// Use this for initialization
	void Start () {
		startingTime = Time.unscaledTime;
		//car.parameters = evo.baseParam.ToCarParams();
		if (RandomizeInit) {
			car.parameters = evo.RandomizeParams();
		}
		else {
			if (evo.succesfulParams.Count > 0) {
				car.parameters = evo.Mutate2(evo.baseParam.ToCarParams());
			}
			else {
				car.parameters = evo.baseParam.ToCarParams();
			}
		}

		car.enabled = true;
//		car.DieAndReset();
	}

	private void Update() {
		if (startingTime + timeToSuicide < Time.unscaledTime) {
			startingTime = Time.unscaledTime;
			car.parameters = evo.RandomizeParams();
			car.DieAndReset();
			if (sm == null) {
				sm = FindObjectOfType<SimulationManager>();
			}
			sm.CarDied();
		}
	}
	// Update is called once per frame

	private void OnTriggerEnter2D(Collider2D other) {
		if (!(other.transform == car.transform)) return;
		if (other.CompareTag("Player")) {

			if (Vector3.Distance(other.transform.position, p1.transform.position) <
			    Vector3.Distance(other.transform.position, p2.transform.position)) {
				Debug.Log("Facing wrong");
				car.parameters = evo.RandomizeParams();
				
				car.DieAndReset();
				if (sm == null) {
					sm = FindObjectOfType<SimulationManager>();
				}
				sm.CarDied();
				car.parameters.completesTrack = false;
		//		smthHappened.Invoke(car.parameters);
				return;
			}
			
			timeToComplete = (Time.unscaledTime - startingTime) * Time.timeScale;
			if (timeToComplete < 1f) {
				car.parameters = evo.RandomizeParams();
				car.DieAndReset();
				car.parameters.completesTrack = false;
				if (sm == null) {
					sm = FindObjectOfType<SimulationManager>();
				}
				sm.CarDied();
		//		smthHappened.Invoke(car.parameters);
				return;
			}
			car.parameters.timeToComplete = timeToComplete;
			startingTime = Time.unscaledTime;
			car.parameters.completesTrack = true;
			if (sm == null) {
				sm = FindObjectOfType<SimulationManager>();
			}
			sm.CarReached(timeToComplete);
		//	smthHappened.Invoke(car.parameters);
			car.EndRaceAndMutate(evo.Mutate2(car.parameters));
		}
	}

	public void CarDied() {
		Debug.Log("Car crashed");
		startingTime = Time.unscaledTime;
		car.parameters.completesTrack = false;
	//	smthHappened.Invoke(car.parameters);
		if (sm == null) {
			sm = FindObjectOfType<SimulationManager>();
		}
		sm.CarDied();
		car.EndRaceAndMutate(evo.Mutate2(car.parameters));
	}

	[ContextMenu("ChangeTimeScale")]
	private void ChangeTimeScale() {
		Time.timeScale = newTimeScale;
	}
}
