using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlPlatformer : MonoBehaviour {

	PlayerControlMaster masterCon;

	ShieldControl shieldCon;

	Rigidbody rb;
	Animator anim;

	float speed;

	Vector2 inputVec2 = new Vector2(0, 0);


	void Awake () {
		masterCon = GetComponent<PlayerControlMaster>();
		shieldCon = GetComponent<ShieldControl>();

		rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();

		speed = GetComponent<PlayerControlMaster>().moveSpeed;
	}


	void Start() {
        
    }


    void FixedUpdate() {
	
		if (!masterCon.grounded || shieldCon.rideModeActive && (rb.velocity.x > .5f || rb.velocity.x < -.5f)) {
			rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
		}
//		else {
//			///When the player is running (Not riding) and stops moving, stop abruptly
////TOMAYBEDO Instead of stopping abruptly, slow him down quickly, but gradually (maybe using Lerp?)
//			rb.velocity = Vector3.up * rb.velocity.y;
//		}
	}


	public void OnMovement(InputValue moveVal) {
		//print(moveVal.Get<float>());

		if (!shieldCon.rideModeActive) {
			anim.SetFloat("MoveBlend", moveVal.Get<float>());
			transform.LookAt(transform.position + (Vector3.right * moveVal.Get<float>()));

			rb.velocity = new Vector3(moveVal.Get<float>() * speed, rb.velocity.y, 0);
		}
	}
}
