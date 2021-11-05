using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlRunner : MonoBehaviour {

	Rigidbody rb;

	float speed;


    void Start() {
        rb = GetComponent<Rigidbody>();
		speed = GetComponent<PlayerControlMaster>().moveSpeed;
    }


    void Update() {
		//if (Input.GetAxis("Horizontal") != 0) {
		transform.LookAt(transform.position + (Vector3.right/* * Input.GetAxis("Horizontal")*/));


		//rb.AddForce(Vector3.right * speed);
		//transform.Translate(Vector3.right /** Input.GetAxis("Horizontal")*/ * moveSpeed * Time.deltaTime, Space.World);
		//}
	}


	void FixedUpdate () {
		rb.velocity = new Vector3(speed, rb.velocity.y, 0);

	}
}
