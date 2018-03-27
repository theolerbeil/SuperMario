using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class interactable : MonoBehaviour {
	public Image dialogueBox;
	public Text dialogueTitle;
	public Text dialogueText;
	public string titre;
	public string dialogue;
	// Use this for initialization
	void Start () {
		dialogueBox.enabled = false;
		dialogueTitle.enabled = false;
		dialogueText.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay(Collider other) {
		if (other.tag == "Player") {
			if (Input.GetKeyDown (KeyCode.E)) {
				dialogueTitle.text = titre;
				dialogueText.text = dialogue;
				dialogueBox.enabled =!dialogueBox.enabled;
				dialogueTitle.enabled = !dialogueTitle.enabled;;
				dialogueText.enabled = !dialogueText.enabled;;
			}
		}
	}
	void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {

			dialogueBox.enabled = false;
			dialogueTitle.enabled = false;
			dialogueText.enabled = false;
		}
	}
}
