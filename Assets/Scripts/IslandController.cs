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


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
