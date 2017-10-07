using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMovement : MonoBehaviour {

	public GameObject camera;

	// Use this for initialization
	void Start () {
		camera = GameObject.FindGameObjectWithTag ("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {
		if (camera.transform.position.z < 0) {
			if (transform.rotation.x > 0.515) {
				transform.Rotate (new Vector3(-0.2f, 0, 0));
			}
		} else {
			if (transform.rotation.x < 0.785) {
				transform.Rotate (new Vector3(0.2f, 0, 0));
			}
		}
	}
}
