using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goomba : MonoBehaviour {
	public GameObject Player;
	public GameObject Eyes;
	public int DistToReturn;
	public float rotationSpeed;
	public float moveSpeed;
	private Animator anim;
	public bool isDead;
	public AudioClip impact,ecrase;
	AudioSource audiosource;
	public GameObject SmokeParticle;
	// Use this for initialization
	void Start () {
		Eyes.SetActive (false);
		Player = GameObject.FindWithTag ("Player");
		anim = gameObject.GetComponent<Animator> ();
		isDead = false;
		audiosource = GetComponent<AudioSource> ();

	}
	
	// Update is called once per frame
	void Update () {
		
		if (!isDead) {
			if (Vector3.Distance (transform.position, Player.transform.position) <= DistToReturn) {
				anim.SetInteger ("state", 1);
				Quaternion rotationAngle = Quaternion.LookRotation (Player.transform.position - transform.position); // we get the angle has to be rotated
				transform.rotation = Quaternion.Slerp (transform.rotation, rotationAngle, Time.deltaTime * rotationSpeed); // we rotate the rotationAngle 
				transform.localEulerAngles = new Vector3 (0, transform.localEulerAngles.y, 0); //bloquer sur l'axe y


				transform.position += transform.forward * moveSpeed * Time.deltaTime;
			} else {
				anim.SetInteger ("state", 0);
			}
		}
			
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "punch") {
			isDead = true;
			Eyes.SetActive (true);
			anim.SetInteger ("state", 2);
			GetComponent<Rigidbody>().AddForce(other.transform.forward * 200f);
			GetComponent<Rigidbody>().AddForce(other.transform.right * 200f);
			audiosource.PlayOneShot (impact, 0.3f);
			StartCoroutine(WaitToDestroy(2));
		}
		if (other.tag == "Player") {
			
			Player.GetComponent<TPCharacterController> ().Jump();
			Eyes.SetActive (true);
			isDead = true;
			audiosource.PlayOneShot (ecrase, 0.2f);
			anim.SetInteger ("state", 3);
			StartCoroutine(WaitToDestroy(1));
		}
	}
	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.tag == "Player")
		{
			Player.GetComponent<PlayerInteractions> ().ActualiserVie (-1);
			Player.GetComponent<TPCharacterController> ().Hurt();
			GetComponent<Rigidbody>().AddForce(transform.up * 100);
			GetComponent<Rigidbody>().AddForce(transform.forward * -500);

		}
	}
	IEnumerator WaitToDestroy(float time)
	{
		yield return new WaitForSeconds(time);	
		Instantiate(SmokeParticle,new Vector3(gameObject.transform.position.x,gameObject.transform.position.y+0.5f,gameObject.transform.position.z), Quaternion.Euler(90,0,0));
		Destroy (this.gameObject);
	}

}
