using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
    void OnDisable() {
        Time.timeScale = 1;
        Cursor.visible = false;
    }

    void OnEnable() {
        Time.timeScale = 0;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
