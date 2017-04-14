using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ship : MonoBehaviour 
{
	/// <summary>
	/// The more sails which are up, the greater the speed of the ship.
	/// </summary>
	[SerializeField]
	private int numberOfSailsUp = 0;
	public float maxVelocityPerSail;
	private int numberOfSailsAvailable = 3;

	/// <summary>
	/// The temporary sails, which I will make (un)furl based on current speed.
	/// </summary>
	[SerializeField]
	private List<GameObject> sails;

	public Text debugText;
	private Rigidbody myRigidbody
	{
		get
		{ 
			if (_myRigidbody != null)
				return _myRigidbody;
			else
			{
				Debug.LogError ("No rigidbody found on ("+this.name+")");
				return null;
			}
		} 
		set{
			_myRigidbody = value;
		}
	}
	private Rigidbody _myRigidbody;

	// Use this for initialization
	void Start () {
		numberOfSailsAvailable = sails.Count;
		myRigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () 
	{


	}

	// Anything done in FixedUpdate, is called in certain number of times per second, based on the physics settings
	void FixedUpdate()
	{
		if (Input.GetAxis("Vertical") > 0)
		{
			numberOfSailsUp = numberOfSailsAvailable;
		}
		else if (Input.GetAxis("Vertical") < 0)
		{
			numberOfSailsUp = 0;
		}

		// Apply movement based on the number of unfurled sails
		ApplyMovement();

		// Check the sails' furl-status, to animate them to their intended sizes
		FurlSails();
	}

	/// <summary>
	/// (un)Furls the sails based on the number of active sails.
	/// </summary>
	private void FurlSails()
	{
		if (numberOfSailsUp > 0)
		{
			for (int i = 0; i < sails.Count; i++)
			{
				if (sails[i].transform.localScale.y < 1f)
				{
					sails[i].transform.localScale = new Vector3 (
						sails[i].transform.localScale.x,
						sails[i].transform.localScale.y + 0.01f,
						sails[i].transform.localScale.z);
				}
			}
		}
		else
		{
			for (int i = 0; i < sails.Count; i++)
			{
				if (sails[i].transform.localScale.y > 0.1f)
				{
					sails[i].transform.localScale = new Vector3 (
						sails[i].transform.localScale.x,
						sails[i].transform.localScale.y - 0.01f,
						sails[i].transform.localScale.z);
				}
			}
		}
	}

	private void ApplyMovement()
	{
		debugText.text = "Forward movement: " + Vector3.forward * (float)numberOfSailsUp + "\n Velocity: " + myRigidbody.velocity.magnitude;
		ForceMode forceModeToUse = ForceMode.Force;

		// Apply forward movement based on the number of unfurled sails
		if (myRigidbody.velocity.magnitude < (maxVelocityPerSail * (float)numberOfSailsUp))
			myRigidbody.AddRelativeForce (Vector3.forward * (float)numberOfSailsUp, forceModeToUse);
		else if (myRigidbody.velocity.magnitude < 0.3f)
			myRigidbody.AddRelativeForce (Vector3.forward, forceModeToUse);	// always apply a little forward motion, so we can always steer

		// Apply rotational torque based on the speed of the ship. We can't turn the ship, if we don't have any movement
		if (Input.GetAxis("Horizontal") > 0)
			myRigidbody.AddRelativeTorque(Vector3.up * myRigidbody.velocity.magnitude * (numberOfSailsUp > 0?(float)numberOfSailsUp:(float)numberOfSailsAvailable), forceModeToUse);
		else if (Input.GetAxis("Horizontal") < 0)
			myRigidbody.AddRelativeTorque(Vector3.down * myRigidbody.velocity.magnitude * (numberOfSailsUp > 0?(float)numberOfSailsUp:(float)numberOfSailsAvailable), forceModeToUse);

		transform.localEulerAngles = new Vector3 (
			0f,
			transform.localEulerAngles.y,
			transform.localEulerAngles.z);
	}
}
