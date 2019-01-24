using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrapController : MonoBehaviour {
    [Tooltip("Amount of damage dealt to player after he triggers the trap.")]
    public int damageDealt = 25;

    private GameObject playerObject;
    private PlayerCombatController playerCombatController;
    private AudioSource audioSource;

	// Use this for initialization
	void Start () {
        playerObject = GameObject.FindGameObjectWithTag("SpritePlayer");
        playerCombatController = playerObject.GetComponent<PlayerCombatController>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {}

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.tag == "SpritePlayer")
        {
            audioSource.PlayOneShot(audioSource.clip);
            playerCombatController.DecreaseHealth(damageDealt);
        }
    }
}
