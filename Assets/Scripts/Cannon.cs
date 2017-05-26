using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {

	public GameObject cannonBallPrefab;

    [SerializeField]
    private Transform cannonBallInstantiatePosition;

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
		GameObject cannonBall = GameObject.Instantiate<GameObject>(cannonBallPrefab, cannonBallInstantiatePosition.position, cannonBallInstantiatePosition.rotation, null);
		cannonBall.GetComponent<CannonBall>().SetParentCannon(this.gameObject);
		cannonBall.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * power, ForceMode.VelocityChange);
	}

}
