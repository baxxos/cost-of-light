using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour {
    public PlayerCombatController combatController;

    // Use this for initialization
    void Start()
    {
        transform.localScale = new Vector2(
            combatController.health / combatController.maxHealth,
            transform.localScale.y
        );
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        combatController.OnPlayerHealthChanged += HandleHealthChange;
        combatController.OnPlayerHealthZero += HandleHealthZero;
    }

    void OnDisable()
    {
        combatController.OnPlayerHealthChanged -= HandleHealthChange;
        combatController.OnPlayerHealthZero -= HandleHealthZero;
    }

    private void HandleHealthChange(float healthChange, float currentHealth, float maxHealth)
    {
        transform.localScale = new Vector2(currentHealth / maxHealth, transform.localScale.y);
    }

    private void HandleHealthZero()
    {
        transform.localScale = new Vector2(0, transform.localScale.y);
    }
}
