using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlMaster : MonoBehaviour {

	//SceneManager sceneMan;
	public float moveSpeed = 4;
	public float moveSpeedMax = 10;

	public bool grounded = false;
	[SerializeField] bool canJump = true;
	[SerializeField] float jumpTimer;
	[SerializeField] float jumpTimeCurrent;

	Rigidbody rb;
	Animator anim;

	GameObject shield;

	
	void Awake() {
		rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

		if (GameObject.Find("SceneManager").GetComponent<SceneManager>().levelType == SceneManager.levelTypes.Platformer) {
			GetComponent<PlayerControlPlatformer>().enabled = true;
			GetComponent<PlayerControlRunner>().enabled = false;
		}
		else {
			GetComponent<PlayerControlPlatformer>().enabled = false;
			GetComponent<PlayerControlRunner>().enabled = true;
		}

		shield = GetComponent<ShieldControl>().shield;
    }


    void Update() {
		shield.GetComponent<BoxCollider>().isTrigger = true;

		grounded = false;
		//Debug.DrawRay(transform.position, -transform.up * 1f, Color.red);

		Ray downRay = new Ray(transform.position + (transform.up * 0.5f),  -transform.up);
		Ray forwardRay = new Ray(transform.position + (transform.up * 0.5f), transform.forward + -transform.up);
		Ray backRay = new Ray(transform.position + (transform.up * 0.5f), -transform.forward + -transform.up);

		Debug.DrawRay(transform.position + (transform.up * 0.5f), -transform.up, Color.red);
		Debug.DrawRay(transform.position + (transform.up * 0.5f), transform.forward + -transform.up, Color.red);
		Debug.DrawRay(transform.position + (transform.up * 0.5f), -transform.forward + -transform.up, Color.red);

		RaycastHit groundHit;
		RaycastHit forwardHit;
		RaycastHit backHit;

		if (Physics.Raycast(downRay, out groundHit, .75f)) {
			if (groundHit.collider.CompareTag("Ground") || groundHit.collider.CompareTag("Shield")) {
				if (groundHit.collider.CompareTag("Shield")) {
					shield.GetComponent<BoxCollider>().isTrigger = false;
				}

				grounded = true;
			}
		}

		if (Physics.Raycast(forwardRay, out forwardHit, .75f)) {
			if (forwardHit.collider.CompareTag("Ground") || forwardHit.collider.CompareTag("Shield")) {
				if (forwardHit.collider.CompareTag("Shield")) {
					shield.GetComponent<BoxCollider>().isTrigger = false;
				}

				grounded = true;
			}
		}

		if (Physics.Raycast(backRay, out backHit, .75f)) {
			if (backHit.collider.CompareTag("Ground") || backHit.collider.CompareTag("Shield")) {
				if (backHit.collider.CompareTag("Shield")) {
					shield.GetComponent<BoxCollider>().isTrigger = false;
				}

				grounded = true;
			}
		}

		//if (Physics.Raycast(transform.position + (transform.up * .01f), -transform.up, out groundHit, 1f)) {
		//	//print(groundHit.collider.name);
		//	if (groundHit.collider.CompareTag("Ground") || groundHit.collider.gameObject == shield) {//groundHit.collider.CompareTag("Shield")) {
		//		if (groundHit.collider.gameObject == shield) {
		//			shield.GetComponent<BoxCollider>().isTrigger = false;
		//		}
		//		grounded = true;
		//	}
		//}

		if (grounded && canJump && Input.GetButtonDown("Jump")) {
			//StartCoroutine(Jump());
			jumpTimeCurrent = jumpTimer;
			rb.AddForce(transform.up * (rb.mass * 5), ForceMode.Impulse);
			anim.SetBool("Jumping", true);

			canJump = false;
		}

		if (!canJump && Input.GetButton("Jump")) {
			if(jumpTimeCurrent > 0) {
				rb.AddForce(transform.up * ((rb.mass * 10) * (jumpTimeCurrent * 2.75f) * Time.timeScale), ForceMode.Force);

				jumpTimeCurrent -= Time.deltaTime;
			}
			//else {
			//	canJump = false;
			//}
		}

		if (Input.GetButtonUp("Jump")) {
			jumpTimeCurrent = 0;
			//canJump = true;
		}

		transform.position = new Vector3(transform.position.x, transform.position.y, 0);
	}


	void OnCollisionEnter (Collision collision) {
		if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Shield")) {
			canJump = true;
			anim.SetBool("Jumping", false);

		}
	}


	IEnumerator Jump() {
		//jumpTimeCurrent = jumpTimer;

		//canJump = false;
		anim.SetBool("Jumping", true);

		yield return new WaitForSeconds(.4f);
		//rb.AddForce(transform.up * (rb.mass * 5), ForceMode.Impulse);
		//print("jumping");

		yield return new WaitForSeconds(1.5f);
		anim.SetBool("Jumping", false);

		yield return new WaitForSeconds(.2f);
		//canJump = true;
	}
}
