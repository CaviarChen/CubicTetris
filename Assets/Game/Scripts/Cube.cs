using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

    private GameObject dlight;
    private MeshRenderer rendererx;

	// Use this for initialization
	void Start () {
        dlight = GameObject.Find("Directional Light");
        rendererx = this.gameObject.GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        // update light position
        rendererx.material.SetVector ("_PointLightPosition", dlight.transform.position);
		
	}
}
