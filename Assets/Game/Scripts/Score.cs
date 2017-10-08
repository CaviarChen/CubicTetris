using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Score : MonoBehaviour {

    public static int score = 0;
    public Text scoreText;

    private static ParticleSystem ps;

    // Use this for initialization
    void Start () {

	}

    public static void addScore(int s) {
        score += s;
        ps = GameObject.Find("Portal_Orb").GetComponent<ParticleSystem>();
        var main = ps.main;
        main.startSize = (float)(score) / 100.0f;
    }


	
	// Update is called once per frame
	void Update () {
        scoreText.text = "Score: " + score.ToString();

    }
}
