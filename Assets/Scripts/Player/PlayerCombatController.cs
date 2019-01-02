
using System;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour {
    public float health = 100;
    public float maxHealth = 100;
    public float damagePerHit = 100;
    public float attackRadius = 0.15f;
    public Transform attackPoint;
    public event Action<float, float> OnPlayerHealthChanged;
    public event Action OnPlayerHealthZero;

    private bool dealingDamage = false;
    private Animator animator;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        HandleMouseControls();
        DealDamageIfAttacking();
    }

    private void HandleMouseControls()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("attack");
        }
    }

    private void DealDamageIfAttacking()
    {
        if (attackPoint.gameObject.activeSelf && !dealingDamage)
        {
            dealingDamage = true;
            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius);

            foreach (Collider2D hitObject in hitObjects)
            {
                if (hitObject.gameObject == gameObject)
                {
                    continue;
                }
                else if (!hitObject.isTrigger)
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
        // TODO
    }

    private void DealDamage()
    {

    }
}
