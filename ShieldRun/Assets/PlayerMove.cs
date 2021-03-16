using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

	float moveSpeed = 4;

	[SerializeField] bool isJumping = false;

	Rigidbody rb;
	Animator anim;

	
	void Awake() {
		rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }


    void Update() {
		//if (Input.GetAxis("Horizontal") != 0) {
			transform.LookAt(transform.position + (Vector3.right/* * Input.GetAxis("Horizontal")*/));

			transform.Translate(Vector3.right /** Input.GetAxis("Horizontal")*/ * moveSpeed * Time.deltaTime, Space.World);
		//}

		if (!isJumping && Input.GetButtonDown("Jump")) {
			StartCoroutine(Jump());
		}

		transform.position = new Vector3(transform.position.x, transform.position.y, 0);
	}


	//void OnCollisionEnter (Collision collision) {
	//	if (collision.gameObject.CompareTag("Hazard")) {
	//		print("Player hit by hazard");
	//	}
	//}


	IEnumerator Jump() {
		isJumping = true;
		anim.SetBool("Jumping", true);

		yield return new WaitForSeconds(.4f);
		rb.AddForce(transform.up * (rb.mass * 10), ForceMode.Impulse);

		yield return new WaitForSeconds(1.5f);
		anim.SetBool("Jumping", false);

		yield return new WaitForSeconds(.2f);
		isJumping = false;
	}
}
