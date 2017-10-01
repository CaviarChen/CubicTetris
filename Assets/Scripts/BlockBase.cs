using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBase : MonoBehaviour {

    public GameObject cubePrefab;
    public General.Block block;
    public int x;
    public int y;
    public int xMax, xMin;
    public GameObject[] cubes;


    private int leftOffset() {
        for (int k = 0; k< 4; k++) {
            for (int j = 0; j< 4; j++) {
                for (int i = 0; i< 2; i++) {
                    if (block.block[i,j,k] != 0) {
                        return k;
                    }
                }
            }
        }
        return 0;
    }

    private int rightOffset() {
	    for (int k = 3; k >= 0; k--) {
		    for (int j = 0; j < 4; j++) {
			    for (int i = 0; i < 2; i++) {
				    if (block.block[i, j, k] != 0) {
					    return k;
				    }
			    }
		    }
	    }
	    return 0;
    }

    public void computeXRange() {
        xMin = -leftOffset();
        xMax = General.length - 1 - 4 + rightOffset();

        print(xMin);
        print(xMax);

    }


    public void createCubes() {
        GameObject blockObject = this.gameObject;

	    for (int i = blockObject.transform.childCount - 1; i >= 0; i--) {
		    Destroy(blockObject.transform.GetChild(i).gameObject);
	    }

        cubes = new GameObject[4 * 4 * 2 + 1];

        for (int i = 0; i < 2; i++) {
		    for (int j = 0; j < 4; j++) {
			    for (int k = 0; k < 4; k++) {
				    if (block.block[i, j, k] != 0) {
					    GameObject newCube = Instantiate(cubePrefab);
				    	newCube.transform.SetParent(blockObject.transform);
                        newCube.transform.localPosition = new Vector3(k * General.cubeSize, j * General.cubeSize, i * General.cubeSize);
                        cubes[block.block[i, j, k]] = newCube;
		    		}
                        
	    		}
	    	}
    	}
    }

    public void setCubeParent(GameObject parent) {
        GameObject blockObject = this.gameObject;
        for (int i = blockObject.transform.childCount - 1; i >= 0; i--) {
            blockObject.transform.GetChild(i).gameObject.transform.parent = parent.transform;
        }

    }

    public void fixPositionX() {
        this.gameObject.transform.position = new Vector3(x * General.cubeSize, this.gameObject.transform.position.y, 0.0f);
    }

    public void fixPositionY() {
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, y * General.cubeSize, 0.0f);
    }

    public void rotateRight(Main mainScript) {

        int[,,] newBlock = new int[2, 4, 4];
            
        for (int i = 0; i< 2; i++) {
            for (int j = 0; j< block.size; j++) {
                for (int k = 0; k< block.size; k++) {
                    newBlock[i, j, k] = block.block[ i, k, block.size - j - 1];
                }
            }
        }

        if (mainScript.needStop(newBlock, 0, 0)) {
            return;
        }

        block.block = newBlock;

        this.createCubes();
    }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
