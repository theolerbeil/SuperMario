using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toad : MonoBehaviour {
	public GameObject Player;
	public int DistToReturn;
	public float rotationSpeed;
	private Animator anim;
	private bool CanCoucou;
	public AudioClip toadSalut;
	public AudioClip toadHurt,toadHurt1,toadHurt2;
	public AudioClip impact;

	AudioSource audiosource;
	// Use this for initialization
	void Start () {
		Player = GameObject.FindWithTag ("Player");
		anim = gameObject.GetComponent<Animator> ();
		CanCoucou = true;
		audiosource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance (transform.position, Player.transform.position) <= DistToReturn) {
			Quaternion rotationAngle = Quaternion.LookRotation (Player.transform.position - transform.position); // we get the angle has to be rotated
			transform.rotation = Quaternion.Slerp (transform.rotation, rotationAngle, Time.deltaTime * rotationSpeed); // we rotate the rotationAngle 
			transform.localEulerAngles = new Vector3 (0, transform.localEulerAngles.y, 0); //bloquer sur l'axe y
			if (CanCoucou) {
				audiosource.PlayOneShot (toadSalut, 1f);
				anim.Play ("toadCoucou");
				CanCoucou = false;
			}
		} else {
			if (!CanCoucou) {

				CanCoucou = true;
			}
		}
	}


	void OnTriggerEnter(Collider other) {
		if (other.tag == "punch") {
			int hurtSound = Random.Range (0,3);
			switch(hurtSound){
			case 0:
				audiosource.PlayOneShot (toadHurt, 0.6f);
				break;
			case 1:
				audiosource.PlayOneShot (toadHurt1, 0.6f);
				break;
			case 2:
				audiosource.PlayOneShot (toadHurt2, 0.6f);
				break;
			}
			audiosource.PlayOneShot (impact, 0.3f);
			anim.Play ("toadHust");
		}
	}


}
