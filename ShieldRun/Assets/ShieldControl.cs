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
	[SerializeField] bool canRide = true;
	[SerializeField] bool rideModeActive = false;

	[SerializeField] GameObject mainCam;
	[SerializeField] GameObject shieldParent;
	public GameObject shield;
	[SerializeField] Collider footCol;

	Animator anim;
	PlayerControlMaster controlScript;


	void Start() {
		mainCam = Camera.main.gameObject;
		anim = GetComponent<Animator>();
		controlScript = GetComponent<PlayerControlMaster>();
		//mode = ShieldState.Protect;
	}


	void Update () {
		
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
					shield.transform.position = Vector3.Lerp(shield.transform.position, shieldHoverPos, shieldMoveSpeed * Time.deltaTime * 3);
				}

				shield.transform.Rotate(transform.right, shieldRotSpeed * Time.deltaTime, Space.World);
			}

			///If the shield goes off-screen, return it to "Protect mode"
			if (shield.transform.position.x <= mainCam.transform.position.x - 10 || shield.transform.position.x >= mainCam.transform.position.x + 10 ||
				shield.transform.position.y <= mainCam.transform.position.y - 5.5f || shield.transform.position.y >= mainCam.transform.position.y + 5.5f) {
				Time.timeScale = 1f;
				ActivateProtectMode();
			}
		}
		else if (shieldAttached /*mode == ShieldState.Protect*/) {
			shield.transform.position = shieldParent.transform.position;

//			if (Vector3.Distance(shield.transform.position, shieldParent.transform.position) > shieldParentProx) {
//				///Move the shield to the position of the shieldParent, near the player
//				shield.transform.position = Vector3.Lerp(shield.transform.position, shieldParent.transform.position, shieldMoveSpeed * Time.deltaTime);
//			}
//			else {
//				///Once the shield is back in place near the player, allow them to detach it again (otherwise they might hit themselves)
////TOMAYBEDO Alternately, just make the shield disappear when "recalling" it from Hover Mode, and reappear where it belongs (attached to the shieldParent), since it's just LIGHT
//				//shieldAttached = true;

//				///Face the front of the shield away from the player
//				//Vector3 shieldLookPos = new Vector3(transform.position.x + shieldParent.transform.position.x, transform.position.y + shieldParent.transform.position.y, shieldParent.transform.position.z);
//				//shieldParent.transform.LookAt(shieldLookPos);
//			}

			if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) {
				
//TODOLATER After implementing 360-degree sensitivity, rotate the parent around the player, rather than placing it at cardinal directions

//TOMAYBEDO Move the following to a relevant object's OnTriggerEnter() function on a script
				if (Input.GetAxisRaw("Vertical") == -1) {//footCol.GetComponent<Collider>().bounds.Contains(shieldParent.transform.position)) {
					shieldParent.SetActive(true);

					if (rideModeActive) {
						print(Input.GetAxisRaw("Vertical"));
						Vector3 shieldRidePos = new Vector3(transform.position.x, transform.position.y - 0.05f/*protectDist*/, transform.position.z);
						//Vector3 shieldRidePos = new Vector3(shieldParent.transform.position.x, shieldParent.transform.position.y + 0.5f, shieldParent.transform.position.z);

						shield.transform.position = shieldRidePos;
						shield.transform.LookAt(shield.transform.position - Vector3.up);

						

						//transform.position = shield.transform.position + (Vector3.up * 0.5f);

						//if (Input.GetAxisRaw("Vertical") != -1) {
						//	StartCoroutine(DeactivateRideMode());
						//}

					}
//TODO Determine if the player should be able to use "Ride mode" while airborne.
//If not, maybe use something besides Input.GetAxisRaw (because it's REALLY NOT a responsive option when trying to determine immediate input)
					else if (controlScript.grounded && canRide /*&& Input.GetAxisRaw("Horizontal") != 0*/) {
							///Activate "Ride Mode"
							StartCoroutine(ActivateRideMode());
							print("Ride mode activated");
					}
					else {
						//if (Input.GetAxisRaw("Horizontal") != 0) {
							///The player can place the shield at the lower diagonals/angles while airborne
							PlaceProtectShield();
						//}
					}
				}
				else {
					if (rideModeActive) {// && Input.GetAxisRaw("Vertical") != -1) {
						StartCoroutine(DeactivateRideMode());
					}
					else {
						//if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) {
							PlaceProtectShield();
						
					}
				}
			}
			else {
				shieldParent.SetActive(false);
			}

			if (!rideModeActive && Input.GetKeyDown(KeyCode.LeftShift)) {
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


	void PlaceProtectShield() {
		///Move/rotate the shieldParent around the player's z-axis
		///Do it by simply placing it at cardinal directions for now
		shieldParent.transform.position = (transform.position + Vector3.up) + new Vector3(Input.GetAxisRaw("Horizontal") * protectDist, Input.GetAxisRaw("Vertical") * protectDist, 0);
		shieldParent.SetActive(true);
		///Face the front of the shield away from the player
		Vector3 shieldLookPos = new Vector3(transform.position.x + Input.GetAxisRaw("Horizontal") * 10, (transform.position.y + Vector3.up.y) + (Input.GetAxisRaw("Vertical") * 10), shieldParent.transform.position.z);
		shieldParent.transform.LookAt(shieldLookPos);

		anim.SetBool("Riding", false);
		shield.transform.position = shieldParent.transform.position;
		shield.transform.LookAt(shieldLookPos);
	}


	IEnumerator ActivateRideMode () {
		///Mount the shield and transfer to Ride Mode

		//Vector3 rideModeColCenter = new Vector3(0, .95f, 0);
		GetComponent<CapsuleCollider>().center = Vector3.up * .95f;//rideModeColCenter;

		rideModeActive = true;
		canRide = false;
		yield return new WaitForSeconds(.1f);

		//TODO Implement Riding animation
		//anim.SetBool("Riding", true);

		//Vector3 shieldRidePos = new Vector3(transform.position.x, transform.position.y - 0.05f/*protectDist*/, transform.position.z);

		//shield.transform.position = shieldRidePos;
		//shield.transform.LookAt(shield.transform.position - Vector3.up);


		//yield return new WaitUntil(() => Input.GetAxisRaw("Vertical") >= 0);
		//rideModeActive = false;
		//canRide = true;

		//print("Ride mode deactivated");

	}


	IEnumerator DeactivateRideMode() {
		///Transition from Ride Mode to Protect Mode
		
		shieldParent.SetActive(false);

		yield return new WaitForSeconds(.1f);
		GetComponent<CapsuleCollider>().center = Vector3.up;
		rideModeActive = false;
		canRide = true;

		print("Ride mode deactivated");

	}


}
