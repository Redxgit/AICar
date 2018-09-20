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

    private GameObject[] gos;

    // Use this for initialization


    public void ResetSimulation() {
        for (int i = 0; i < gos.Length; i++) {
            Destroy(gos[i]);
        }

        carsCint = 0;
        carsSpawne = 0;
        carsRint = 0;
        bestTimef = Mathf.Infinity;
        
        carsSpawned.text = carsSpawne.ToString();
        carsCrashed.text = carsCint.ToString();
        carsReaching.text = carsRint.ToString();
        bestTime.text = "N/A";

        gc.ResetData();
        Start();
    }

    void Start() {
        gos = new GameObject[numberOfSimulations];

        if (ResetOnStart) {
            cont.ResetData();
        }

        for (int i = 0; i < numberOfSimulations; i++) {
            GameObject go = Instantiate(evolutionPrefab, Vector3.zero, Quaternion.identity, transform);
            EvolutionManager mn = go.GetComponentInChildren<EvolutionManager>();
            //mn.smthHappened += ProcessCarEnded;
            mn.sm = this;
            //cf.target = go.transform.GetChild(0);
            //cf.enabled = true;
            gos[i] = go;


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
        } else {
            carsCint++;
            carsCrashed.text = carsCint.ToString();
        }

        carsSpawne++;
        carsSpawned.text = carsSpawne.ToString();
    }
}