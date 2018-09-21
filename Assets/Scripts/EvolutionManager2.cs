using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionManager2 : MonoBehaviour {
	public EvolutionContainer evo;

	private float timeToComplete;

	private float[] startingTime;

	[SerializeField] private float timeToSuicide;
	[SerializeField] private Transform p1;
	[SerializeField] private Transform p2;


	public SimulationManager2 sm;
	public List<CarAI2> cars;

	public bool RandomizeInit;

	private int[] carsGoInstanceID;


	public void InitThings() {
		startingTime = new float[cars.Count];
		carsGoInstanceID = new int[cars.Count];

		for (int i = 0; i < startingTime.Length; i++) {
			startingTime[i] = Time.unscaledTime;
			carsGoInstanceID[i] = cars[i].gameObject.GetInstanceID();
		}

		//car.parameters = evo.baseParam.ToCarParams();
		for (int i = 0; i < cars.Count; i++) {
			if (RandomizeInit) {
				cars[i].parameters = evo.RandomizeParams();
			}
			else {
				if (evo.succesfulParams.Count > 0) {
					cars[i].parameters = evo.Mutate2(evo.baseParam.ToCarParams());
				}
				else {
					cars[i].parameters = evo.baseParam.ToCarParams();
				}
			}
		}

		for (int i = 0; i < cars.Count; i++) {
			cars[i].enabled = true;
		}
	}

	private void Update() {
		for (int i = 0; i < startingTime.Length; i++) {
			if (startingTime[i] + timeToSuicide < Time.unscaledTime) {
				startingTime[i] = Time.unscaledTime;
				cars[i].EndRaceAndMutate(evo.Mutate2(cars[i].parameters));
				sm.CarDied();
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Player")) {
			int carIndex = 0;

			for (int i = 0; i < cars.Count; i++) {
				if (other.gameObject.GetInstanceID() == carsGoInstanceID[i]) {
					carIndex = i;
					break;
				}
			}


			if (Vector3.Distance(other.transform.position, p1.transform.position) <
			    Vector3.Distance(other.transform.position, p2.transform.position)) {

				//cars[carIndex].parameters = evo.RandomizeParams();

				//car.parameters = evo.RandomizeParams();
				//car.LoadValues();
				//car.DieAndReset();

				cars[carIndex].parameters.completesTrack = false;


				cars[carIndex].EndRaceAndMutate(evo.Mutate2(cars[carIndex].parameters));
				sm.CarDied();
				//		smthHappened.Invoke(car.parameters);
				return;
			}

			timeToComplete = (Time.unscaledTime - startingTime[carIndex]) * Time.timeScale;
			if (timeToComplete < 1f) {
				cars[carIndex].parameters = evo.RandomizeParams();
				//car.LoadValues();
				//car.DieAndReset();
				cars[carIndex].EndRaceAndMutate(evo.Mutate2(cars[carIndex].parameters));
				cars[carIndex].parameters.completesTrack = false;
				sm.CarDied();
				//		smthHappened.Invoke(cars[carIndex].parameters);
				return;
			}

			cars[carIndex].parameters.timeToComplete = timeToComplete;
			startingTime[carIndex] = Time.unscaledTime;
			cars[carIndex].parameters.completesTrack = true;
			sm.CarReached(timeToComplete);
			//	smthHappened.Invoke(cars[carIndex].parameters);
			cars[carIndex].EndRaceAndMutate(evo.Mutate2(cars[carIndex].parameters));
		}
	}

	public void CarDied(int ID) {
		int carIndex = 0;

		for (int i = 0; i < cars.Count; i++) {
			if (ID == carsGoInstanceID[i]) {
				carIndex = i;
				break;
			}
		}

		startingTime[carIndex] = Time.unscaledTime;
		cars[carIndex].parameters.completesTrack = false;
		//	smthHappened.Invoke(car.parameters);
		sm.CarDied();
		cars[carIndex].EndRaceAndMutate(evo.Mutate2(cars[carIndex].parameters));
	}
}