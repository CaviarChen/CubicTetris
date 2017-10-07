using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
	//public GameObject centre;
	public int speed = 100;
	//private float dot;
//	private Vector3 centrePoint;

	private int flipped = -1;
	//flipped = -1, A left movement, D right movement 
	//flipper = 1, A right movement, D left movement

	private float offsetspeed;
	private GameObject mainCamera;
	private GameObject floor;
	private Vector3 cameraC_center;
	private Vector3 target_center;
	Vector3 lastMouseCoordinate = Vector3.zero;
	private double left_bound = 0.3;
	private double right_bound = -0.3;
	private int front;
	private float offset = 5.0f;

	public int isFlipped(){
		return flipped;
	}

	// Use this for initialization
//	void Start () {
////		centrePoint = new Vector3 (4.5f,-0.5f,0.0f);
//	}

	void Start(){
		mainCamera = GameObject.Find ("Main Camera");
		floor = GameObject.FindGameObjectWithTag ("Floor");
		cameraC_center = mainCamera.GetComponent<SphereCollider> ().center;
		target_center = floor.GetComponent<BoxCollider> ().center;
		offsetspeed = speed;

//		print (cameraC_center);
//		print (target_center);

		//print (mainCamera.transform.renderer.bounds.center);

	}
	// Update is called once per frame
	void Update(){
		print (transform.eulerAngles.y);
		if (transform.position.z < 0) {
			front = 1;
		} else {
			front = -1;
		}

		if (Input.GetKeyDown (KeyCode.Z)) {
			float z = -transform.position.z;
			transform.position = new Vector3 (transform.position.x,transform.position.y,z);
			transform.LookAt (target_center);
			flipped = -flipped;
		}


		int direction = -1;
		bool change = false;
		float left = 0.0f,right=0.0f,up=0.0f,down=0.0f;
		if (Input.GetAxis ("Mouse X") < 0) {
			left = Mathf.Abs(Input.GetAxis ("Mouse X"));
			change = true;

		}
		if (Input.GetAxis ("Mouse X") > 0) {
			right = Mathf.Abs(Input.GetAxis ("Mouse X"));
			change = true;

		}

		if (Input.GetAxis ("Mouse Y") < 0) {
			down = Mathf.Abs (Input.GetAxis ("Mouse Y"));
			change = true;

		}

		if (Input.GetAxis ("Mouse Y") > 0 ) {
			up = Mathf.Abs (Input.GetAxis ("Mouse Y"));
			change = true;

		}
		if (!change) {
			return;
		}	
		direction = moveDirection (left, right, up, down);

		if (direction == 0 && canMove(transform.eulerAngles.y))   
		{
			transform.RotateAround (target_center, new Vector3(0, target_center.y, 0), speed * Time.deltaTime);
		
		} 
		else if (direction == 1 && canMove(transform.eulerAngles.y))
		{
			transform.RotateAround (target_center, new Vector3(0, -target_center.y, 0), speed * Time.deltaTime);
		
		} else if (direction == 2 && transform.position.y < 10) {
			transform.RotateAround (target_center, new Vector3 (front * target_center.x, 0, 0), speed * Time.deltaTime);	
				
		
		} else if (direction == 3 && transform.position.y > 3) {
			transform.RotateAround (target_center, new Vector3 (-front * target_center.x, 0, 0), speed * Time.deltaTime);
		
		} else {
//			print ("Error occur");
		}

		//offset

		if (transform.eulerAngles.y > 40.0f && transform.eulerAngles.y < 60.0f) {
			transform.RotateAround (target_center, new Vector3 (0, -target_center.y, 0), offsetspeed * Time.deltaTime);
		} 
		else if (transform.eulerAngles.y > 300.0f && transform.eulerAngles.y < 320.0f) {
			transform.RotateAround (target_center, new Vector3 (0, target_center.y, 0), offsetspeed * Time.deltaTime);
		} 
		else if (transform.eulerAngles.y > 120.0f && transform.eulerAngles.y < 135.0f) {
			transform.RotateAround (target_center, new Vector3 (0, target_center.y, 0), offsetspeed * Time.deltaTime);
		} 
		else if (transform.eulerAngles.y > 225.0f && transform.eulerAngles.y < 240.0f) {
			transform.RotateAround (target_center, new Vector3 (0, -target_center.y, 0), offsetspeed * Time.deltaTime);
		}



			
		transform.LookAt(target_center);

	}
		

	private bool canMove(float i){
		if (checkValidRange (transform.eulerAngles.y, 0.0f, 45.0f)
		   || checkValidRange (transform.eulerAngles.y, 315.0f, 360.0f)
		   || checkValidRange (transform.eulerAngles.y, 135.0f, 225.0f)) {
			return true;
		}
		return false;
	}

	private bool checkValidRange(float i,float lowbound,float upperbound){
		if (i >= lowbound && i <= upperbound) {
//			print ("true");
			return true;
		}
//		print ("false");
		return false;
		
	}



	int moveDirection(float left,float right,float up,float down){
		float direction = Mathf.Max (left, right, up, down);
		if (direction == left) {
			return 0;
		} else if (direction == right) {
			return 1;
		} else if (direction == up) {
			return 2;
		} else if (direction == down) {
			return 3;
		} else {
			return 100;
		}
	}



