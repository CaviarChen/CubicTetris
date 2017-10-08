using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public GameObject blockPrefab;
    public GameObject hintPrefab;

	public GameObject camera;
	public CameraMovement cameraScript;


    public static General.Block[] blocks;

    public GameObject NextBlock;
    public GameObject FinishedCube;
    public GameObject GameArea;


    public Texture[] textures;


    private GameObject[,,] space = new GameObject[2, General.length, General.height + 4];


    private GameObject currentBlockObject;
    private GameObject currentNextBlockObject = null;
    private BlockBase currentScript;
    private float timeForNextCheck;
    private bool isMoving = false;
    private float timeForMovingAni;
    private int nextBlockId;
    private int nextBlockTextureId;
    private float currentTimeForEachDrop;
    private bool isGameOver = false;

    private GameObject[,] hintboxes = new GameObject[2, 4];

	private GameObject canvas;

	// Use this for initialization
	void Start () {

		camera = GameObject.FindGameObjectWithTag ("MainCamera");
		cameraScript = (CameraMovement)camera.GetComponent(typeof(CameraMovement));
        blocks = General.generateBlockTemplate();
		canvas = GameObject.Find ("Canvas");

        nextBlockId = Random.Range(0, blocks.Length);
        nextBlockTextureId = Random.Range(0, textures.Length);
        addNextBlock();

    }



    // hint boxes
    void createHintBoxes() {

        clearHintBoxes();

        for (int i = 0; i < 2; i++) {
            for (int k = 0; k < 4; k++) {

                bool flag = false;
                int j;
                for (j = 0; j < 4; j++) {
                    if (currentScript.block.block[i, j, k] > 0) {
                        flag = true;
                        break;
                    }
                }

                if (flag) {
                    // need a hint box at [i, k]
                    hintboxes[i, k] = Instantiate(hintPrefab);
                    hintboxes[i, k].transform.SetParent(GameArea.transform);

                    // actual position
                    int x, y, z;
                    x = (k + currentScript.x);
                    z = (i + currentScript.z);

                    //find first empty position
                    for (y = j + currentScript.y; y >= 0; y--) {
                        if (space[z, x, y] != null) {
                            break;
                        }
                    }
                    y += 1;

                    hintboxes[i, k].transform.localPosition = new Vector3(x, y, z) * General.cubeSize;
                    hintboxes[i, k].transform.localPosition += new Vector3(0.0f, -0.2f, 0.0f);

                    
                }
            }
        }
        
    }

    void clearHintBoxes() {
        for (int i = 0; i < 2; i++) {
            for (int k = 0; k < 4; k++) {
                if (hintboxes[i, k] != null) {
                    Destroy(hintboxes[i, k]);
                    hintboxes[i, k] = null;
                }
            }
        }
        
    }

    // ----------

    void addNextBlock() {

        // random block
        currentBlockObject = createBlock(this.gameObject, blocks[nextBlockId], nextBlockTextureId);
		//currentBlockObject = createBlock(this.gameObject, blocks[8]);

        currentScript = (BlockBase)currentBlockObject.GetComponent(typeof(BlockBase));
        // random pos
        currentScript.x = Random.Range(currentScript.xMin, currentScript.xMax + 1);
        
        if (!needStop(currentScript.block.block, 0, 0, 1)) {
            if(Random.Range(0,2)==1) {
                currentScript.z += 1;
            }
        }
        currentScript.fixPositionZ();
        currentScript.fixPositionX();


        createHintBoxes();

        currentTimeForEachDrop = General.timeForEachDrop;
        timeForNextCheck = currentTimeForEachDrop;

        isMoving = true;
        if (currentNextBlockObject != null) {
            Destroy(currentNextBlockObject);
        }

        nextBlockId = Random.Range(0, blocks.Length);
        nextBlockTextureId = Random.Range(0, textures.Length);
        currentNextBlockObject = createBlock(this.gameObject, blocks[nextBlockId], nextBlockTextureId);
        currentNextBlockObject.transform.parent = NextBlock.transform;
        currentNextBlockObject.transform.localPosition = new Vector3(0, 0, 0);


    }


    GameObject createBlock(GameObject playArea, General.Block block, int tid) {
        GameObject blockObject = Instantiate(blockPrefab);
        blockObject.transform.SetParent(playArea.transform);
        BlockBase script = (BlockBase) blockObject.GetComponent(typeof(BlockBase));
        script.block = block;
        script.createCubes(this, tid);
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


    public bool needStop(int[,,] block, int xOffset, int yOffset, int zOffset) {
        for (int i = 0; i < 2; i++) {
            for (int j = 0; j < 4; j++) {
                for (int k = 0; k < 4; k++) {
                    if (block[i,j,k]!=0) {
                        if(isSpaceoccupied(i + currentScript.z + zOffset, k + currentScript.x + xOffset, j + currentScript.y + yOffset)) {
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
                        space[i + currentScript.z, k + currentScript.x, j + currentScript.y] = currentScript.cubes[currentScript.block.block[i, j, k]];
                    }
                }
            }
        }
        currentScript.setCubeParent(FinishedCube);
		transform.Find ("Main Camera").GetComponent<CameraShake> ().shake ();
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


            Score.score += 100;

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


    bool checkGameOver() {
        for (int i = 0; i < 2; i++) {
            for (int j = 0; j < General.length; j++) {
                if (space[i, j, General.height] != null) { // exceed General.height
                    return true;
                }
            }
        }
        return false;
    }


    static void Swap<T>(ref T x, ref T y) {
	    T t = y;
	    y = x;
	    x = t;
    }

    void Update() {
        if (isGameOver) {
            return;
        }

		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (Time.timeScale == 0) {
				Time.timeScale = 1;
				canvas.transform.Find("PauseMenuPanel").gameObject.SetActive(false);
			} else {
				Time.timeScale = 0;
				canvas.transform.Find("PauseMenuPanel").gameObject.SetActive(true);
			}
		}

		if (Time.timeScale != 0) {
			if (isMoving) {




				//  currentBlockObject.transform.position +=
				//      new Vector3(0.0f, -General.cubeSize * (Time.deltaTime / General.timeForEachMove), 0.0f);


				timeForNextCheck -= Time.deltaTime;


				if (timeForNextCheck <= 0) {
					timeForNextCheck += currentTimeForEachDrop;

					if (needStop (currentScript.block.block, 0, -1, 0)) {
						currentScript.fixPositionY ();
						isMoving = false;
						finishCurrentBlock ();
						clearHintBoxes ();
						cleanFullRow ();
						if (checkGameOver ()) {
							isGameOver = true;
							print ("GAME OVER!!!!");
							return;
						}
						addNextBlock ();
					} else {
						currentScript.y -= 1;
						timeForMovingAni = 0;

					}



				} else {


					if (timeForMovingAni <= General.timeForEachMoveAni && timeForMovingAni >= 0) {
						float yChange = -General.cubeSize * General.rubberBandFunction (timeForMovingAni / General.timeForEachMoveAni);

						timeForMovingAni += Time.deltaTime;

						yChange += General.cubeSize * General.rubberBandFunction (timeForMovingAni / General.timeForEachMoveAni);
						currentBlockObject.transform.position
	                                      += new Vector3 (0.0f, -yChange, 0.0f);

					}

					if (timeForMovingAni > General.timeForEachMoveAni) {
						currentScript.fixPositionY ();
						timeForMovingAni = -1.0f;
					}


					//cameraScript.isFlipped()
					// 1 : back
					//-1 : front


					string dropKey = "space";
					string leftKey = "a";
					string rightKey = "d";
					string upKey = "w";
					string downKey = "s";
					string leftRKey = "q";
					string rightRKey = "e";


					if (cameraScript.isFlipped () == 1) {
						Swap (ref leftKey, ref rightKey);
						Swap (ref upKey, ref downKey);
						Swap (ref leftRKey, ref rightRKey);
					}

					if (!needStop (currentScript.block.block, 0, -1, 0)) {
						if (Input.GetKeyDown (leftRKey)) {
							currentScript.rotateLeft (this);
							createHintBoxes ();
						}
						if (Input.GetKeyDown (rightRKey)) {
							currentScript.rotateRight (this);
							createHintBoxes ();
						}
					}

					if (Input.GetKeyDown (leftKey)) {
						if (!needStop (currentScript.block.block, -1, 0, 0)) {
							currentScript.x -= 1;
							currentScript.fixPositionX ();
							createHintBoxes ();
						}
					}

					if (Input.GetKeyDown (rightKey)) {
						if (!needStop (currentScript.block.block, 1, 0, 0)) {
							currentScript.x += 1;
							currentScript.fixPositionX ();
							createHintBoxes ();
						}
					}

					if (Input.GetKeyDown (downKey)) {
						if (!needStop (currentScript.block.block, 0, 0, -1)) {
							currentScript.z -= 1;
							currentScript.fixPositionZ ();
							createHintBoxes ();
						}
					}

					if (Input.GetKeyDown (upKey)) {
						if (!needStop (currentScript.block.block, 0, 0, 1)) {
							currentScript.z += 1;
							currentScript.fixPositionZ ();
							createHintBoxes ();
						}
					}

					if (Input.GetKeyDown (dropKey)) {
						currentTimeForEachDrop = General.timeForEachMoveAni;
						timeForNextCheck = 0;
					}


				}


			}

		}

    }
}