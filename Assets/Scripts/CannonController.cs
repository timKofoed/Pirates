using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Cannon controller, which will determine which cannons to fire, where to point them and replace them with upgrades, etc.
/// </summary>
public class CannonController : MonoBehaviour {

	// Note: I may need to replace this with a cannon script, so each cannon can handle it's own firing animation and projectile, cooldown, etc.
	public List<Cannon> cannons;
	public float cannonPower = 10f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// Aims and fires the cannons at worldPositionToFireAt (either aiming directly for the position, or just in that direction with a ballistic arc).
	/// </summary>
	/// <param name="worldPositionToFireAt">World position to fire at.</param>
	public void FireCannonsAt(Vector3 worldPositionToFireAt, float distance)
	{
		// aim the cannons at the position, and fire the projectiles
//		Debug.Log("Cannons ("+ this.name +") go BOOM");
		foreach (var cannon in cannons)
		{
			// Rotate the cannons in the horizontal plane
			cannon.transform.LookAt(worldPositionToFireAt, Vector3.up);

			// cancel rotation and tilt, because they may be set by the LookAt function
			cannon.transform.localEulerAngles = new Vector3 (
				-30f,	// tilt the cannons upwards (?) - I could use some math to calculate the parabola of the cannons, or just set it to a default angle
				cannon.transform.localEulerAngles.y,
				0f
			);

			// Call a function on each cannon to fire its projectile in the specified direction (with a specific speed?)
			cannon.Fire(cannonPower);
		}



	}

	/// <summary>
	/// Returns the distance to the target, to be used to determine which of the cannon sides to use (the cannon-group with the shortest distance is selected)
	/// </summary>
	/// <returns>The to target.</returns>
	public float DistanceToTarget(Vector3 worldPositionToCheck)
	{
		return Vector3.Distance(this.transform.position, worldPositionToCheck);
	}
}
