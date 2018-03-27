using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPCharacterController : MonoBehaviour {
	

	public Transform playerCam, character, centerPoint;
	private CharacterController controller;
	private Animator anim;

	[Header("Camera Zoom")]
	private float mouseX, mouseY;
	public float mouseSensitivity = 10f;
	public float mouseYPosition = 1f;

	[Header("Camera Zoom")]
	private float zoom;
	public float zoomSpeed;
	public float zoomMin;
	public float zoomMax;

	[Header("Moving Player")]
	public float rotationSpeed = 5f;
	private float moveFB, moveLR;
	public float moveSpeed = 2f;


	public float verticalVelocity;
	[Header("Jump")]
	public float gravity =  14.0f;
	private float gravity2;
	public float jumpForce = 10.0f;
	public int JumpCombo;

	[Header("Sound")]
	public AudioClip MarioJump1;
	public AudioClip MarioJump2;
	public AudioClip MarioJump3;
	public AudioClip MarioAttack1;
	public AudioClip MarioHurt;
	public AudioClip MarioImpact;
	AudioSource audiosource;

	[Header("Gestion Combo")]
	public float currentComboTimer;
	private bool ActivateTimerToReset;
	private int currentComboState;
	private float origTimer;


	private bool jump;
	private bool hurt;
	private bool bombJump;

	private bool CanRun;
	public GameObject SmokeParticle;

	// Use this for initialization
	void Start () {
		gravity2 = gravity;
		zoom = zoomMin;
		controller = character.GetComponent<CharacterController> ();
		anim = character.GetComponent<Animator> ();
		audiosource = GetComponent<AudioSource> ();

		ActivateTimerToReset = false;
		currentComboState = 0;

		// Store original timer reset duration
		origTimer = currentComboTimer;

		CanRun = true;
		jump = false;
		hurt = false;
		bombJump = false;
	}
	
	// Update is called once per frame
	void Update () {

		/*----------------------------------------------------------------------------------- CAMERA -----------------------------------------------------------------------------*/
		zoom += Input.GetAxis ("Mouse ScrollWheel") * zoomSpeed;

		if (zoom < zoomMin) 
			zoom = zoomMin;
		
		else if (zoom > zoomMax) 
			zoom = zoomMax;
		

		playerCam.transform.localPosition = new Vector3 (0, 0, zoom);

	

			mouseX += Input.GetAxis ("Mouse X");
			mouseY -= Input.GetAxis ("Mouse Y");


		//Bloquer la hauteur de la camera
		mouseY = Mathf.Clamp (mouseY, 0f, 60f);

		playerCam.LookAt (centerPoint);

		//Rotationer le center point dans le sens de la camera
		centerPoint.localRotation = Quaternion.Euler (mouseY, mouseX, 0);


		/*-------------------------------------------------------------------------- PLAYER CONTROLL ---------------------------------------------------------------------------------------------*/
	
		ControlSystem();

		ResetComboState();

		ControlMove ();
	
	
		if(Input.GetButtonDown("Fire1")){
			if (controller.isGrounded) {
				anim.SetTrigger ("Attacking");
			} 
			else{
				anim.Play ("MarioBombJump");
				bombJump = true;
			
			}
		}



	}

	void ResetComboState()
	{
		if (ActivateTimerToReset)
		{
			if (controller.isGrounded) {
				currentComboTimer -= Time.deltaTime;
				if (currentComboTimer <= 0) {
					currentComboState = 0;
					ActivateTimerToReset = false;
					currentComboTimer = origTimer;
				}
			} else {
				
				currentComboTimer = origTimer;
			}
		}
	}

	void ControlMove() {
		moveFB = Input.GetAxis ("Vertical") * moveSpeed;
		moveLR = Input.GetAxis ("Horizontal") * moveSpeed;
		Vector3 movement = new Vector3 (moveLR, verticalVelocity, moveFB);
		movement = character.rotation * movement;

		//transform.localRotation = Quaternion.LookRotation(new Vector3(movement.x, 0.0f, movement.z));
		controller.Move (movement* Time.deltaTime);


		if (Input.GetKey (KeyCode.W)) {
			anim.SetInteger ("direction", 0);

		}
		if (Input.GetKey (KeyCode.D)) {
			anim.SetInteger ("direction", 1);

		}
		if (Input.GetKey (KeyCode.S)) {
			anim.SetInteger ("direction", 2);
		}
		if (Input.GetKey (KeyCode.A)) {
			anim.SetInteger ("direction", 3);

		}

		//Faire suivre le center point au joueur
		centerPoint.position = new Vector3 (character.position.x, character.position.y + mouseYPosition, character.position.z);

		//Diriger le joueur dans le sens de la camera
		if ((Input.GetAxis ("Vertical") >0 | Input.GetAxis ("Vertical") <0)||(Input.GetAxis ("Horizontal") >0 | Input.GetAxis ("Horizontal") <0) ) {
			Quaternion turnAngle = Quaternion.Euler (0, centerPoint.eulerAngles.y, 0);
			character.rotation = Quaternion.Slerp (character.rotation, turnAngle, Time.deltaTime * rotationSpeed);		

		}
		
	


		if (controller.velocity != new Vector3(0,0,0) && controller.isGrounded) {
			anim.SetFloat ("speedMult", moveSpeed/10);
			anim.SetInteger ("State", 1);

		}
		if(Input.GetKey(KeyCode.LeftShift)){
			if (CanRun) {
				moveSpeed = moveSpeed * 1.5f;
				CanRun = false;
			}
		}
			if(Input.GetKeyUp(KeyCode.LeftShift)){
				moveSpeed = moveSpeed / 1.5f;
			CanRun = true;
			}

	}

	void ControlSystem()
	{
		if (controller.isGrounded) {
			gravity = 500;
			verticalVelocity = -gravity * Time.deltaTime;
			if (bombJump) {
				bombJump = false;
				audiosource.PlayOneShot (MarioImpact, 0.5f);
				Instantiate(SmokeParticle,new Vector3(gameObject.transform.position.x,gameObject.transform.position.y+0.5f,gameObject.transform.position.z), Quaternion.Euler(90,0,0));
			}

			if (Input.GetKeyDown (KeyCode.Space) | Input.GetButtonDown("Jump") | jump) {

				//controller l'etat du combo
				if (currentComboState < 3) {
					currentComboState++;
				} else {
					currentComboState = 0;
				}
				ActivateTimerToReset = true;

				anim.SetInteger ("JumpCombo", currentComboState);

				//Definir la force du saut selon l'etat du combo
				switch (currentComboState) {
				case 1:
					anim.Play ("MarioJump");
					audiosource.PlayOneShot (MarioJump1, 0.2f);
					verticalVelocity = jumpForce;
					break;
				case 2:
					anim.Play ("MarioJump2");
					audiosource.PlayOneShot (MarioJump2, 0.2f);
					verticalVelocity = jumpForce + 4;
					break;
				case 3:
					anim.Play ("MarioJump3");
					audiosource.PlayOneShot (MarioJump3, 0.2f);
					verticalVelocity = jumpForce + 8;
					break;				
				}
				

			}else if(hurt){
				
				StartCoroutine(WaitToDisable());
			}
			else {
				anim.SetInteger ("State", 0);
			}
			if (jump) {
				jump = false;
			}

		} else {

			if (!bombJump) {
				gravity = gravity2;
			} else {
				gravity = gravity2*2;
			}

			verticalVelocity -= gravity * Time.deltaTime;
			anim.SetInteger ("State", 2);


		}


	}

	IEnumerator WaitToDisable()
	{
		yield return new WaitForSeconds(0.5f);		
		hurt = false;
	}

	public void Jump(){
		jump = true;

	}
	public void Hurt(){
		hurt = true;
		anim.Play ("MarioHurt");
		audiosource.PlayOneShot (MarioHurt, 0.2f);

	}

}
