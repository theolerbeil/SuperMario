using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerLogic : MonoBehaviour {
	[SerializeField]
	private Animator animator;
	[SerializeField]
	private float DirectionDampTime = .25f;

	private float speed = 0.0f;
	private float h = 0.0f;
	private float v = 0.0f;
		
	// Use this for initialization
	void Start () {
		animator = gameObject.GetComponent<Animator> ();
		if (animator.layerCount >= 2) {
			animator.SetLayerWeight (1, 1);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (animator) {
			v = Input.GetAxis ("Vertical");
			h = Input.GetAxis ("Horizontal");
			speed = new Vector2 (h, v).sqrMagnitude;

			animator.SetFloat ("Speed",speed);
			animator.SetFloat ("Direction",h,DirectionDampTime,Time.deltaTime);

			if (Input.GetKey (KeyCode.W)) {
				transform.Translate(Vector3.forward *speed*2* Time.deltaTime);
			}
			if (Input.GetKey (KeyCode.D)) {
				transform.Rotate(Vector3.up *speed*10* Time.deltaTime);
			}
			if (Input.GetKey (KeyCode.S)) {
				transform.Translate(Vector3.forward *speed*2* Time.deltaTime);
			}
			if (Input.GetKey (KeyCode.A)) {
				transform.Translate(Vector3.forward *speed*2* Time.deltaTime);
			}
		}
	}
}
