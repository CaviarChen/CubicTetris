using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideTrees : MonoBehaviour {

	public GameObject camera;

	// Use this for initialization
	void Start () {
		camera = GameObject.FindGameObjectWithTag ("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {
		if (camera.transform.position.z < 0) {
			GetComponent<MeshRenderer> ().enabled = true;
		} else {
			GetComponent<MeshRenderer> ().enabled = false;
		}
	}
}
