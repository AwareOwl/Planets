using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonScript : SateliteScript {

    MoonType type;
    AudioSource audioSource;
    Material material;

    private void Awake () {
        Transform parent = transform.parent;
        if (parent != null) {
            parentSatelite = parent.GetComponent<SateliteScript> ();
        }

        MoonType [] moonTypes = MainScript.instance.moonTypes;
        type = moonTypes [Random.Range (0, moonTypes.Length)];

        size = type.size;
        transform.localScale = Vector3.one * size / parentSatelite.size;

        FindRandomPosition (parentSatelite.satelites, parentSatelite.size + size + 0.3f, parentSatelite.size + size + 1f);

        material = GetComponent<Renderer> ().material;
        material.color = type.color;
        material.shader = MainScript.instance.sprite;

        audioSource = gameObject.AddComponent<AudioSource> ();
        audioSource.clip = MainScript.instance.placeholderAudio;
        audioSource.volume = 0.1f;
    }

    // Use this for initialization
    void Start () {

    }

    bool pendingToDestroy = false;
    float destroyTimer = 1;

    // Update is called once per frame
    void Update () {
        RotateSatelite ();
        if (pendingToDestroy) {
            destroyTimer -= Time.deltaTime * 1.5f;
            Color col = material.color;
            col.a = destroyTimer;
            material.color = col;
            if (destroyTimer <= 0) {
                Destroy (gameObject);
            }
        }
    }

    private void OnCollisionEnter (Collision collision) {
        if (gameObject != null) {
            if (pendingToDestroy) {
                return;
            }
            SateliteScript ss = transform.parent.GetComponent<SateliteScript> ();
            ss.satelites.Remove (gameObject);
            audioSource.Play ();
            pendingToDestroy = true;
        }
    }
    
}
