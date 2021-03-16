using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enum ShieldState {Hover, Protect};
//ShieldState mode;

public class ShieldControl : MonoBehaviour {

	[SerializeField] bool shieldAttached = true;
	[SerializeField] bool shieldHoverSet = false;
	float shieldParentProx = 0.1f;
	float protectDist = 1.5f;
	float shieldMoveSpeed = 6f;
	float shieldRotSpeed = 180f;

	[SerializeField] GameObject mainCam;
	[SerializeField] GameObject shieldParent;
	[SerializeField] GameObject shield;
	[SerializeField] Collider footCol;

	Animator anim;


	void Start() {
		mainCam = Camera.main.gameObject;
		anim = GetComponent<Animator>();
		//mode = ShieldState.Protect;
	}


	void LateUpdate() {
		//if(mode == ShieldState.Hover) {
		if (!shieldAttached) {
			if (shieldHoverSet) {
				if (Input.GetKeyDown(KeyCode.E)) {
					ActivateProtectMode();
				}
			}
			else {
				if (Input.GetKeyUp(KeyCode.LeftShift)) {
					shieldHoverSet = true;
					Time.timeScale = 1f;
				}
				else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
					Vector3 shieldHoverPos = new Vector3(shield.transform.position.x + (Input.GetAxis("Horizontal") /* shieldMoveSpeed * Time.deltaTime*/),
						shield.transform.position.y + (Input.GetAxis("Vertical") /* shieldMoveSpeed * Time.deltaTime*/), shieldParent.transform.position.z);
					shield.transform.position = Vector3.Lerp(shield.transform.position, shieldHoverPos, shieldMoveSpeed * Time.deltaTime * 2);
				}

				shield.transform.Rotate(transform.right, shieldRotSpeed * Time.deltaTime, Space.World);
			}

			///If the shield goes off-screen, return it to "Protect mode"
			if (shield.transform.position.x <= mainCam.transform.position.x - 10 || shield.transform.position.x >= mainCam.transform.position.x + 10) {
				ActivateProtectMode();
			}
		}
		else if (shieldAttached /*mode == ShieldState.Protect*/) {
			if (Vector3.Distance(shield.transform.position, shieldParent.transform.position) > shieldParentProx) {
				///Move the shield to the position of the shieldParent, near the player
				shield.transform.position = Vector3.Lerp(shield.transform.position, shieldParent.transform.position, shieldMoveSpeed * Time.deltaTime);
			}
			else {
				///Face the front of the shield away from the player
				//Vector3 shieldLookPos = new Vector3(transform.position.x + shieldParent.transform.position.x, transform.position.y + shieldParent.transform.position.y, shieldParent.transform.position.z);
				//shieldParent.transform.LookAt(shieldLookPos);
			}

			if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) {
				///Move/rotate the shieldParent around the player's z-axis
				///Do it by simply placing it at cardinal directions for now
				shieldParent.transform.position = (transform.position + Vector3.up) + new Vector3(Input.GetAxisRaw("Horizontal") * protectDist, Input.GetAxisRaw("Vertical") * protectDist, 0);

				///Face the front of the shield away from the player
				Vector3 shieldLookPos = new Vector3(transform.position.x + Input.GetAxisRaw("Horizontal") * 10, (transform.position.y + Vector3.up.y) + (Input.GetAxisRaw("Vertical") * 10), shieldParent.transform.position.z);
				shieldParent.transform.LookAt(shieldLookPos);
//TODOLATER After implementing 360-degree sensitivity, rotate the parent around the player, rather than placing it at cardinal directions

//TOMAYBEDO Move the following to a relevant object's OnTriggerEnter() function on a script
				if (footCol.GetComponent<Collider>().bounds.Contains(shieldParent.transform.position)) {
					///Activate "Ride Mode"
					ActivateRideMode();

				}
				else {
					anim.SetBool("Riding", false);
					shield.transform.position = shieldParent.transform.position;
				}
			}

			if (Input.GetKeyDown(KeyCode.LeftShift)) {
				ActivateHoverMode();
			}
		}
	}


	void ActivateHoverMode () {
		shieldAttached = false;
		
//TOPROBABLYDO Lerp this down from 1f
		Time.timeScale = .25f; 

		shield.transform.parent = null;

		//yield return new WaitUntil(input.GetKeyUp(KeyCode.LeftShift));

	}


	void ActivateProtectMode () {
		shieldAttached = true;
		shieldHoverSet = false;

		//shieldParent.transform.parent = transform;
		//shieldParent.transform.localPosition = new Vector3(0, 1, shieldParentProx);
		shield.transform.parent = shieldParent.transform;
		shield.transform.localEulerAngles = Vector3.zero;

	}


	void ActivateRideMode () {
//TODO Implement Riding animation
		//anim.SetBool("Riding", true);

		Vector3 shieldRidePos = new Vector3(transform.position.x, transform.position.y - protectDist, transform.position.z);

		shield.transform.position = shieldRidePos;
		shield.transform.LookAt(shield.transform.position - Vector3.up);
	}
}
