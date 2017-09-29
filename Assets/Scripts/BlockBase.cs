using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBase : MonoBehaviour {

    public GameObject cubePrefab;
    public General.Block block;
    public int x;
    public int y;
    public int xMax, xMin;


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

	    for (int i = 0; i < 2; i++) {
		    for (int j = 0; j < 4; j++) {
			    for (int k = 0; k < 4; k++) {
				    if (block.block[i, j, k] != 0) {
					    GameObject newCube = Instantiate(cubePrefab);
				    	newCube.transform.SetParent(blockObject.transform);
                        newCube.transform.localPosition = new Vector3(k * General.cubeSize, j * General.cubeSize, i * General.cubeSize);
		    		}
                        
	    		}
	    	}
    	}
    }

    public void rotateRight() {

        int[,,] newBlock = new int[2, 4, 4];
            
        for (int i = 0; i< 2; i++) {
            for (int j = 0; j< block.size; j++) {
                for (int k = 0; k<= block.size; k++) {
                    newBlock[i, j, k] = block.block[ i, k, block.size - j - 1];
                }
            }
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
