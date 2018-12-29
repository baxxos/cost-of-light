
using System;
using UnityEngine;

public class CombatController : MonoBehaviour {
    public float health = 100;
    public float maxHealth = 100;
    public event Action<float, float> OnPlayerHealthChanged;
    public event Action OnPlayerHealthZero;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DecreaseHealth(float amount)
    {
        if (health - amount <= 0)
        {
            if (OnPlayerHealthZero != null)
            {
                OnPlayerHealthZero();
            }

            return;
        }

        health -= amount;

        // Notify subscribers about health level decrease
        if (OnPlayerHealthChanged != null)
        {
            OnPlayerHealthChanged(health, maxHealth);
        }
    }

    public void IncreaseHealth(float amount)
    {

    }
}
