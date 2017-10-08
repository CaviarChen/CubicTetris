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
		if (camerax.transform.position.z < 0) {
			GetComponent<MeshRenderer> ().enabled = true;
		} else {
			GetComponent<MeshRenderer> ().enabled = false;
		}
	}
}
