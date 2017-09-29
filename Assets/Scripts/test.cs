using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    public GameObject cubePrefabs;

    public struct Block {
        public int[,,] block;
        public int size;
    }

    static Block[] blocks;

    //static int[,,,] blocks = new int[2, 2, 4, 4]
    //    // block 0
    //{
    //    {
    //        {
    //            {1, 2, 0, 0},
    //            {3, 4, 0, 0},
    //            {0, 0, 0, 0},
    //            {0, 0, 0, 0},
    //        },
    //        {
    //            {5, 6, 0, 0},
    //            {7, 8, 0, 0},
    //            {0, 0, 0, 0},
    //            {0, 0, 0, 0},

    //        }
    //    },
    //    {
    //        {
    //            {0, 0, 0, 0},
    //            {0, 1, 0, 0},
    //            {0, 0, 0, 0},
    //            {0, 0, 0, 0},

    //        },
    //        {
    //            {0, 0, 0, 0},
    //            {2, 3, 4, 0},
    //            {0, 5, 0, 0},
    //            {0, 0, 0, 0}
    //        }
    //    }
    //};

    static float cubeSize = 1.05f;

    private Block currentBlock;

	// Use this for initialization
	void Start () {

        // --------------------
        blocks = new Block[2];
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


        currentBlock = blocks[0];

        createBlock(currentBlock, this.gameObject);	
	}

    void createBlock(Block block, GameObject blockObject) {

        for (int i = 0; i < 2; i++) {
            for (int j = 0; j < 4; j++) {
                for (int k = 0; k < 4; k++) {
                    if (block.block[ i, j, k] != 0) {
                        GameObject newCube = Instantiate(cubePrefabs);
                        newCube.transform.SetParent(blockObject.transform);
                        newCube.transform.position = new Vector3(k * cubeSize, j * cubeSize, i * cubeSize);
                    }

                }
            }
        }
    }

    Block rightRotateBlockArray(Block block) {
        Block newBlock;
        newBlock.block = new int[2, 4, 4];

        for (int i = 0; i< 2; i++) {
            for (int j = 0; j< block.size; j++) {
                for (int k = 0; k<= block.size; k++) {
                    newBlock.block[i, j, k] = block.block[ i, k, block.size - j - 1];
                }
            }
        }
        newBlock.size = block.size;

        return newBlock;
    }


	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space")) {
            currentBlock = rightRotateBlockArray(currentBlock);

            for (int i = this.gameObject.transform.childCount - 1; i >= 0; i--) {
                Destroy(this.gameObject.transform.GetChild(i).gameObject);
            }

            createBlock(currentBlock, this.gameObject);

            
        }
		
	}
}