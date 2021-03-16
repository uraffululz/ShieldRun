using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamScript : MonoBehaviour {

	Vector3 camOffset;

	GameObject player;
	
	void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
		
    }


    void LateUpdate() {
		camOffset = new Vector3(player.transform.position.x, 5, -10);

		transform.position = camOffset;
    }
}
