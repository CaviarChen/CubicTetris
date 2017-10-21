using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Score : MonoBehaviour {

    public static int score;
    public GameObject scoreTextF;
    public GameObject scoreTextB;

    private static ParticleSystem ps;

    public static void addScore(int s) {
        score += s;
        // update the size of the particle system at the portal
        ps = GameObject.Find("Portal_Orb").GetComponent<ParticleSystem>();
        var main = ps.main;
        main.startSize = (float)(score) / 150.0f;
    }

     // Use this for initialization
    public static void init() {
        score = 0;
        addScore(0);
    }

	
	// Update is called once per frame
	void Update () {
        // update the score text
        scoreTextF.GetComponent<TextMesh>().text = "Score: " + score.ToString();
        scoreTextB.GetComponent<TextMesh>().text = "Score: " + score.ToString();
    }
}
