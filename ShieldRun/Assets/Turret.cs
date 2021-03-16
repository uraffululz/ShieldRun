using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

	float timerMax = 2f;
	float timerCurrent;

	[SerializeField] GameObject projectile;
	
	void Start() {
        timerCurrent = timerMax;
    }


    void Update() {
        timerCurrent -= Time.deltaTime;

		if (timerCurrent <= 0) {
			SpawnProjectile();

			timerCurrent = timerMax;
		}
    }


	void SpawnProjectile() {
		Instantiate(projectile, transform.position + (transform.forward * .65f), transform.rotation);
	}
}
