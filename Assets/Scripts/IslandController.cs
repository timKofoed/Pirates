using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Island controller, which will received an incoming ship, place it in the dock, furl the sails and trigger the appropriate menu, etc.
/// </summary>
public class IslandController : MonoBehaviour {

	[SerializeField]
	private GameObject harbour;

	private Ship dockedShip;

    [SerializeField]
    private GameObject icon;

	// Use this for initialization
	void Start () {
        //re-orient the icon for the minimap, so it is pointing the correct way
        //var lookAtPos = icon.transform.position;
        //lookAtPos.z -= 1f;
        //icon.transform.LookAt(lookAtPos);

    }
	
	// Update is called once per frame
	void Update () {
        // Keep the icon rotated correctly, depending on how the minimap camera is rotated
        if (MiniMapCameraController.instance != null)
            icon.transform.localEulerAngles = new Vector3(0f, MiniMapCameraController.instance.transform.localEulerAngles.y + 180f, 0f);
	}

	public void ShipWantsToDock(Ship shipToDock)
	{
		if (dockedShip == shipToDock)
			return;
		else
		{
			dockedShip = shipToDock;
			shipToDock.EnterDock(harbour.transform);
		}
	}

	public void ShipWantsToUndock(Ship shipToUndock)
	{
		if (dockedShip == shipToUndock)
			dockedShip = null;
	}
}
