using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraMovement : MonoBehaviour {

	public float speed = 0.1f;
	public int frequency = 10;

	private int freqCount = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (freqCount <= 0) {
			freqCount = frequency;
		} else if (freqCount > frequency * 3 / 4) {
			transform.Translate (Vector3.down * speed);
			freqCount--;
		} else if (freqCount > frequency / 2) {
			transform.Translate (Vector3.up * speed);
			freqCount--;
		} else if (freqCount > frequency / 4) {
			transform.Translate (Vector3.right * speed);
			freqCount--;
		} else {
			transform.Translate (Vector3.left * speed);
			freqCount--;
		}
	}
}
