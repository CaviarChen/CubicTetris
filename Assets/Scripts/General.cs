using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General : MonoBehaviour {

    public struct Block {
	    public int[,,] block;
	    public int size;
    }

    public static float cubeSize = 1.05f;
    // width is 2
    public static int length = 8;
    public static int height = 10;
    public static float timeForEachDrop = 1.0f;
    public static float timeForEachMoveAni = 0.2f;
    //public static float timeForEachRotateAni = 0.1f;


    // takes x in [0,1] and return y in [0,1]
    // (-cos(x*pi)+1) / 2
    public static float rubberBandFunction(float x) {
        return (-Mathf.Cos(x * Mathf.PI) + 1) / 2.0f;
    }

    public static Block[] generateBlockTemplate() {
           // --------------------
        General.Block[] blocks = new General.Block[2];
        blocks[0].block = new int[2, 4, 4] {
        {
            {0, 0, 0, 0},
            {0, 1, 0, 0},
            {0, 0, 0, 0},
            {0, 0, 0, 0},

        },
        {
            {0, 0, 0, 0},
            {2, 3, 4, 0},
            {0, 5, 0, 0},
            {0, 0, 0, 0}
        }
        };
        // size decide the center of the block (for rotating)
        blocks[0].size = 3;

        blocks[1].block = new int[2, 4, 4] {
        {
            {1, 2, 0, 0},
            {3, 4, 0, 0},
            {0, 0, 0, 0},
            {0, 0, 0, 0},
        },
        {
            {5, 6, 0, 0},
            {7, 8, 0, 0},
            {0, 0, 0, 0},
            {0, 0, 0, 0},

        }
        };

        blocks[1].size = 2;

        // --------------------

        return blocks;
    }



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
