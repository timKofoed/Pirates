using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCameraController : MonoBehaviour {

    public static MiniMapCameraController instance;
	public Ship playerShip;
	public Vector3 cameraOffsetFromPlayer;

	// Use this for initialization
	void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = playerShip.transform.position + cameraOffsetFromPlayer;
		this.transform.LookAt(playerShip.transform, playerShip.transform.forward);
	}
}
