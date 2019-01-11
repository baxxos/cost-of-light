using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrapController : MonoBehaviour {
    private GameObject playerObject;
    private AudioSource audioSource;

	// Use this for initialization
	void Start () {
        playerObject = GameObject.FindGameObjectWithTag("SpritePlayer");
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {}

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.tag == "SpritePlayer")
        {
            audioSource.PlayOneShot(audioSource.clip);
            playerObject.GetComponent<PlayerCombatController>().DecreaseHealth(25);
        }
    }
}
