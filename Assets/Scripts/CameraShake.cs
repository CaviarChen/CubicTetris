using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

	private bool isshakeCamera = false;

	public float shakeLevel = 3f;
	public float setShakeTime = 0.2f;

	private float shakeTime = 0.0f;
	private float shakeDelta = 0.005f;
	private Camera mainCamera;

	private int count = 5;

	// Use this for initialization
	void Start()
	{
		mainCamera = GetComponent<Camera>();
		shakeTime = setShakeTime;
		shakeDelta = 0.005f;
	}

	// Update is called once per frame
	void Update()
	{
		if (isshakeCamera)
		{
			if (shakeTime > 0)
			{
				shakeTime -= Time.deltaTime;
				if (shakeTime <= 0)
				{
					mainCamera.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
					isshakeCamera = false;
					shakeTime = setShakeTime;
					shakeDelta = 0.005f;
				}
				else
				{
					if (count <= 5)
					{
						count--;
						if (count == 0) {
							count = 5;
						}
						mainCamera.rect = new Rect(shakeDelta * (-1.0f + shakeLevel * Random.value), shakeDelta * (-1.0f + shakeLevel * Random.value), 1.0f, 1.0f);
					}
				}
			}
		}
	}

	public void shake()
	{
		isshakeCamera = true;
	}
}
