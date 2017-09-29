using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public GameObject blockPrefab;


    static General.Block[] blocks;

    public GameObject[,,] space = new GameObject[2, General.length, General.height + 4];


    private GameObject currentBlockObject;
    private BlockBase currentScript;
    private float timeForNextCheck;
    private bool isMoving = false;

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


        currentBlockObject = createBlock(this.gameObject, blocks[0]);
        currentScript = (BlockBase) currentBlockObject.GetComponent(typeof(BlockBase));
        timeForNextCheck = General.timeForEachMove;
        isMoving = true;
        	
	}


    GameObject createBlock(GameObject playArea, General.Block block) {
        GameObject blockObject = Instantiate(blockPrefab);
        blockObject.transform.SetParent(playArea.transform);
        BlockBase script = (BlockBase) blockObject.GetComponent(typeof(BlockBase));
        script.block = block;
        script.createCubes();
        script.computeXRange();


        // temporary
        script.x = script.xMin;
        script.y = General.height - 1 + 4;


        blockObject.transform.position = new Vector3(script.x * General.cubeSize, script.y * General.cubeSize, 0.0f);



        return blockObject;
    }
	
	// Update is called once per frame
	void Update () {
        if (isMoving) {
            currentBlockObject.transform.position +=
                new Vector3(0.0f, -General.cubeSize * (Time.deltaTime / General.timeForEachMove), 0.0f);

            timeForNextCheck -= Time.deltaTime;

            if (timeForNextCheck <= 0) {
                timeForNextCheck += General.timeForEachMove;
                currentScript.y -= 1;
                print(currentScript.y);
                if (currentScript.y == -1) {
                    isMoving = false;
                }

            }
        }




        if (Input.GetKeyDown("space")) {
            currentScript.rotateRight();


        }

    }
}