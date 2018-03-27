using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bruiteurpas : MonoBehaviour {

	public Transform smokeEffect;
	private bool isTriggered;
	public AudioClip pas;
	AudioSource audiosource;

	// Use this for initialization
	void Start () {
		audiosource = GetComponent<AudioSource> ();
	
	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other){

		if (!isTriggered && other.tag.Equals ("terrain")) {
			audiosource.pitch = Random.Range(0.8f, 1.2f);
			audiosource.PlayOneShot (pas, 0.5f);
			if (smokeEffect) {
				Instantiate (smokeEffect, new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y + 0.1f, gameObject.transform.position.z), Quaternion.Euler (90, 0, 0));
			}
		}

	}

	void OnTriggerExit(Collider other){

		if (isTriggered && other.tag.Equals ("terrain")) {
			isTriggered = false;
		}
	}
}
