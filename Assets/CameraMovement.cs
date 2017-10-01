using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
	public GameObject centre;
	public int speed;
	private float dot;


	// Use this for initialization
	void Start () {
		gameObject.transform.position = new Vector3 (0,0,-10);	
	}
	
	// Update is called once per frame
	void Update () {
		float angle = calculateAngle ();
		if (Input.GetAxis ("Mouse X") < 0) {
			transform.RotateAround (Vector3.zero, Vector3.up, speed * Time.deltaTime);
		}else if (Input.GetAxis ("Mouse X") > 0) {
			transform.RotateAround (Vector3.zero, Vector3.down, speed * Time.deltaTime);
		}
		gameObject.transform.LookAt (centre.transform);
//		else if (Input.GetAxis ("Mouse Y") > 0) {
//			transform.RotateAround (Vector3.zero, Vector3.left, speed * Time.deltaTime);
//		}else if (Input.GetAxis ("Mouse Y") > 0) {
//			transform.RotateAround (Vector3.zero, Vector3.right, speed * Time.deltaTime);
//		}
	}

	float calculateAngle(){
		Vector3 v1 = gameObject.transform.position;
		//print (v1);
		Vector3 v2 = centre.transform.position;
		//print (v2);
		dot = Vector3.Dot (v1,v2);
		print (dot);
		dot = dot / (v1.magnitude * v2.magnitude);

		float angle = Mathf.Acos (dot) * 180 / Mathf.PI;
		print (angle);
		return angle;

	}
}
