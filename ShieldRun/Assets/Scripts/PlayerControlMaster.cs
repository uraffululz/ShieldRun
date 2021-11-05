using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlMaster : MonoBehaviour {

	ShieldControl shieldCon;

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
	[SerializeField] BoxCollider shieldCol;

	
	void Awake() {
		shieldCon = GetComponent<ShieldControl>();
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

		shield = shieldCon.shield;
    }


    void FixedUpdate() {
		//BoxCollider shieldCol = shield.GetComponent<BoxCollider>();
		///shieldCol.isTrigger = true;

		grounded = false;
		//Debug.DrawRay(transform.position, -transform.up * 1f, Color.red);

		Ray downRay = new Ray(transform.position + (transform.up * 0.5f),  -transform.up * .5f);
		Ray forwardRay = new Ray(transform.position + (transform.up * 0.5f), (transform.forward + (-transform.up * 2)) * .3f);
		Ray backRay = new Ray(transform.position + (transform.up * 0.5f), (-transform.forward + (-transform.up * 2)) * .3f);

		Debug.DrawRay(transform.position + (transform.up * 0.5f), -transform.up * .5f, Color.red);
		Debug.DrawRay(transform.position + (transform.up * 0.5f), (transform.forward + (-transform.up * 2)) * .3f, Color.red);
		Debug.DrawRay(transform.position + (transform.up * 0.5f), (-transform.forward + (-transform.up * 2)) * .3f, Color.red);

		RaycastHit groundHit;
		RaycastHit forwardHit;
		RaycastHit backHit;

		if (Physics.Raycast(downRay, out groundHit, .65f)) {
			if (groundHit.collider.CompareTag("Ground") || groundHit.collider.CompareTag("Shield")) {
				if (groundHit.collider.CompareTag("Shield")) {
					shieldCol.isTrigger = false;
					shieldCon.SetShieldHover();
				}

				grounded = true;
			}
		}

		if (Physics.Raycast(forwardRay, out forwardHit, .65f)) {
			if (forwardHit.collider.CompareTag("Ground") || forwardHit.collider.CompareTag("Shield")) {
				if (forwardHit.collider.CompareTag("Shield")) {
					shieldCol.isTrigger = false;
					shieldCon.SetShieldHover();
				}

				grounded = true;
			}
		}

		if (Physics.Raycast(backRay, out backHit, .65f)) {
			if (backHit.collider.CompareTag("Ground") || backHit.collider.CompareTag("Shield")) {
				if (backHit.collider.CompareTag("Shield")) {
					shieldCol.isTrigger = false;
					shieldCon.SetShieldHover();
				}

				grounded = true;
			}
		}


		//if (grounded && canJump && Input.GetButtonDown("Jump")) {
		//	//StartCoroutine(Jump());
		//	jumpTimeCurrent = jumpTimer;
		//	rb.AddForce(transform.up * (rb.mass * 5), ForceMode.Impulse);
		//	anim.SetBool("Jumping", true);

		//	canJump = false;
		//	shieldCon.canRide = false;
		//}

		if (!canJump) {
			if (jumpTimeCurrent > 0) {
				rb.AddForce(transform.up * ((rb.mass * 5) * (jumpTimeCurrent * 2.75f) * Time.timeScale), ForceMode.Force);

				jumpTimeCurrent -= Time.deltaTime;
			}
			//else {
			//	canJump = false;
			//}
		}

		//if (Input.GetButtonUp("Jump")) {
		//	jumpTimeCurrent = 0;
		//	//canJump = true;
		//}

		transform.position = new Vector3(transform.position.x, transform.position.y, 0);
	}


	void OnCollisionEnter (Collision collision) {
///TOMAYBEDO Is this grounded stuff redundant? Am I doing the same in Update()?
		if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Shield")) {
			//canJump = true;
			anim.SetBool("Jumping", false);

			if (collision.gameObject.CompareTag("Ground")) {
				shieldCon.canRide = true;
			}
		}
	}


	public void OnJump (InputValue jumpVal) {
		//print("Jumping");

		if (grounded && canJump && jumpVal.isPressed) {
			//StartCoroutine(Jump());
			jumpTimeCurrent = jumpTimer;
			rb.AddForce(transform.up * (rb.mass * 5), ForceMode.Impulse);
			anim.SetBool("Jumping", true);

			canJump = false;

//TODO Make sure this doesn't mean the player can just bump their head on a higher platform or something, and then enter ride mode
			shieldCon.canRide = false;
		}

		//if (!canJump) {
		//	if (jumpTimeCurrent > 0) {
		//		rb.AddForce(transform.up * ((rb.mass * 10) * (jumpTimeCurrent * 2.75f) * Time.timeScale), ForceMode.Force);

		//		jumpTimeCurrent -= Time.deltaTime;

		//		print("Jumping higher");
		//	}
		//}

		if (!jumpVal.isPressed) {
			jumpTimeCurrent = 0;
			canJump = true;
			//print("Jump button released");
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
