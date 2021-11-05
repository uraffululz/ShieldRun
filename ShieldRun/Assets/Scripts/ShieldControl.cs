using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//enum ShieldState {Hover, Protect};
//ShieldState mode;

public class ShieldControl : MonoBehaviour {

	Rigidbody rb;

	[SerializeField] bool shieldAttached = true;
	[SerializeField] bool shieldHoverSet = false;
	//float shieldParentProx = 0.1f;
	float protectDist = 1.5f;
	float shieldMoveSpeed = 6f;
	//float shieldRotSpeed = 180f;
	//bool shieldHoverMoving = false;
	Vector3 shieldHoverToPoint;
	float shieldHoverRotSpeed = 360f;
	float currentShieldHoverRot;
	//[SerializeField] InputActionMap map;
	//InputAction moveShield;

	public bool canRide = true;
	public bool rideModeActive = false;

	[SerializeField] GameObject mainCam;
	[SerializeField] GameObject shieldParent;
	public GameObject shield;
	[SerializeField] BoxCollider shieldCol;
	[SerializeField] Collider footCol;

	Animator anim;
	PlayerControlMaster controlScript;


	void OnEnable () {
		//map = GetComponent<PlayerInput>().currentActionMap;
		//map.Enable();
		//moveShield = map.actions[2];
	}

	void Start() {
		rb = GetComponent<Rigidbody>();

		mainCam = Camera.main.gameObject;
		anim = GetComponent<Animator>();
		controlScript = GetComponent<PlayerControlMaster>();
		//mode = ShieldState.Protect;

		shieldHoverToPoint = new Vector3();
	}


	void Update () {

	}


	void LateUpdate () {
		if (rideModeActive) {
			//print(Input.GetAxisRaw("Vertical"));
			Vector3 shieldRidePos = new Vector3(transform.position.x, transform.position.y - 0.05f/*protectDist*/, transform.position.z);
			//Vector3 shieldRidePos = new Vector3(shieldParent.transform.position.x, shieldParent.transform.position.y + 0.5f, shieldParent.transform.position.z);

///TOMAYBEDO Should I be manipulating the Shield Parent's position and rotation instead of the shield itself?
///When in "Protect Mode", I should be manipulating one or the other, NOT BOTH
			shield.transform.position = shieldRidePos;
			shield.transform.LookAt(shield.transform.position - Vector3.up, transform.forward);

			//transform.position = shield.transform.position + (Vector3.up * 0.5f);
		}
		///TODO Determine if the player should be able to use "Ride mode" while airborne.
		///If not, maybe use something besides Input.GetAxisRaw (because it's REALLY NOT a responsive option when trying to determine immediate input)
		


		if (!shieldAttached) {
			if (!shieldHoverSet) {
				if (shieldHoverToPoint != Vector3.zero) {// && /*??? -->*/shield.transform.position != shieldHoverToPoint) {
					shield.transform.position = Vector3.Lerp(shield.transform.position, shield.transform.position + shieldHoverToPoint, shieldMoveSpeed * Time.deltaTime * 3);
				}
///TODO This rotation is automatic for now. Needs to be player-controlled
				if (currentShieldHoverRot != 0) {
					shield.transform.Rotate(Vector3.forward, currentShieldHoverRot * shieldHoverRotSpeed * Time.deltaTime, Space.World);
				}
			}

			///If the shield goes off-screen, return it to "Protect mode"
			if (shield.transform.position.x <= mainCam.transform.position.x - 10 || shield.transform.position.x >= mainCam.transform.position.x + 10 ||
				shield.transform.position.y <= mainCam.transform.position.y - 5.5f || shield.transform.position.y >= mainCam.transform.position.y + 5.5f) {
				Time.timeScale = 1f;
				ActivateProtectMode();
			}

#region obsolete code?
			//}
			//else if (shieldAttached /*mode == ShieldState.Protect*/) {
			//	shield.transform.position = shieldParent.transform.position;

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

			//if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) {

///TODOLATER After implementing 360-degree sensitivity, rotate the parent around the player, rather than placing it at cardinal directions

			//	//TOMAYBEDO Move the following to a relevant object's OnTriggerEnter() function on a script
			//	if (Input.GetAxisRaw("Vertical") == -1) {//footCol.GetComponent<Collider>().bounds.Contains(shieldParent.transform.position)) {
			//		shieldParent.SetActive(true);

			//		if (rideModeActive) {
			//			//print(Input.GetAxisRaw("Vertical"));
			//			Vector3 shieldRidePos = new Vector3(transform.position.x, transform.position.y - 0.05f/*protectDist*/, transform.position.z);
			//			//Vector3 shieldRidePos = new Vector3(shieldParent.transform.position.x, shieldParent.transform.position.y + 0.5f, shieldParent.transform.position.z);

			//			shield.transform.position = shieldRidePos;
			//			shield.transform.LookAt(shield.transform.position - Vector3.up, transform.forward);



			//			//transform.position = shield.transform.position + (Vector3.up * 0.5f);

			//			//if (Input.GetAxisRaw("Vertical") != -1) {
			//			//	StartCoroutine(DeactivateRideMode());
			//			//}

			//		}
			//		//TODO Determine if the player should be able to use "Ride mode" while airborne.
			//		//If not, maybe use something besides Input.GetAxisRaw (because it's REALLY NOT a responsive option when trying to determine immediate input)
			//		else if (controlScript.grounded && canRide && rb.velocity.x != 0 /*|| && Input.GetAxisRaw("Horizontal") != 0*/) {
			//			///Activate "Ride Mode"
			//			StartCoroutine(ActivateRideMode());
			//			print("Ride mode activated");
			//		}
			//		else {
			//			//if (!controlScript.grounded) {
			//			///The player can place the shield at the lower diagonals/angles while airborne
			//			PlaceProtectShield();
			//			//}
			//		}
			//	}
			//	else {
			//		if (rideModeActive) {// && Input.GetAxisRaw("Vertical") != -1) {
			//			StartCoroutine(DeactivateRideMode());
			//		}
			//		else {
			//			//if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) {
			//			PlaceProtectShield();

			//		}
			//	}
			//}
			//else {
			//	shieldParent.SetActive(false);
			//	rideModeActive = false;
			//}

			//if (!rideModeActive && Input.GetKeyDown(KeyCode.LeftShift)) {
			//	ActivateHoverMode();
			//}

#endregion obsolete code?
		
		}
	}


