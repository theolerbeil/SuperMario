using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour {

	public float Speed = 5f;
	public float JumpHeight = 2f;
	public float GroundDistance = 0.2f;
	public float DashDistance = 5f;
	public LayerMask Ground;

	private Rigidbody _body;
	private Vector3 _inputs;
	private bool _isGrounded = true;
	private Transform _groundChecker;
	public Transform cam;

	void Start()
	{
		_body = GetComponent<Rigidbody>();
		_groundChecker = transform.GetChild(0);
	    _inputs = Vector3.zero;
	}

	void Update()
	{
		_isGrounded = Physics.CheckSphere(_groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);

		_inputs.x = Input.GetAxis("Horizontal");
		_inputs.z = Input.GetAxis("Vertical");
		if (_inputs != Vector3.zero) {
            transform.localEulerAngles = new Vector3(0, cam.eulerAngles.y, 0);
            transform.Translate(Vector3.forward * Speed * Time.deltaTime);
            // transform.forward = _inputs;
        }

        if (Input.GetButtonDown("Jump") && _isGrounded)
		{
		Debug.Log ("Saute");
			_body.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
		}
		/*if (Input.GetButtonDown("Fire1"))
		{
		Debug.Log ("Dash");
			Vector3 dashVelocity = Vector3.Scale(transform.forward, DashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * _body.drag + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * _body.drag + 1)) / -Time.deltaTime)));
			_body.AddForce(dashVelocity, ForceMode.VelocityChange);
		}*/
	}


	void FixedUpdate()
	{

		_body.MovePosition(_body.position + _inputs * Speed * Time.fixedDeltaTime);
	}
}












	/*public float speed;
	public float rotationSpeed;
	public GameObject cam;
	private Rigidbody rb;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		float x = Input.GetAxis("Horizontal") * Time.deltaTime * rotationSpeed;
		float z = Input.GetAxis("Vertical") * Time.deltaTime * speed;

	

		transform.Translate(0, 0, z);
		transform.Rotate(0, x, 0);

	/*
		float angle = 90;
		if (Vector3.Angle (transform.forward, cam.transform.position - transform.position) < angle) {
			
		} else {
			
		}
	}
}*/
