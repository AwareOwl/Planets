using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunScript : SateliteScript {
    
    
    float planetTimer = 10;

    private void Awake () {
        size = 3;
        transform.localScale = Vector3.one * size;
        transform.localPosition = new Vector3 (0, 0, 0);

        Renderer renderer = GetComponent<Renderer> ();
        Material material = renderer.material;
        material.color = Color.yellow;
        material.shader = MainScript.instance.sprite;
        

    }

    void Update () {
        planetTimer += Time.deltaTime * SettingsScript.timeScale;
        if (planetTimer > 0) {
            GameObject clone = CreateSphere ();
            clone.transform.parent = transform;
            clone.AddComponent<PlanetScript> ();
            satelites.Add (clone);
            planetTimer -= SettingsScript.spawnRate;
        }

        CustomGarbageCollector ();

        MainScript.instance.planetCountText.text = "Planet count: " + satelites.Count.ToString();
    }
}
