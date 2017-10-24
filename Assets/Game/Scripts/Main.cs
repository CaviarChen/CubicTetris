using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {
    // prefabs
    public GameObject blockPrefab;
    public GameObject hintPrefab;

    // block data
    public static General.Block[] blocks;

    // cube colours
    public Color[] colours;

    public GameObject NextBlock;
    public GameObject FinishedCube;
    public GameObject GameArea;



    // particle for canceling cubes
    private GameObject particle;
    private ParticleSystem particle_system;
    private Vector3 particle_system_start_position;

    // array for all cubes that are fixed
    private GameObject[,,] space = new GameObject[2, General.length, General.height + 4];

    private CameraMovement cameraScript;

    // current and next block
    private GameObject currentBlockObject;
    private GameObject currentNextBlockObject = null;
    private BlockBase currentScript = null;

    private float timeForNextCheck;
    private bool isMoving = false;
    private float timeForMovingAni;
    private int nextBlockId;
    private int nextBlockColourId;
    private float currentTimeForEachDrop;
    private bool isGameOver = false;

    // array for hint boxes
    private GameObject[,] hintboxes = new GameObject[2, 4];

    private GameObject canvas;
    private GameObject cameraPivot;

    // Use this for initialization
    void Start() {

        blocks = General.generateBlockTemplate();


        GameObject camerax;

        // camera
        camerax = GameObject.FindGameObjectWithTag("MainCamera");
        cameraScript = (CameraMovement)camerax.GetComponent(typeof(CameraMovement));
        canvas = GameObject.Find("Canvas");
        cameraPivot = GameObject.Find("CameraPivot");

        // particle system
        particle = GameObject.Find("Particle");
        particle_system = (ParticleSystem)particle.GetComponent(typeof(ParticleSystem));
        particle_system_start_position = particle.transform.position;
        particle.SetActive(false);

        // init and set up the first block
        nextBlockId = Random.Range(0, blocks.Length);
        nextBlockColourId = Random.Range(0, colours.Length);
        addNextBlock();
        Score.init();

    }



    // hint boxes
    void createHintBoxes() {

        clearHintBoxes();

        // for every horizontal points
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

    // destory all hint boxes
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


    // add next block to the scene
    void addNextBlock() {

        // create block
        currentBlockObject = createBlock(this.gameObject, blocks[nextBlockId], nextBlockColourId);
        currentScript = (BlockBase)currentBlockObject.GetComponent(typeof(BlockBase));

        // random pos
        currentScript.x = Random.Range(currentScript.xMin, currentScript.xMax + 1);

        if (!isMovePossible(currentScript.block.block, 0, 0, 1)) {
            if (Random.Range(0, 2) == 1) {
                currentScript.z += 1;
            }
        }
        currentScript.y = General.height;

        // fix position
        currentScript.fixPositionZ();
        currentScript.fixPositionX();
        currentScript.fixPositionY();


        createHintBoxes();

        // reset timer
        currentTimeForEachDrop = General.timeForEachDrop;
        timeForNextCheck = currentTimeForEachDrop;
        timeForMovingAni = -1;

        isMoving = true;
        if (currentNextBlockObject != null) {
            Destroy(currentNextBlockObject);
        }

        // random pick next block
        nextBlockId = Random.Range(0, blocks.Length);
        nextBlockColourId = Random.Range(0, colours.Length);
        currentNextBlockObject = createBlock(this.gameObject, blocks[nextBlockId], nextBlockColourId);
        currentNextBlockObject.transform.parent = NextBlock.transform;
        currentNextBlockObject.transform.localPosition = new Vector3(0, 0, 0);

    }


    // create a block
    GameObject createBlock(GameObject playArea, General.Block block, int cid) {

        GameObject blockObject = Instantiate(blockPrefab);
        blockObject.transform.SetParent(playArea.transform);
        BlockBase script = (BlockBase)blockObject.GetComponent(typeof(BlockBase));
        script.block = block;
        script.createCubes(this, cid);
        script.computeXRange();

        return blockObject;
    }

    // is a point occupied by a fixed cube
    bool isSpaceOccupied(int i, int j, int k) {
        if (!((0 <= i) && (i < 2))) return true;
        if (!((0 <= j) && (j < General.length))) return true;
        if (!((0 <= k) && (k < General.height + 4))) return true;
        if (space[i, j, k] != null) return true;

        return false;
    }


    public bool isMovePossible(int[,,] block, int xOffset, int yOffset, int zOffset) {
        for (int i = 0; i < 2; i++) {
            for (int j = 0; j < 4; j++) {
                for (int k = 0; k < 4; k++) {
                    if (block[i, j, k] != 0) {
                        if (isSpaceOccupied(i + currentScript.z + zOffset, k + currentScript.x + xOffset,
                                                                            j + currentScript.y + yOffset)) {
                            return true;
                        }
                    }
                }
            }
        }


        return false;

    }

    // set all cubes of current block to fixed state
    void finishCurrentBlock() {

        // fix position
        currentScript.fixPositionX();
        currentScript.fixPositionY();
        currentScript.fixRotation();


        // add to space array
        for (int i = 0; i < 2; i++) {
            for (int j = 0; j < 4; j++) {
                for (int k = 0; k < 4; k++) {
                    if (currentScript.block.block[i, j, k] != 0) {
                        space[i + currentScript.z, k + currentScript.x, j + currentScript.y] =
                                                currentScript.cubes[currentScript.block.block[i, j, k]];
                    }
                }
            }
        }

        currentScript.setCubeParent(FinishedCube);
        cameraPivot.transform.Find("Main Camera").GetComponent<CameraShake>().shake();
    }

    // find the first row that is full
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

            if (rowFlag) {
                return k;
            }

        }

        return -1;
    }


    // clean all rows that are full
    void cleanFullRow() {
        int count = 0;

        while (true) {
            int row = findFullRow();
            if (row == -1) break;

            count++;

            // delete
            for (int i = 0; i < 2; i++) {
                for (int j = 0; j < General.length; j++) {
                    Destroy(space[i, j, row]);
                }
            }

            // fall
            for (int k = row; k < General.height + 4; k++) {
                // for each row
                for (int i = 0; i < 2; i++) {
                    for (int j = 0; j < General.length; j++) {
                        if (k == General.height + 4 - 1) {
                            // clean top row
                            space[i, j, k] = null;
                        } else {
                            space[i, j, k] = space[i, j, k + 1];
                            if (space[i, j, k] != null) {
                                space[i, j, k].transform.position -= new Vector3(0.0f, General.cubeSize, 0.0f);
                            }
                        }
                    }
                }
            }
        }

        if (count > 0) {
            // play SE
            SE.Play(1);

            // canceling mutiple rows at once is always better
            Score.addScore(count * 100 + 10 * count * count);

            // particle system
            particle.SetActive(true);
            particle_system.Emit(1000);
        }



    }


    // check if the game is over
    bool checkGameOver() {
        for (int i = 0; i < 2; i++) {
            for (int j = 0; j < General.length; j++) {

                // exceed General.height
                if (space[i, j, General.height] != null) {
                    return true;
                }
            }
        }
        return false;
    }

    public bool GameOver() {
        return isGameOver;
    }

    // swap two variables
    static void Swap<T>(ref T x, ref T y) {
        T t = y;
        y = x;
        x = t;
    }

    // Main update method
    void Update() {

        // GameOver panel
        if (isGameOver) {
            canvas.transform.Find("GameOverPanel").gameObject.SetActive(true);
            return;
        }

        // Menu
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (Time.timeScale == 0) {

                canvas.transform.Find("PauseMenuPanel").gameObject.SetActive(false);

            } else {
                canvas.transform.Find("PauseMenuPanel").gameObject.SetActive(true);

            }
        }

        if (Time.timeScale != 0) {
            // not paused
            if (isMoving) {
                // update timer
                timeForNextCheck -= Time.deltaTime;

                if (timeForNextCheck <= 0) {
                    // doing one check

                    particle.SetActive(false);
                    particle.transform.position = particle_system_start_position;
                    timeForNextCheck += currentTimeForEachDrop;

                    if (isMovePossible(currentScript.block.block, 0, -1, 0)) {
                        // need to finish current block
                        currentScript.fixPositionY();
                        isMoving = false;
                        // play SE
                        SE.Play(0);

                        finishCurrentBlock();
                        clearHintBoxes();
                        cleanFullRow();
                        if (checkGameOver()) {

                            // play SE
                            SE.Play(4);

                            isGameOver = true;
                            return;
                        }
                        Score.addScore(5);
                        addNextBlock();

                    } else {
                        // move one step down
                        currentScript.y -= 1;
                        timeForMovingAni = 0;
                    }

                } else {
                    // normal moving

                    // moving down animation
                    if (timeForMovingAni <= General.timeForEachMoveAni && timeForMovingAni >= 0) {
                        float yChange = -General.cubeSize *
                                            General.rubberBandFunction(timeForMovingAni / General.timeForEachMoveAni);

                        timeForMovingAni += Time.deltaTime;

                        yChange += General.cubeSize *
                                            General.rubberBandFunction(timeForMovingAni / General.timeForEachMoveAni);
                        currentBlockObject.transform.position
                                          += new Vector3(0.0f, -yChange, 0.0f);
                    }

                    // prevent moving too far
                    if (timeForMovingAni > General.timeForEachMoveAni) {
                        currentScript.fixPositionY();
                        timeForMovingAni = -1.0f;
                    }


                    //cameraScript.isFlipped()
                    // 1 : back
                    //-1 : front

                    // default key
                    string dropKey = "space";
                    string leftKey = "a";
                    string rightKey = "d";
                    string upKey = "w";
                    string downKey = "s";
                    string leftRKey = "q";
                    string rightRKey = "e";

                    // swap keys when the camera is flipped
                    if (cameraScript.isFlipped() == 1) {
                        Swap(ref leftKey, ref rightKey);
                        Swap(ref upKey, ref downKey);
                        Swap(ref leftRKey, ref rightRKey);
                    }

                    // rotate
                    if (!isMovePossible(currentScript.block.block, 0, -1, 0)) {
                        if (Input.GetKeyDown(leftRKey)) {
                            currentScript.rotateLeft(this);
                            createHintBoxes();
                        }
                        if (Input.GetKeyDown(rightRKey)) {
                            currentScript.rotateRight(this);
                            createHintBoxes();
                        }
                    }

                    // movement
                    if (Input.GetKeyDown(leftKey)) {
                        if (!isMovePossible(currentScript.block.block, -1, 0, 0)) {
                            currentScript.x -= 1;
                            currentScript.fixPositionX();
                            createHintBoxes();
                            // play SE
                            SE.Play(3);
                        } else {
                            // play SE
                            SE.Play(2);
                            // not possible, shake the camera
                            GameObject.Find("Main Camera").GetComponent<CameraShake>().shake();
                        }
                    }
                    if (Input.GetKeyDown(rightKey)) {
                        if (!isMovePossible(currentScript.block.block, 1, 0, 0)) {
                            currentScript.x += 1;
                            currentScript.fixPositionX();
                            createHintBoxes();
                            // play SE
                            SE.Play(3);
                        } else {
                            // play SE
                            SE.Play(2);
                            // not possible, shake the camera
                            GameObject.Find("Main Camera").GetComponent<CameraShake>().shake();
                        }
                    }
                    if (Input.GetKeyDown(downKey)) {
                        if (!isMovePossible(currentScript.block.block, 0, 0, -1)) {
                            currentScript.z -= 1;
                            currentScript.fixPositionZ();
                            createHintBoxes();
                            // play SE
                            SE.Play(3);
                        } else {
                            // play SE
                            SE.Play(2);
                            // not possible, shake the camera
                            GameObject.Find("Main Camera").GetComponent<CameraShake>().shake();
                        }
                    }
                    if (Input.GetKeyDown(upKey)) {
                        if (!isMovePossible(currentScript.block.block, 0, 0, 1)) {
                            currentScript.z += 1;
                            currentScript.fixPositionZ();
                            createHintBoxes();
                            // play SE
                            SE.Play(3);
                        } else {
                            // play SE
                            SE.Play(2);
                            // not possible, shake the camera
                            GameObject.Find("Main Camera").GetComponent<CameraShake>().shake();
                        }
                    }

                    // drop (change the time for each drop)
                    if (Input.GetKeyDown(dropKey)) {
                        currentTimeForEachDrop = General.timeForEachMoveAni;
                        timeForNextCheck = 0;
                    }
                }
            }
        }
    }

}
