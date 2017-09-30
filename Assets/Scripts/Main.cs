using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public GameObject blockPrefab;


    static General.Block[] blocks;


    public GameObject FinishedCube;
    private GameObject[,,] space = new GameObject[2, General.length, General.height + 4];


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

        addNewBlock();



    }

    void addNewBlock() {
        currentBlockObject = createBlock(this.gameObject, blocks[1]);
        currentScript = (BlockBase)currentBlockObject.GetComponent(typeof(BlockBase));
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
        script.y = General.height;

        script.fixPositionX();
        script.fixPositionY();




        return blockObject;
    }

    // Update is called once per frame1

    bool isSpaceoccupied(int i, int j, int k) {
        if (!((0 <= i) && (i < 2))) return true;
        if (!((0 <= j) && (j < General.length))) return true;
        if (!((0 <= k) && (k < General.height + 4))) return true;
        if (space[i, j, k] != null) return true;

        return false;
    }


    public bool needStop(int[,,] block, int xOffset, int yOffset) {
        for (int i = 0; i < 2; i++) {
            for (int j = 0; j < 4; j++) {
                for (int k = 0; k < 4; k++) {
                    if (block[i,j,k]!=0) {
                        if(isSpaceoccupied(i, k + currentScript.x + xOffset, j + currentScript.y + yOffset)) {
                            print("xxx");
                            print(i);
                            print(k + currentScript.x + xOffset);
                            print(j + currentScript.y + yOffset);
                            return true;
                        }
                    }
                }
            }
        }


        return false;

    }

    void finishCurrentBlock() {
        currentScript.fixPositionX();
        currentScript.fixPositionY();

        // add to space
        for (int i = 0; i < 2; i++) {
            for (int j = 0; j < 4; j++) {
                for (int k = 0; k < 4; k++) {
                    if (currentScript.block.block[i, j, k] != 0) {
                        space[i, k + currentScript.x, j + currentScript.y] = currentScript.cubes[currentScript.block.block[i, j, k]];
                    }
                }
            }
        }

        currentScript.setCubeParent(FinishedCube);
        Destroy(currentBlockObject);
    }

    int findFullRow() {
        for (int k = 0; k < General.height; k++) {
            // for each row
            bool rowFlag = true;
            for (int i = 0; i < 2; i++) {
                for (int j = 0; j < General.length; j++) {
                    if (space[i, j, k] == null) {
                        rowFlag = false;
                        break;
                    }
                }
                if (!rowFlag) break;
            }

            if(rowFlag) {
                return k;
            }

        }

        return -1;

    }


    void cleanFullRow() {

        while (true) {
            int row = findFullRow();
            if (row == -1) break;

            // delete
            for (int i = 0; i < 2; i++) {
                for (int j = 0; j < General.length; j++) {
                    Destroy(space[i, j, row]);
                }
            }

            // fall
            for (int k = row; k < General.height+4; k++) {
                // for each row
                for (int i = 0; i < 2; i++) {
                    for (int j = 0; j < General.length; j++) {
                        if(k==General.height+4-1) {
                            // clean top row
                            space[i, j, k] = null;
                        } else {
                            space[i, j, k] = space[i, j, k + 1];
                            if (space[i,j,k]!=null) {
                                space[i, j, k].transform.position -= new Vector3(0.0f, General.cubeSize, 0.0f);
                            }                            
                        }
                        

                    }
                }
            }
        }

    }


	void Update () {
        if (isMoving) {
            currentBlockObject.transform.position +=
                new Vector3(0.0f, -General.cubeSize * (Time.deltaTime / General.timeForEachMove), 0.0f);

            timeForNextCheck -= Time.deltaTime;

            if (timeForNextCheck <= 0) {
                timeForNextCheck += General.timeForEachMove;
                currentScript.y -= 1;
                

                if (needStop(currentScript.block.block,0, -1)) {
                    isMoving = false;
                    finishCurrentBlock();
                    cleanFullRow();
                    addNewBlock();
                } 

            } else {
                if (Input.GetKeyDown("space")) {
                    currentScript.rotateRight(this);
                }
                if (Input.GetKeyDown("a")) {
                    if (!needStop(currentScript.block.block, -1, 0)) {
                        currentScript.x -= 1;
                        currentScript.fixPositionX();
                    }
                }
                if (Input.GetKeyDown("d")) {
                    if (!needStop(currentScript.block.block, 1, 0)) {
                        currentScript.x += 1;
                        currentScript.fixPositionX();
                    }
                }
            }


        }




    }
}