using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SateliteScript : MonoBehaviour {

    public SateliteScript parentSatelite;
    public List<GameObject> satelites = new List<GameObject> ();

    public float size;
    int brk = 0;

    float garbageTimer = 0;

    private void OnMouseOver () {
        if (Input.GetMouseButtonDown (0)) {
            MainScript.instance.selectedObject = this;
        }
    }

    public void CustomGarbageCollector () {
        garbageTimer += Time.deltaTime;
        if (garbageTimer > 1) {
            List<GameObject> nonNullSatelites = new List<GameObject> ();
            foreach (GameObject obj in satelites) {
                if (obj != null) {
                    nonNullSatelites.Add (obj);
                }
            }
            satelites = nonNullSatelites;
            garbageTimer -= 1;
        }
    }

    public void FindRandomPosition (List <GameObject> avoidList, float min, float max) {
        if (brk > 1000) { // Nie znaleziono wolnej pozycji przy 1000 próbach
            return;
        }
        SetRandomPosition (min, max);
        foreach (GameObject obj in avoidList) {
            if (obj == null) {
                continue;
            }
            SateliteScript ss = obj.GetComponent<SateliteScript> ();
            if (ss != null) {
                Vector3 deltaPos = obj.transform.position - transform.position;
                float mag = deltaPos.magnitude;
                if (mag < size + ss.size + 0.1f) {
                    brk++;
                    FindRandomPosition (avoidList, min, max);
                    return;
                }
            }
        }
    }

    public void SetRandomPosition (float min, float max) {
        float distance = Random.Range (min, max);
        transform.localPosition = new Vector3 (distance, 0, 0);
        RotateAroundParent (Random.Range (0, 360f));
    }

    public void RotateAroundParent (float degrees) {
        Transform parent = transform.parent;
        Vector3 parentPosition;
        if (parent == null) {
            parentPosition = Vector3.zero;
        } else {
            parentPosition = parent.position;
        }
        transform.RotateAround (parentPosition, Vector3.up, degrees);
    }


    public float GetSpeed () {
        return Mathf.Sqrt (1 / transform.localPosition.magnitude);
    }

    public void RotateSatelite () {
        RotateAroundParent (SettingsScript.timeScale * GetSpeed () * Time.deltaTime);
    }

    static public GameObject CreateSphere () {
        GameObject clone;
        clone = GameObject.CreatePrimitive (PrimitiveType.Sphere);
        Rigidbody rb = clone.AddComponent<Rigidbody> ();
        rb.isKinematic = false;
        rb.useGravity = false;
        rb.maxDepenetrationVelocity = 0;
        rb.maxAngularVelocity = 0;
        return clone;
    }
}
