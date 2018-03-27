using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bowser : MonoBehaviour {
	public Transform Player;
	public float moveSpeed;
	public float rotationSpeed;
	public int MaxDist;
	public int MinDist;
	private Animator anim;
	public int State;
	// Use this for initialization
	void Start () {
		State = 1;
		anim = gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (State == 1) {
			anim.SetInteger ("State", 1);
			/*---------- Rotation de Bowser -------------*/
			Quaternion rotationAngle = Quaternion.LookRotation (Player.position - transform.position); // we get the angle has to be rotated
			transform.rotation = Quaternion.Slerp (transform.rotation, rotationAngle, Time.deltaTime * rotationSpeed); // we rotate the rotationAngle 
			transform.localEulerAngles = new Vector3 (0, transform.localEulerAngles.y, 0); //bloquer sur l'axe y

			/*---------- Deplacement de Bowser -------------*/
			if (Vector3.Distance (transform.position, Player.position) >= MinDist) {
				transform.position += transform.forward * moveSpeed * Time.deltaTime;
			} else {
				State = 2;
			}
				

		}
		if (State == 2) {
			anim.SetInteger ("State", 2);
			if (Vector3.Distance (transform.position, Player.position) >= MinDist) {
				State = 1;
			}

		}
	}
}
