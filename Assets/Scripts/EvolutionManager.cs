﻿using System;
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
    void Start() {
        startingTime = Time.time;
        //car.parameters = evo.baseParam.ToCarParams();
        if (RandomizeInit) {
            car.parameters = evo.RandomizeParams();
        } else {
            if (evo.succesfulParams.Count > 0) {
                car.parameters = evo.Mutate2(evo.baseParam.ToCarParams());
            } else {
                car.parameters = evo.baseParam.ToCarParams();
            }
        }

        car.enabled = true;
//		car.DieAndReset();
    }

    private void Update() {
        if (startingTime + timeToSuicide < Time.time) {
            startingTime = Time.time;
            //car.parameters = evo.RandomizeParams();
            //car.LoadValues();
            //car.DieAndReset();
            car.EndRaceAndMutate(evo.Mutate2(car.parameters));
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
                //car.LoadValues();
                //car.DieAndReset();
                car.EndRaceAndMutate(evo.Mutate2(car.parameters));
                sm.CarDied();
                car.parameters.completesTrack = false;
                //		smthHappened.Invoke(car.parameters);
                return;
            }

            timeToComplete = (Time.time - startingTime) * Time.timeScale;
            if (timeToComplete < 1f) {
                car.parameters = evo.RandomizeParams();
                //car.LoadValues();
                //car.DieAndReset();
                car.EndRaceAndMutate(evo.Mutate2(car.parameters));
                car.parameters.completesTrack = false;
                sm.CarDied();
                //		smthHappened.Invoke(car.parameters);
                return;
            }

            car.parameters.timeToComplete = timeToComplete;
            startingTime = Time.time;
            car.parameters.completesTrack = true;
            sm.CarReached(timeToComplete);
            //	smthHappened.Invoke(car.parameters);
            car.EndRaceAndMutate(evo.Mutate2(car.parameters));
        }
    }

    public void CarDied() {
        Debug.Log("Car crashed");
        startingTime = Time.time;
        car.parameters.completesTrack = false;
        //	smthHappened.Invoke(car.parameters);
        sm.CarDied();
        car.EndRaceAndMutate(evo.Mutate2(car.parameters));
    }

    [ContextMenu("ChangeTimeScale")]
    private void ChangeTimeScale() { Time.timeScale = newTimeScale; }
}