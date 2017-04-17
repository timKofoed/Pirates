using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUIController : MonoBehaviour {

	public enum WeaponGroup 
	{
		Top,
		Left,
		Right,
		Bottom
	}

	public Image weaponWheelTop, weaponWheelBottom, weaponWheelLeft, weaponWheelRight;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateCooldownForWeapon(float cooldownPercentage, WeaponGroup weaponGroup)
	{
		switch (weaponGroup)
		{
		case WeaponGroup.Top:
			weaponWheelTop.fillAmount = cooldownPercentage;
			break;
		case WeaponGroup.Bottom:
			weaponWheelBottom.fillAmount = cooldownPercentage;
			break;
		case WeaponGroup.Left:
			weaponWheelLeft.fillAmount = cooldownPercentage;
			break;
		case WeaponGroup.Right:
			weaponWheelRight.fillAmount = cooldownPercentage;
			break;
		default:
			break;
		}
	}
}
