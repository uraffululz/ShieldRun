using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

	int maxHP = 5;
	[SerializeField] int currentHP;


	void Awake () {
		currentHP = maxHP;
	}


	void Start() {
        
    }


    void Update() {
        
    }


	public void TakeDamage (int dmg) {
		currentHP -= dmg;
		print("HP: " + currentHP + " / " + maxHP);

		if (currentHP <= 0) {
			Die();
		}
	}


	void Die () {
		print("YOU DIED!");

		///sceneManager.sceneActive = false;
	}
}
