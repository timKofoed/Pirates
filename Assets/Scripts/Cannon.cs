using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {

	public GameObject cannonBallPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// Instantiate a cannon ball and propel it out of the cannon
	/// </summary>
	public void Fire(float power = 30f)
	{
		GameObject cannonBall = GameObject.Instantiate<GameObject>(cannonBallPrefab, this.transform.position, this.transform.rotation, null);
		cannonBall.GetComponent<CannonBall>().SetParentCannon(this.gameObject);
		cannonBall.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * power, ForceMode.VelocityChange);
	}

}
