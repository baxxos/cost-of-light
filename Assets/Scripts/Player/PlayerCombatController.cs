﻿using System;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour {
    [Tooltip("Reference to the input collector class (component).")]
    public InputCollector inputCollector;
    [Tooltip("Current amount of hitpoints.")]
    public float health = 100;
    [Tooltip("Maximum amount of hitpoints.")]
    public float maxHealth = 100;
    [Tooltip("Damage dealt per one hit.")]
    public float damagePerHit = 100;
    [Tooltip("Radius of each attack.")]
    public float attackRadius = 0.5f;
    [Tooltip("Reference to the game object used for calculating collisions when dealing damage (e.g. weapon)")]
    public Transform attackPoint;

    public event Action<float, float, float> OnPlayerHealthChanged;
    public event Action OnPlayerHealthZero;

    private bool dealingDamage = false;
    private Animator animator;
    private AudioSource audioSource;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        DealDamageIfAttacking();
    }

    void OnEnable()
    {
        inputCollector.OnAttack += Attack;
    }

    void OnDisable()
    {
        inputCollector.OnAttack -= Attack;
    }

    private void Attack()
    {
        // Trigger the attack animation during which the attack point gets activated
        animator.SetTrigger("attack");
    }

    private void DealDamageIfAttacking()
    {
        if (attackPoint.gameObject.activeSelf && !dealingDamage)
        {
            dealingDamage = true;
            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius);
            audioSource.PlayOneShot(audioSource.clip);

            foreach (Collider2D hitObject in hitObjects)
            {
                if (hitObject.gameObject == gameObject)
                {
                    continue;
                }
                else if (!hitObject.isTrigger && hitObject.isActiveAndEnabled)
                {
                    hitObject.GetComponent<EnemyCombatController>().DecreaseHealth(damagePerHit);
                }
            }
        }
        else if (!attackPoint.gameObject.activeSelf && dealingDamage)
        {
            dealingDamage = false;
        }
    }

    public void DecreaseHealth(float amount)
    {
        if (health - amount <= 0)
        {
            if (OnPlayerHealthZero != null)
            {
                OnPlayerHealthZero();
            }

            health = 0;
            return;
        }
        else
        {
            health -= amount;
        }
        
        // Notify subscribers about the damage, current health and maximum health
        if (OnPlayerHealthChanged != null)
        {
            OnPlayerHealthChanged(-amount, health, maxHealth);
        }
    }
}
