using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionManager : MonoBehaviour {

	[SerializeField] private CarAI car;

	[SerializeField] private EvolutionContainer evo;

	private float timeToComplete;

	private float startingTime;

	[SerializeField] private float newTimeScale;

	// Use this for initialization
	void Start () {
		startingTime = Time.unscaledTime;
		//car.parameters = evo.baseParam.ToCarParams();
		if (evo.succesfulParams.Count > 0) {
			car.parameters = evo.Mutate2(evo.baseParam.ToCarParams());
		}
		else {
			car.parameters = evo.baseParam.ToCarParams();
		}
		car.enabled = true;
//		car.DieAndReset();
	}
	
	// Update is called once per frame

	private void OnTriggerEnter2D(Collider2D other) {
		if (!(other.transform == car.transform)) return;
		if (other.CompareTag("Player")) {
			timeToComplete = (Time.unscaledTime - startingTime) * Time.timeScale;
			if (timeToComplete < 1f) {
				
				car.DieAndReset();
				return;
			}
			car.parameters.timeToComplete = timeToComplete;
			startingTime = Time.unscaledTime;
			car.parameters.completesTrack = true;
			car.EndRaceAndMutate(evo.Mutate2(car.parameters));
		}
	}

	public void CarDied() {
		startingTime = Time.unscaledTime;
		car.parameters.completesTrack = false;
		car.EndRaceAndMutate(evo.Mutate2(car.parameters));
	}

	[ContextMenu("ChangeTimeScale")]
	private void ChangeTimeScale() {
		Time.timeScale = newTimeScale;
	}
}
