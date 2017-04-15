using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSmoke : MonoBehaviour {

	private ParticleSystem myParticleSystem;

	// Use this for initialization
	void Start () {
		myParticleSystem = GetComponent<ParticleSystem>();
		float totalDuration = myParticleSystem.main.duration + myParticleSystem.main.startLifetime.constant;
		Destroy(myParticleSystem.gameObject, myParticleSystem.main.duration + myParticleSystem.main.startLifetime.constant);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
