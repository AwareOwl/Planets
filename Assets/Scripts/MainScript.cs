using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScript : MonoBehaviour {

    public Text timeText;
    public Text planetCountText;
    public Text selectedObjectSizeText;
    public Text selectedObjectCountText;

    public InputField timeScaleInput;
    public InputField spawnRateInput;
    public InputField minDistFromSunInput;
    public InputField maxDistFromSunInput;
    public InputField minPlanetScale;
    public InputField maxPlanetScale;

    [HideInInspector]
    public SateliteScript selectedObject;

    [HideInInspector]
    public Shader sprite;

    public MoonType [] moonTypes;

    public AudioClip placeholderAudio;

    static public MainScript instance;

	// Use this for initialization
	void Awake () {
        sprite = Shader.Find ("Sprites/Default");
        instance = this;
        SateliteScript.CreateSphere ().AddComponent<SunScript> ();
    }

    public void SetTimeScale () {
        float value;
        if (Single.TryParse (timeScaleInput.text, out value)) {
            SettingsScript.timeScale = value;
        }
    }

    public void SetSpawnRate () {
        float value;
        if (Single.TryParse (spawnRateInput.text, out value)) {
            SettingsScript.spawnRate = value;
        }
    }

    public void SetMinDistFromSun () {
        float value;
        if (Single.TryParse (minDistFromSunInput.text, out value)) {
            SettingsScript.minDistFromSun = value;
        }
    }

    public void SetMaxDistFromSun () {
        float value;
        if (Single.TryParse (maxDistFromSunInput.text, out value)) {
            SettingsScript.maxDistFromSun = value;
        }
    }

    public void SetMinPlanetScale () {
        float value;
        if (Single.TryParse (minPlanetScale.text, out value)) {
            SettingsScript.minPlanetScale = value;
        }
    }

    public void SetMaxPlanetScale () {
        float value;
        if (Single.TryParse (maxPlanetScale.text, out value)) {
            SettingsScript.maxPlanetScale = value;
        }
    }

    float timer;

    private void Update () {
        timer += Time.deltaTime * SettingsScript.timeScale;
        timeText.text = "Time: " + timer.ToString();
        if (selectedObject != null) {
            selectedObjectSizeText.text = "Size of clicked object: " + selectedObject.size;
            selectedObjectCountText.text = "Satelites of clicked object: " + selectedObject.satelites.Count;
        }
    }
}
