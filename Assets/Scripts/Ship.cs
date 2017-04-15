using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ship : MonoBehaviour 
{
	private struct CannonGroup
	{
		public float distanceToTarget;
		public CannonController cannonController;

		public CannonGroup(float distance, CannonController cannons)
		{
			distanceToTarget = distance;
			cannonController = cannons;
		}
	}

	/// <summary>
	/// The front and rear shooting angles measured using both sides of the ship.
	/// </summary>
	public float frontRearShootingAngle = 50f;

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

	/// <summary>
	/// The cannon groups on the various sides of the ship.
	/// </summary>
	public List<CannonController> cannonGroups;

	public List<CannonController> cannonGroupsFrontRear;
	public List<CannonController> cannonGroupsSides;

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
		// When we click the mouse button, attempt to fire the closest cannon(s)
		if (Input.GetMouseButtonDown(0))
		{ // if left button pressed...
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
//				Debug.Log("Clicked item: " + hit.collider.gameObject.name + " at coords: " + hit.point);

				// Determine which of the sides of the ship is closer to the target, and therefore which we should be commanding
				CannonGroup cannonsSelected = new CannonGroup();

				// Use dot-product to determine which group of cannons to use, so we can specify an angle for each group of cannons (e.g. front/rear: 30 deg, sides: 150 deg)
				Vector3 vectorToPoint = new Vector3();
				vectorToPoint = (hit.point - this.transform.position).normalized;
				float dotProduct = Vector3.Dot(vectorToPoint, transform.forward);
//				Debug.Log("dotProduct = " + dotProduct);

				List<CannonController> cannonsGroupsSelected = cannonGroups;	// use all cannon groups

				// halve the front/rear angle provided, to get the angle from one side of the ship, and convert it to a Cosine value we can compare the Dot-product result to
				float frontRearAnglesCOS = Mathf.Cos(Mathf.Deg2Rad * (frontRearShootingAngle/2f));

				// calculate dot-product based on vectors https://www.mathsisfun.com/algebra/vector-calculator.html
				// input a = 1, 90 deg	b = 1, -65 deg	= -0.906308		(this means 25 deg from the center-line on either side is 0.9 to 1.0)
				if (Mathf.Abs(dotProduct) < frontRearAnglesCOS)	// 0.9 = 25 deg on either side of the ship = 50 deg front/rear and 130 deg for the side-cannons
				{
					// Use the side-cannons
					cannonsGroupsSelected = cannonGroupsSides;
				}
				else
				{
					// Use front/rear cannons
					cannonsGroupsSelected = cannonGroupsFrontRear;
				}

				// use distance to each cannon group to select one
				float currentDistance = 0f;
				foreach (var cannonGroup in cannonsGroupsSelected)
				{
					currentDistance = cannonGroup.DistanceToTarget(hit.point);
					if (currentDistance < cannonsSelected.distanceToTarget || cannonsSelected.distanceToTarget == 0f)
					{
						cannonsSelected.distanceToTarget = currentDistance;
						cannonsSelected.cannonController = cannonGroup;
					}
				}

				Debug.Log("Distance to fire: " + currentDistance);

				// If we found a cannon-group, then order it to fire at the target
				if (cannonsSelected.cannonController != null)
					cannonsSelected.cannonController.FireCannonsAt(hit.point, currentDistance);
				else
					Debug.Log("No cannons selected?");

			}
		}

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
			myRigidbody.AddRelativeForce (Vector3.forward * (float)numberOfSailsUp * maxVelocityPerSail, forceModeToUse);
		else if (myRigidbody.velocity.magnitude < 0.3f)
			myRigidbody.AddRelativeForce (Vector3.forward, forceModeToUse);	// always apply a little forward motion, so we can always steer

		// Apply rotational torque based on the speed of the ship. We can't turn the ship, if we don't have any movement
		if (Input.GetAxis("Horizontal") > 0)
			myRigidbody.AddRelativeTorque(Vector3.up * myRigidbody.velocity.magnitude * (numberOfSailsUp > 0?(float)numberOfSailsUp * maxVelocityPerSail : 0.1f * maxVelocityPerSail), forceModeToUse);
		else if (Input.GetAxis("Horizontal") < 0)
			myRigidbody.AddRelativeTorque(Vector3.down * myRigidbody.velocity.magnitude * (numberOfSailsUp > 0?(float)numberOfSailsUp * maxVelocityPerSail : 0.1f * maxVelocityPerSail), forceModeToUse);

		transform.localEulerAngles = new Vector3 (
			0f,
			transform.localEulerAngles.y,
			transform.localEulerAngles.z);
	}
}
