﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General : MonoBehaviour {

    public struct Block {
	    public int[,,] block;
	    public int size;
    }

    public static float cubeSize = 1.05f;
    // width is 2
    public static int length = 6;
    public static int height = 8;
    public static float timeForEachMove = 1.0f;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
