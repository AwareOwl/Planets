using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetScript : SateliteScript {

    public bool pendingDestroy;
    Rigidbody rigidbody;
    Material material;

    private void Awake () {

        size = Random.Range (SettingsScript.minPlanetScale, SettingsScript.maxPlanetScale);
        transform.localScale = Vector3.one * size;

        Transform parent = transform.parent;
        if (parent != null) {
            parentSatelite = parent.GetComponent<SateliteScript> ();
        }

        FindRandomPosition (parentSatelite.satelites, SettingsScript.minDistFromSun, SettingsScript.maxDistFromSun);

        rigidbody = GetComponent<Rigidbody> ();
        material = GetComponent<Renderer> ().material;
        material.shader = MainScript.instance.sprite;

        int numberOfMoons = Random.Range (0, 10);
        for (int x = 0; x < numberOfMoons; x++) {
            GameObject clone = CreateSphere ();
            clone.transform.parent = transform;
            MoonScript ms = clone.AddComponent<MoonScript> ();
            satelites.Add (clone);
        }

        SetNextColor ();
    }

    float colorTimer = 10;
    Color prevColor;
    Color nextColor;
	
	void Update () {
        RotateSatelite ();
        rigidbody.velocity = Vector3.zero;

        colorTimer += Time.deltaTime;
        if (colorTimer >= 10) {
            colorTimer -= 10;
            prevColor = nextColor;
            SetNextColor ();
        }
        material.color = Color.Lerp (prevColor, nextColor, colorTimer /= 10);
        SetNextColor ();
        material.color = nextColor;


        CustomGarbageCollector ();
    }

    public void SetNextColor () {
        float distance = 1.5f * GetSpeed ();
        nextColor = new Color (distance, distance, distance, 1);
    }

    private void OnCollisionEnter (Collision collision) {
        if (pendingDestroy) {
            return;
        }
        foreach (ContactPoint contact in collision.contacts) {
            GameObject otherObj = contact.otherCollider.gameObject;
            PlanetScript ps = contact.otherCollider.GetComponent<PlanetScript> ();
            if (ps == null) {
                continue;
            }
            ps.pendingDestroy = true;
            float oSize = ps.size;
            size = Mathf.Pow (size * size + oSize * oSize, 1f/3);
            transform.localScale = Vector3.one * size;
            Vector3 newPos = transform.position + ps.transform.position;
            newPos /= 2;
            transform.position = newPos;

            Destroy (gameObject);

        }
    }

    public void TransferMoons (PlanetScript source) {
        foreach (GameObject obj in source.satelites) {
            satelites.Add (obj);
            obj.transform.parent = transform;
        }
    }
}
