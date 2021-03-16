using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	Rigidbody rb;
	
	void Start() {
        rb = GetComponent<Rigidbody>();

		rb.AddForce(transform.forward * 30, ForceMode.Force);

	}


	void Update() {
        //rb.AddForce(transform.forward * 5, ForceMode.Force);
    }


	void OnTriggerEnter (Collider collision) {
		if (collision.gameObject.CompareTag("Player")) {
			print("Player hit by hazard");
		}
		
		Destroy(gameObject);
	}
}
