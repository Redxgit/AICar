using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class SimulationManager2 : MonoBehaviour {
    [SerializeField] private int numberOfSimulations;
    [SerializeField] private GameObject carPrefab;
    [SerializeField] private GameObject evolutionPrefab;

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
    
    [SerializeField] private Text textToToggle;

    private const string pause = "Pause";
    private const string continued = "Continue";

    private bool paused;

    // Use this for initialization

    public void ToggleStatusSimulation() {
        if (paused) {
            Time.timeScale = targetTimeScale;
            textToToggle.text = pause;
        }
        else {
            Time.timeScale = 0;
            textToToggle.text = continued;
        }

        paused = !paused;
    }


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
        Time.timeScale = targetTimeScale;
        paused = false;
        textToToggle.text = pause;

        gc.ResetData();
        Start();
    }

    void Start() {
        gos = new GameObject[numberOfSimulations+1];

        if (ResetOnStart) {
            cont.ResetData();
        }

        GameObject evoMngerGO = Instantiate(evolutionPrefab, transform);
        EvolutionManager2 evoMnger = evoMngerGO.GetComponent<EvolutionManager2>();
        evoMnger.evo = cont;
        evoMnger.sm = this;
        evoMnger.cars = new List<CarAI2>();
        gos[gos.Length - 1] = evoMngerGO;

        for (int i = 0; i < numberOfSimulations; i++) {
            GameObject go = Instantiate(carPrefab,  transform);
            CarAI2 car = go.GetComponentInChildren<CarAI2>();

            car.manager = evoMnger;
            evoMnger.cars.Add(car);
            //mn.smthHappened += ProcessCarEnded;
            
            //cf.target = go.transform.GetChild(0);
            //cf.enabled = true;
            gos[i] = go;


            carsSpawne++;
        }
        
        evoMnger.InitThings();

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
}