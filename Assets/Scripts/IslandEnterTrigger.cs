using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandEnterTrigger : MonoBehaviour {

	[SerializeField]
	private IslandController islandController;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {

		if (other.tag == Ship.PlayerShipTag)
		{
			// The player's ship has entered range of this island, so we need to receive the ship, make it invulnerable, furl the sails and trigger the menu, etc.
			islandController.ShipWantsToDock(other.transform.root.GetComponent<Ship>());

		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == Ship.PlayerShipTag)
		{
			// The player's ship has left range of this island, so we need to let the ship leave
			islandController.ShipWantsToUndock(other.transform.root.GetComponent<Ship>());

		}
	}
}
