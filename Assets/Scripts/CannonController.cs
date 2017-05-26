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
	[SerializeField]
	private float cooldownMax = 3f;
	private float cooldownRemaining = 0f;

	[SerializeField]
	private WeaponUIController.WeaponGroup thisWeaponGroup;

	[SerializeField]
	private WeaponUIController weaponUI;

    private Ship ship;

	// Use this for initialization
	void Start () {
        ship = GetComponentInParent<Ship>();
        if (ship == null)
            Debug.LogError("CannonController ("+this.name+") failed to find its parent ship");
	}
	
	// Update is called once per frame
	void Update () {

		// Update this cannon-group's cooldown timer
		if (cooldownRemaining > 0f)
			cooldownRemaining -= Time.deltaTime;
		else
			cooldownRemaining = 0f;

		// Update this cannon-group's cooldown timer visualization
		weaponUI.UpdateCooldownForWeapon(1f - (cooldownRemaining / cooldownMax), thisWeaponGroup);
	}

	/// <summary>
	/// Aims and fires the cannons at worldPositionToFireAt (either aiming directly for the position, or just in that direction with a ballistic arc).
	/// </summary>
	/// <param name="worldPositionToFireAt">World position to fire at.</param>
	public void FireCannonsAt(Vector3 worldPositionToFireAt, float distance)
	{
        // if the Z angle is positive, then the ship is leaning left. The left cannons need to be modified by -z angle, and the right cannons need the +z angle
        // ...and if the angle is e.g. -10, then the following angle is returned as 350, so I need to change it back to the small negative number I expect
        var shipTilt = ship.transform.localEulerAngles.z > 180.0f ? ship.transform.localEulerAngles.z - 360.0f : ship.transform.localEulerAngles.z;
        float shipTiltOffset = 0f;

        // compensate for the tilt of the ship, so the cannons still aim where we wanted them to (unless the total angle becomes > 45deg relative to the deck)
        switch (thisWeaponGroup)
        {
            case WeaponUIController.WeaponGroup.Top:    shipTiltOffset = 0f;    break;
            case WeaponUIController.WeaponGroup.Bottom: shipTiltOffset = 0f;    break;
            case WeaponUIController.WeaponGroup.Left:
                shipTiltOffset = -shipTilt;
                break;
            case WeaponUIController.WeaponGroup.Right:
                shipTiltOffset = shipTilt;
                break;
        }

        // NOTE: negative angle will turn the cannons UP.
        var angle = Mathf.Clamp( (Mathf.Abs(distance) * -3f) + shipTiltOffset, -45f, 10f);  // distance * -3f seems to work for upwards angle.

        Debug.Log("Cannons (" + this.name + ") go BOOM - distance (" + distance + "), offset ("+shipTilt+" --> "+ shipTiltOffset + ") = angle (" + angle + ")");

        // If the cannons are not on cooldown, then aim the cannons at the position, and fire the projectiles
        if (cooldownRemaining <= 0f)
		{
			cooldownRemaining = cooldownMax;
			foreach (var cannon in cannons)
			{
				// Rotate the cannons in the horizontal plane
				cannon.transform.LookAt(worldPositionToFireAt, Vector3.up);

				// cancel rotation and tilt, because they may be set by the LookAt function
				cannon.transform.localEulerAngles = new Vector3 (
                    angle,	// tilt the cannons upwards (?) - I could use some math to calculate the parabola of the cannons, or just set it to a default angle
					cannon.transform.localEulerAngles.y,
					0f
				);

				// Call a function on each cannon to fire its projectile in the specified direction (with a specific speed?)
				cannon.Fire(cannonPower);
			}
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
