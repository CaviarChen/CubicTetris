
//reference: https://www.youtube.com/watch?v=bVo0YLLO43s




using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
	private GameObject gameArea;
	private Main mainscript;
	protected Transform Camera;
	protected Transform CameraPivot;

	protected Vector3 cameraRotation;
	protected float _CameraDistance = 10f;

	public float MouseSpeed = 4f;
	public float OrbitDampening = 10f;

	public int speed = 100;
    public GameObject scoreTextF;
    public GameObject scoreTextB;
    public GameObject gameOverText;
    public GameObject gameOverScoreText;

    private int flipped = -1;
	//flipped = -1, A left movement, D right movement 
	//flipper = 1, A right movement, D left movement

	private float offsetspeed;
	private GameObject mainCamera;
	private GameObject floor;
	private Vector3 cameraC_center;
	private Vector3 target_center;
    private Vector3 gameOver_center;
	private Vector3 camera_start_position;
	private Vector3 camera_back_position;
	private Vector3 camera_reset_position;
	private Vector3 camera_gameover_position;
	private bool isGameOver = false;
	private int front;
	private Vector3 velocity;
    private Vector3 velocity2;
    private Vector3 targetPosition;

	private bool ismovingback = false;

	public int isFlipped(){
		return flipped;
	}

	void Start(){

		this.Camera = this.transform;
		this.CameraPivot = this.transform.parent;
		gameArea = GameObject.Find ("GameArea");
		mainscript = (Main)gameArea.GetComponent (typeof(Main));
		mainCamera = GameObject.Find ("Main Camera");
		floor = GameObject.FindGameObjectWithTag ("Floor");
		cameraC_center = mainCamera.GetComponent<SphereCollider> ().center;
		targetPosition = cameraC_center;
		camera_start_position = mainCamera.transform.position;
		camera_back_position = new Vector3 (camera_start_position.x,camera_start_position.y,(2*target_center.z -camera_start_position.z));
		camera_reset_position = camera_back_position;
		camera_gameover_position = new Vector3 (camera_start_position.x,40.0f,-40.0f);
		target_center = new Vector3(floor.GetComponent<BoxCollider> ().center.x,
			floor.GetComponent<BoxCollider> ().center.y,
			floor.GetComponent<BoxCollider> ().center.z);
		offsetspeed = speed;

        isGameOver = false;
		gameOverText.SetActive (false);
        gameOverScoreText.SetActive (false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        gameOver_center = target_center;



	}
		
	void Update(){

        if (isGameOver) {
            gameOver_center = Vector3.SmoothDamp(gameOver_center, gameOverText.transform.position + new Vector3(2, 0, -1), ref velocity2, 0.5f);
            transform.position = Vector3.SmoothDamp(transform.position, camera_gameover_position, ref velocity, 0.5f);
            transform.LookAt (gameOver_center);
            return;
        }

		if (mainscript.GameOver ()) {
			gameOverText.SetActive (true);
			isGameOver = true;
            scoreTextF.SetActive(false);
            scoreTextB.SetActive(false);
            gameOverScoreText.SetActive(true);
            gameOverScoreText.GetComponent<TextMesh>().text = scoreTextF.GetComponent<TextMesh>().text;
			return;
		}
        

		if (transform.position.z < 0) {
			front = 1;
		} else {
			front = -1;
		}


		if (!ismovingback && (Input.GetKeyDown (KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))) {
			ismovingback = true;
			flipped = -flipped;
			if (flipped == -1) {
				targetPosition = camera_start_position;
                scoreTextF.SetActive(true);
                scoreTextB.SetActive(false);

            } else {
				targetPosition = camera_back_position;
                scoreTextB.SetActive(true);
                scoreTextF.SetActive(false);
            }
		}


		if (transform.position == targetPosition) {
			ismovingback = false;

		}

		if (ismovingback) {
			Quaternion QT = Quaternion.Euler(0, 0, 0);
			this.CameraPivot.rotation = QT;
			transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity,0.1f);
			transform.LookAt (target_center);
			return;
		}

			


		if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0){
			cameraRotation.x += Input.GetAxis("Mouse X") * MouseSpeed;
			cameraRotation.y += Input.GetAxis("Mouse Y") * MouseSpeed;

			if (cameraRotation.y < -20f)
				cameraRotation.y = -20f;
			else if (cameraRotation.y > 60f)
				cameraRotation.y = 60f;
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