using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitScript : MonoBehaviour {

    void Start() {
        
    }


    void Update() {
        
    }


	void OnCollisionEnter (Collision collision) {
		if (collision.collider.CompareTag("Player")) {
			collision.collider.GetComponent<PlayerHealth>().TakeDamage(1);
			print ("Player touched the pit!");
		}
	}
}
