using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMovement : MonoBehaviour {

	
	public float speed = 0.2f;
    public Material cubes;

    private GameObject camerax;



	// Use this for initialization
	void Start () {
		camerax = GameObject.FindGameObjectWithTag ("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {

        print(this.gameObject.transform.position);
        cubes.SetVector ("_PointLightPosition", this.gameObject.transform.position);


        // moving the light based on the position of the camera
		if (camerax.transform.position.z < 0) {
			if (transform.rotation.x > 0.515) {
				transform.Rotate (new Vector3(-1 * speed, 0, 0));
			}
		} else {
			if (transform.rotation.x < 0.785) {
				transform.Rotate (new Vector3(speed, 0, 0));
			}
		}
	}
}
