using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatController : MonoBehaviour {
    [Tooltip("The starting amount of health.")]
    public float health = 100;
    [Tooltip("Maximum amount of health.")]
    public float maxHealth = 100;
    [Tooltip("Minimum amount of time spent in the same state.")]
    public float stateSwitchTimeMin = 3f;
    [Tooltip("Maximum amount of time spent in the same state.")]
    public float stateSwitchTimeMax = 10f;
    [Tooltip("Reference to the sprite which indicates the enemy state.")]
    public SpriteRenderer stateIndicator;

    private bool isInvincible = true;

    // Use this for initialization
    void Start () {
        Invoke("SwitchStates", Random.Range(stateSwitchTimeMin, stateSwitchTimeMax));
    }
	
	// Update is called once per frame
	void Update () {
		if (isInvincible)
        {
            // Debug.Log("Enemy CAN BE attacked now");
        }
        else
        {
            // Debug.Log("Enemy CANNOT BE attacked now");
        }
	}

    private void SwitchStates()
    {
        isInvincible = !isInvincible;

        /* Hide the state indicator by setting its alpha to 0 when the enemy is not invincible. 
        We don't disable/deactivate the object or its components in order to keep the invokes running. */
        var spriteColor = stateIndicator.color;
        spriteColor.a = (isInvincible ? 1f : 0f);
        stateIndicator.color = spriteColor;

        // Switch state periodically in random intervals of <x; y> seconds.
        Invoke("SwitchStates", Random.Range(stateSwitchTimeMin, stateSwitchTimeMax));
    }

    public void DecreaseHealth(float amount)
    {
        if (isInvincible)
        {
            return;
        }
        else if (health - amount <= 0)
        {
            gameObject.SetActive(false);
            return;
        }

        health -= amount;
    }
}
