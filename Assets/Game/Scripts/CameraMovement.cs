using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
	private GameObject gameArea;
	private Main mainscript;
	private Transform Camera;
	private Transform CameraPivot;

	private Vector3 cameraRotation;
	private float _CameraDistance = 10f;

	public float MouseSpeed = 4f;
	public float OrbitDampening = 10f;

	public int speed = 100;
    public GameObject scoreTextF;
    public GameObject scoreTextB;
    public GameObject gameOverText;
    public GameObject gameOverScoreText;



	private GameObject mainCamera;
	private GameObject floor;

	private Vector3 target_center;
    private Vector3 gameOver_center;
	private Vector3 camera_start_position;
	private Vector3 camera_back_position;
	private Vector3 camera_gameover_position;
	private bool isGameOver = false;

	private int front = 1;
	//front = 1, A left movement, D right movement 
	//front = -1, A right movement, D left movement

	private Vector3 velocity;
    private Vector3 velocity2;
    private Vector3 targetPosition;

	private bool ismovingback = false;


	public int isFlipped(){
		return -front;
	}

	void Start(){
		this.Camera = this.transform;
		this.CameraPivot = this.transform.parent;
		gameArea = GameObject.Find ("GameArea");
		mainscript = (Main)gameArea.GetComponent (typeof(Main));
		mainCamera = GameObject.Find ("Main Camera");
		floor = GameObject.FindGameObjectWithTag ("Floor");

		//camera start position 
		targetPosition = mainCamera.GetComponent<SphereCollider> ().center;

		//front and back centre position 
		camera_start_position = mainCamera.transform.position;
		camera_back_position = new Vector3 (camera_start_position.x,camera_start_position.y,(2*target_center.z -camera_start_position.z));

		//game over position
		camera_gameover_position = new Vector3 (camera_start_position.x,40.0f,-40.0f);

		//camera look at position
		target_center = new Vector3(floor.GetComponent<BoxCollider> ().center.x,
			3.5f,
			floor.GetComponent<BoxCollider> ().center.z);
		
		//gameOver
        isGameOver = false;
		gameOverText.SetActive (false);
        gameOverScoreText.SetActive (false);
		gameOver_center = target_center;

		//mouser invisible
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        



	}
		
	void Update(){

		//set game over position 
        if (isGameOver) {
            gameOver_center = Vector3.SmoothDamp(gameOver_center, gameOverText.transform.position + new Vector3(2, 0, -1), ref velocity2, 0.5f);
            transform.position = Vector3.SmoothDamp(transform.position, camera_gameover_position, ref velocity, 0.5f);
            transform.LookAt (gameOver_center);
            return;
        }

		//show 'Game Over' Text 
		if (mainscript.GameOver ()) {
			gameOverText.SetActive (true);
			isGameOver = true;
            scoreTextF.SetActive(false);
            scoreTextB.SetActive(false);
            gameOverScoreText.SetActive(true);
            gameOverScoreText.GetComponent<TextMesh>().text = scoreTextF.GetComponent<TextMesh>().text;
			return;
		}
        
		//check if the user is looking from the front view
		if (transform.position.z < 0) {
			front = 1;
		} else {
			front = -1;
		}

		//start to turn back
		if (!ismovingback && (Input.GetKeyDown (KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))) {
			ismovingback = true;
			front = -front;
			if (front == 1) {
				targetPosition = camera_start_position;
                scoreTextF.SetActive(true);
                scoreTextB.SetActive(false);

            } else {
				targetPosition = camera_back_position;
                scoreTextB.SetActive(true);
                scoreTextF.SetActive(false);
            }
		}

		//complete turn-around
		if (transform.position == targetPosition) {
			ismovingback = false;

		}

		//is turning back
		if (ismovingback) {
			Quaternion QT = Quaternion.Euler(0, 0, 0);
			this.CameraPivot.rotation = QT;
			transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity,0.1f);
			transform.LookAt (target_center);
			return;
		}

			
		//get mouse input and move camera
		if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0){
			cameraRotation.x += Input.GetAxis("Mouse X") * MouseSpeed;
			cameraRotation.y += Input.GetAxis("Mouse Y") * MouseSpeed;

			//set boundarys
			if (cameraRotation.y < -15f)
				cameraRotation.y = -15f;
			else if (cameraRotation.y > 50f)
				cameraRotation.y = 50f;
			if (cameraRotation.x < -40f)
				cameraRotation.x = -40f;
			else if (cameraRotation.x > 40f)
				cameraRotation.x = 40f;

			Quaternion QT = Quaternion.Euler((front)*cameraRotation.y, -cameraRotation.x, 0);
			this.CameraPivot.rotation = Quaternion.Lerp(this.CameraPivot.rotation, QT, Time.deltaTime * OrbitDampening);
		}
			
		transform.LookAt(target_center);

	}
}