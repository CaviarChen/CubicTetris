using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMovement : MonoBehaviour {

	
	public float speed;

    private GameObject camerax;



	// Use this for initialization
	void Start () {
		camerax = GameObject.FindGameObjectWithTag ("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (Vector3.zero);
        // moving the light based on the position of the camera
		if (camerax.transform.position.z < 0) {
			if (transform.position.z > -99) {
				transform.RotateAround(Vector3.zero, Vector3.left, speed * Time.deltaTime);
			}
		} else {
			if (transform.position.z < 99) {
				transform.RotateAround(Vector3.zero, Vector3.right, speed * Time.deltaTime);
			}
		}
	}
}
