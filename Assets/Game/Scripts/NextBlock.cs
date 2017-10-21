using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextBlock : MonoBehaviour {

    public float rotationSpeed;

	// Update is called once per frame
	void Update () {

        // keep rotating the blcok
        if(this.gameObject.transform.childCount>=1) {
            GameObject blockObject = this.gameObject.transform.GetChild(0).gameObject;
            BlockBase script = (BlockBase)blockObject.GetComponent(typeof(BlockBase));
            float center = General.cubeSize * (script.block.size - 1) / 2.0f;

            for (int i = 0; i < blockObject.transform.childCount; i++) {
                blockObject.transform.GetChild(i).gameObject.transform.RotateAround
                           (blockObject.transform.position + new Vector3(center, center, 0.0f),
                                                    new Vector3(0.0f, 0.0f, 1.0f), Time.deltaTime * rotationSpeed);
            }
        }
    }
}
