using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject objectToFollow;
	public Vector3 distanceToObject;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = objectToFollow.transform.position + distanceToObject;
		this.transform.LookAt(objectToFollow.transform);
		this.transform.localEulerAngles = new Vector3 (
			this.transform.localEulerAngles.x,
			0f,
			0f
		);
	}
}
