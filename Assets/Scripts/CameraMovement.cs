using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
	//public GameObject centre;
	public int speed;
	//private float dot;
	private Vector3 centrePoint;

	private bool flipped = false;
	//flipped = false, A left movement, D right movement 
	//flipper = true, A right movement, D left movement

	public bool isFlipped(){
		return flipped;
	}

	// Use this for initialization
	void Start () {
		centrePoint = new Vector3 (4.5f,-0.5f,0.0f);
		//gameObject.transform.position = new Vector3 (0,0,-10);	
	}
	
	// Update is called once per frame
	void Update () {
		float angle = calculateAngle ();
		//print (angle);
		if (angle < 0) {
			if (Input.GetAxis ("Mouse X") < 0 && angle > -135 && flipped == false) {
				transform.RotateAround (centrePoint, Vector3.up, speed * Time.deltaTime);
			} else if (Input.GetAxis ("Mouse X") > 0 && angle < -45 && flipped == false) {
				transform.RotateAround (centrePoint, Vector3.down, speed * Time.deltaTime);
			}
			if (angle < -100) {
				transform.RotateAround (centrePoint, Vector3.down, 0.5f*speed * Time.deltaTime);
			}else if (angle > -80) {
				transform.RotateAround (centrePoint, Vector3.up, 0.5f*speed * Time.deltaTime);
			}
		} else {
			if (Input.GetAxis ("Mouse X") < 0 && angle > 45 && flipped == true) {
				transform.RotateAround (centrePoint, Vector3.up, speed * Time.deltaTime);
			}
			else if (Input.GetAxis ("Mouse X") > 0 && angle < 135 && flipped == true) {
				transform.RotateAround (centrePoint, Vector3.down, speed * Time.deltaTime);
			}
			if (angle < 80) {
				transform.RotateAround (centrePoint, Vector3.down, 0.5f * speed * Time.deltaTime);
			} else if (angle > 1) {
				transform.RotateAround (centrePoint, Vector3.up, 0.5f*speed * Time.deltaTime);
			}
		}












		if (Input.GetKeyDown (KeyCode.Z)) {
			transform.RotateAround (centrePoint, Vector3.up, 180.0f);
			if (flipped) {
				flipped = false;
			} else {
				flipped = true;
			}
		}
		//gameObject.transform.LookAt (centre.transform);
//		else if (Input.GetAxis ("Mouse Y") > 0) {
//			transform.RotateAround (Vector3.zero, Vector3.left, speed * Time.deltaTime);
//		}else if (Input.GetAxis ("Mouse Y") > 0) {
//			transform.RotateAround (Vector3.zero, Vector3.right, speed * Time.deltaTime);
//		}
	}



	float calculateAngle(){
		float dot;
		Vector3 v1 = gameObject.transform.position;
		//print ("camera position is at "+v1);
		Vector3 v2 = centrePoint;
		//print ("centre position is at "+v2);
		dot = Vector3.Dot (v1,v2);
		//print ("dot between camera and centre is "+dot);
		dot = dot / (v1.magnitude * v2.magnitude);

		float angle = Mathf.Acos (dot) * 180 / Mathf.PI;

		if (v1.z > 0) {
			return angle;
		} else {
			return -angle;
		}


	}
}
