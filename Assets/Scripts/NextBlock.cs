using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextBlock : MonoBehaviour {

    public float rotationSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        for (int i = this.gameObject.transform.childCount - 1; i >= 0; i--) {
            GameObject blockObject = this.gameObject.transform.GetChild(i).gameObject;
            BlockBase script = (BlockBase)blockObject.GetComponent(typeof(BlockBase));
            float center = General.cubeSize * (script.block.size - 1) / 2.0f;


            for (int j = blockObject.transform.childCount - 1; j >= 0; j--) {
                blockObject.transform.GetChild(j).gameObject.transform.RotateAround
                           (blockObject.transform.position + new Vector3(center, center, 0.0f), new Vector3(0.0f, 0.0f, 1.0f), Time.deltaTime * rotationSpeed);
            }

        }

    }
}