	public void OnCycleHoverMode (/*InputValue stateVal*/) {
///TOMAYBEDO Remove this isPressed check, and just make sure the button input is on "when pressed" only. For now, it's redundant.
		//if (stateVal.isPressed) {
			if (shieldAttached) {
				if (!rideModeActive) {
					ActivateHoverMode();
				}
			}
			else {
				if (shieldHoverSet) {
					//shieldHoverMoving = false;
					ActivateProtectMode();
				}
				else {
					SetShieldHover();
				}
			}
		//}
	}


	public void SetShieldHover () {
		shieldHoverSet = true;
		Time.timeScale = 1f;
	}


	void OnCancelHoverMode () {
		ActivateProtectMode();
	}


	public void OnShieldMovement(InputValue shieldMoveVal) {
		if (shieldAttached && !rideModeActive) {
			//if (shieldMoveVal.Get<Vector2>().y < -.71f) {
				shield.SetActive(true);

//				if (rideModeActive) {
//					//print(Input.GetAxisRaw("Vertical"));
//					Vector3 shieldRidePos = new Vector3(transform.position.x, transform.position.y - 0.05f/*protectDist*/, transform.position.z);
//					//Vector3 shieldRidePos = new Vector3(shieldParent.transform.position.x, shieldParent.transform.position.y + 0.5f, shieldParent.transform.position.z);

/////TOMAYBEDO Should I be manipulating the Shield Parent's position and rotation instead of the shield itself?
/////When in "Protect Mode", I should be manipulating one or the other, NOT BOTH
//					shield.transform.position = shieldRidePos;
//					shield.transform.LookAt(shield.transform.position - Vector3.up, transform.forward);

//					//transform.position = shield.transform.position + (Vector3.up * 0.5f);
//				}
/////TODO Determine if the player should be able to use "Ride mode" while airborne.
/////If not, maybe use something besides Input.GetAxisRaw (because it's REALLY NOT a responsive option when trying to determine immediate input)
				//else if (controlScript.grounded && canRide && /*??? -->*/rb.velocity.x != 0) {
				//	///Activate "Ride Mode"
				//	StartCoroutine(ActivateRideMode());
				//	print("Ride mode activated");
				//}
				//else {
					///The player can place the shield at the lower diagonals/angles while airborne
					PlaceProtectShield(shieldMoveVal.Get<Vector2>());
			//	}
			//}
			//else {
			//	if (rideModeActive) {// && Input.GetAxisRaw("Vertical") != -1) {
			//		StartCoroutine(DeactivateRideMode());
			//	}
			//	else {
			//		//if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) {
			//		PlaceProtectShield(shieldMoveVal.Get<Vector2>());

			//	}
			//}

		}
		else if (!shieldAttached) {
			if (!shieldHoverSet) {
				//shieldHoverMoving = true;
				//if (shieldContext.started) {
				//	shieldHoverToPoint = shield.transform.position;
				//}
				//else {
					shieldHoverToPoint = new Vector3(shieldMoveVal.Get<Vector2>().x, shieldMoveVal.Get<Vector2>().y, 0);
					//new Vector3(shield.transform.position.x + (shieldContext.ReadValue<Vector2>().x /* shieldMoveSpeed * Time.deltaTime*/),
					//	shield.transform.position.y + (shieldContext.ReadValue<Vector2>().y /* shieldMoveSpeed * Time.deltaTime*/), shieldParent.transform.position.z);
				//}


				//shield.transform.position = Vector3.Lerp(shield.transform.position, shieldHoverToPoint, shieldMoveSpeed * Time.deltaTime * 3);
			}
		}

		//print("Positioning shield: X: " + shieldContext.ReadValue<Vector2>().x.ToString() + " | Y: " + shieldContext.ReadValue<Vector2>().y.ToString());

///TOMAYBEDO This may be more efficient to check if (!shieldMoveVal.isPressed)
		if (shieldMoveVal.Get<Vector2>() == Vector2.zero) {
			if (shieldAttached && !rideModeActive) {
				///shield.SetActive(false);
			}
			//rideModeActive = false;

			shieldHoverToPoint = Vector3.zero;
		}
	}


