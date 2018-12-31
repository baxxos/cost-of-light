using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatController : MonoBehaviour {
    public float health;
    public float maxHealth;
    private bool isInvincible = true;

    // Use this for initialization
    void Start () {
        Invoke("SwitchStates", 0.5f);
    }
	
	// Update is called once per frame
	void Update () {
		if (isInvincible)
        {
            Debug.Log("Enemy CAN BE attacked now");
        }
        else
        {
            Debug.Log("Enemy CANNOT BE attacked now");
        }
	}

    private void SwitchStates()
    {
        isInvincible = !isInvincible;
        Invoke("SwitchStates", Random.Range(0.1f, 0.5f));
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
