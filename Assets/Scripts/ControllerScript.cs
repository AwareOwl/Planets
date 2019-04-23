using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour {

    float distance = 30;
    float rotation = 0;

    Vector3 lastMousePosition;

	// Use this for initialization
	void Awake () {
		
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = transform.position.normalized * distance;
        
        if (Input.GetMouseButton (0)) {
            Vector3 vector = new Vector3 (Input.mousePosition.x - lastMousePosition.x, Input.mousePosition.y - lastMousePosition.y, 0);
            vector *= -10f / Screen.height;
            transform.Translate (vector);
            distance = transform.position.magnitude;
        }
        if (Input.GetMouseButton (1)) {
            transform.RotateAround (Vector3.zero, Vector3.up, Input.mousePosition.x - lastMousePosition.x);
        }

        lastMousePosition = Input.mousePosition;

        distance -= Input.mouseScrollDelta.y;
        distance = Mathf.Clamp (distance, 4, 500);

    }
}