	void ActivateHoverMode () {
		shield.SetActive(true);
		shieldAttached = false;
		
///TOPROBABLYDO Lerp this down from 1f
		Time.timeScale = .25f; 

		shield.transform.parent = null;

		//yield return new WaitUntil(input.GetKeyUp(KeyCode.LeftShift));

	}

	void OnRotateHoverShield (InputValue hoverRotVal) {
		//print(hoverRotVal.Get<float>());
		if (!shieldAttached && !shieldHoverSet) {
			currentShieldHoverRot = -hoverRotVal.Get<float>();
		}
		else {
			currentShieldHoverRot = 0;
		}
	}


	void ActivateProtectMode () {
		shieldCol.isTrigger = true;
		shieldAttached = true;
		shieldHoverSet = false;

		//shieldParent.transform.parent = transform;
		///shieldParent.transform.localPosition = new Vector3(0, 1, 0); ///shieldParentProx);
		shieldParent.transform.position = transform.position + Vector3.up + (transform.forward * protectDist);
		shieldParent.transform.LookAt(new Vector3(transform.position.x, shieldParent.transform.position.y, 0) + (transform.forward * 5));

		shield.transform.parent = shieldParent.transform;
		shield.transform.position = shieldParent.transform.position;
		shield.transform.localEulerAngles = Vector3.zero;
		shield.SetActive(false);
	}


	void PlaceProtectShield(Vector2 protectDir) {
		///Move & rotate the shieldParent around the player's z-axis
		Vector3 shieldLookPos = new Vector3();

		if (protectDir != Vector2.zero) {
			shieldParent.transform.position = (transform.position + Vector3.up) + new Vector3(protectDir.x * protectDist, protectDir.y * protectDist, 0);
			shield.SetActive(true);

			///Face the front of the shield away from the player
			shieldLookPos = new Vector3(transform.position.x + protectDir.x * 10, (transform.position.y + Vector3.up.y) + (protectDir.y * 10), 0);
		}
		else {
			shieldParent.transform.position = transform.position + Vector3.up + (transform.forward * protectDist);
			shield.SetActive(false);
			shieldLookPos = new Vector3(transform.position.x, shieldParent.transform.position.y, 0) + (transform.forward * 5);
		}

//TODO Make sure the shield is rotated correctly when facing ANY DIRECTION
		shieldParent.transform.LookAt(shieldLookPos, -transform.forward);
		//shieldParent.transform.rotation = Quaternion.Euler(0, 0, shieldParent.transform.localRotation.x);

		anim.SetBool("Riding", false); /*???*/
		shield.transform.position = shieldParent.transform.position;
		shield.transform.localEulerAngles = Vector3.zero;
	}


	void OnToggleRideMode (InputValue rideToggle) {
		if (controlScript.grounded && shieldAttached && canRide && !rideModeActive) {
			shield.SetActive(true);
			GetComponent<CapsuleCollider>().center = Vector3.up * .95f;
			rideModeActive = true;
			canRide = false;
			anim.SetBool("Riding", true);
			
			//StartCoroutine(ActivateRideMode());
		}
		else if (!rideToggle.isPressed) {
			if (rideModeActive) {
				StartCoroutine(DeactivateRideMode());
			}
		}
	}


	IEnumerator ActivateRideMode () {
		///Mount the shield and transfer to Ride Mode

		//Vector3 rideModeColCenter = new Vector3(0, .95f, 0);
		GetComponent<CapsuleCollider>().center = Vector3.up * .95f;//rideModeColCenter;
		//transform.position = new Vector3(transform.position.x, transform.position.y + .3f, 0);

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
		//shield.transform.position = shieldParent.transform.position;
		//shield.transform.localEulerAngles = Vector3.zero;
		//shield.SetActive(false);
		ActivateProtectMode();
		anim.SetBool("Riding", false);

		yield return new WaitForSeconds(.1f);
		GetComponent<CapsuleCollider>().center = Vector3.up;
		rideModeActive = false;
		///KEEP FOR NOW canRide = true;

		print("Ride mode deactivated");
	}


}
