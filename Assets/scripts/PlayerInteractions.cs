using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractions : MonoBehaviour {
	public Text CoinText;
	public Text VieText;
	public Image[] vieImg;
	public int Coin;
	public int vie = 8;
	public AudioClip CoinSound;
	AudioSource audiosource;

	public GameObject AttackTrigger;
	// Use this for initialization
	void Start () {
		audiosource = GetComponent<AudioSource> ();
		AttackTrigger.SetActive(false);
		for(int i = 0; i < vieImg.Length; i++){
			vieImg [i].enabled = false;
		}
		vieImg [vie].enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		CoinText.text = "x" + Coin;
		VieText.text = "Vie : " + vie;

		if(Input.GetButtonDown("Fire1")){
			

			StartCoroutine(WaitToDisable());
		}

	}
	void OnTriggerEnter(Collider other) {
		if (other.tag == "coin") {
			audiosource.PlayOneShot (CoinSound, 0.1f);
			Coin++;
			Destroy (other.gameObject);
		}

	}

	IEnumerator WaitToDisable()
	{
		yield return new WaitForSeconds(0f);		
		AttackTrigger.SetActive(true);
		yield return new WaitForSeconds(0.2f);
		AttackTrigger.SetActive(false);
	}
	public void ActualiserVie(int value){
		vie += value;
		for(int i = 0; i < vieImg.Length; i++){
			vieImg [i].enabled = false;
		}
		vieImg [vie].enabled = true;
	}
}
