using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SE : MonoBehaviour {

    public AudioClip[] audioClips;
    // 0: blockDropped
    // 1: cubeCanceled
    // 2: moveCanceled
    // 3: madeMove
    // 4: gameOver

    private static SE SEInstance;

    public static void Play(int id) {
        AudioSource audio = SEInstance.GetComponent<AudioSource>();
        audio.clip = SEInstance.audioClips[id];
        audio.Play();
    }


	// Use this for initialization
	void Start () {
        SEInstance = this;
	}

}