//	void Update () {

		//transform.RotateAround(Vector3.zero, Vector3.left, 20 * Time.deltaTime);


//		float angle = calculateAngle ();
//		float updownangle = calculateUpDownAngle ();
//		//print (angle);
//		print(updownangle);
//		if (angle < 0) {
//			if (Input.GetAxis ("Mouse X") < 0 && angle > -135 && flipped == false) {
//				transform.RotateAround (centrePoint, Vector3.up, speed * Time.deltaTime);
//			} else if (Input.GetAxis ("Mouse X") > 0 && angle < -45 && flipped == false) {
//				transform.RotateAround (centrePoint, Vector3.down, speed * Time.deltaTime);
//			}
//			if (angle < -120) {
//				transform.RotateAround (centrePoint, Vector3.down, 0.5f*speed * Time.deltaTime);
//			}else if (angle > -60) {
//				transform.RotateAround (centrePoint, Vector3.up, 0.5f*speed * Time.deltaTime);
//			}
//		} else {
//			if (Input.GetAxis ("Mouse X") < 0 && angle > 45 && flipped == true) {
//				transform.RotateAround (centrePoint, Vector3.up, speed * Time.deltaTime);
//			}
//			else if (Input.GetAxis ("Mouse X") > 0 && angle < 135 && flipped == true) {
//				transform.RotateAround (centrePoint, Vector3.down, speed * Time.deltaTime);
//			}
//			if (angle < 60) {
//				transform.RotateAround (centrePoint, Vector3.down, 0.5f * speed * Time.deltaTime);
//			} else if (angle > 120) {
//				transform.RotateAround (centrePoint, Vector3.up, 0.5f*speed * Time.deltaTime);
//			}
//		}
//
//		if (Input.GetAxis ("Mouse Y") > 0 ) {
//			transform.RotateAround (centrePoint,Vector3.left, speed * Time.deltaTime);
//		}
//		else if (Input.GetAxis ("Mouse Y") < 0) {
//			transform.RotateAround (centrePoint,Vector3.right, speed * Time.deltaTime);
//		}
//
//
//		if (Input.GetKeyDown (KeyCode.Z)) {
//			transform.RotateAround (centrePoint, Vector3.up, 180.0f);
//			if (flipped) {
//				flipped = false;
//			} else {
//				flipped = true;
//			}
//		}
//		gameObject.transform.LookAt (centrePoint);

//	}

//	float calculateUpDownAngle(){
//		float dot;
//		Vector3 v1 = new Vector3(centrePoint.x,gameObject.transform.position.y,gameObject.transform.position.z);
////		print ("camera position is at "+v1);
//		Vector3 v2 = centrePoint;
////		print ("centre position is at "+v2);
//		dot = Vector3.Dot (v1,v2);
////		print ("dot between camera and centre is "+dot);
//		dot = dot / (v1.magnitude * v2.magnitude);
//
//		float angle = Mathf.Acos (dot) * 180 / Mathf.PI;
//
//		if (v1.z > 0) {
//			return angle;
//		} else {
//			return -angle;
//		}
//	}
//
//	float calculateAngle(){
//		float dot;
//		Vector3 v1 = new Vector3(gameObject.transform.position.x,centrePoint.y,gameObject.transform.position.z);
//		//print ("camera position is at "+v1);
//		Vector3 v2 = centrePoint;
//		//print ("centre position is at "+v2);
//		dot = Vector3.Dot (v1,v2);
//		//print ("dot between camera and centre is "+dot);
//		dot = dot / (v1.magnitude * v2.magnitude);
//
//		float angle = Mathf.Acos (dot) * 180 / Mathf.PI;
//
//		if (v1.z > 0) {
//			return angle;
//		} else {
//			return -angle;
//		}
//			
//	}
}
