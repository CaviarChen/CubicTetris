using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideTrees : MonoBehaviour {

	private GameObject camerax;

	// Use this for initialization
	void Start () {
		camerax = GameObject.FindGameObjectWithTag ("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {
        // hide some gameobject when the camera at the back
        // prevent those things blocking the camera
		if (camerax.transform.position.z < 0) {
			GetComponent<MeshRenderer> ().enabled = true;
		} else {
			GetComponent<MeshRenderer> ().enabled = false;
		}
	}
}
