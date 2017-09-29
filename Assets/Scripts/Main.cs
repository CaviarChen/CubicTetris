using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public GameObject blockPrefab;


    static General.Block[] blocks;


    private GameObject currentBlockObject;
    private BlockBase currentScript;

	// Use this for initialization
	void Start () {

        // --------------------
        blocks = new General.Block[2];
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


        currentBlockObject = createBlock(this.gameObject, blocks[0]);
        currentScript = (BlockBase) currentBlockObject.GetComponent(typeof(BlockBase));
        	
	}


    GameObject createBlock(GameObject playArea, General.Block block) {
        GameObject blockObject = Instantiate(blockPrefab);
        blockObject.transform.SetParent(playArea.transform);
        BlockBase script = (BlockBase) blockObject.GetComponent(typeof(BlockBase));
        script.block = block;
        script.createCubes();
        return blockObject;
    }




	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space")) {
            currentScript.rotateRight();

            
        }
		
	}
}