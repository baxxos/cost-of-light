using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatController : MonoBehaviour {
    [Tooltip("The starting amount of health.")]
    public float health = 100;
    [Tooltip("Maximum amount of health.")]
    public float maxHealth = 100;
    [Tooltip("Damage dealt per one hit.")]
    public float damagePerHit = 10;
    [Tooltip("Time (seconds) between two subsequent attacks.")]
    public float attackCooldown = 1;
    [Tooltip("Minimum amount of time spent in the same state.")]
    public float stateSwitchTimeMin = 3f;
    [Tooltip("Maximum amount of time spent in the same state.")]
    public float stateSwitchTimeMax = 10f;
    [Tooltip("Reference to the sprite which indicates the enemy state.")]
    public SpriteRenderer stateIndicator;

    private bool isInvincible = true;
    private GameObject playerObject;
    private Animator animator;
    private EnemyStateManager stateManager;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        stateManager = GetComponent<EnemyStateManager>();

        playerObject = GameObject.FindGameObjectWithTag("SpritePlayer");
        Invoke("SwitchStates", Random.Range(stateSwitchTimeMin, stateSwitchTimeMax));
    }
	
	// Update is called once per frame
	void Update () {

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

    public void AttackPlayer()
    {
        stateManager.CurrentState = EnemyStateManager.EnemyState.attacking;

        // Triggers the attack animation which triggers the related enemy combat controller action
        animator.SetTrigger("attack");
    }

    public void DealDamageToPlayer()
    {
        playerObject.GetComponent<PlayerCombatController>().DecreaseHealth(damagePerHit);
    }

    public void DecreaseHealth(float amount)
    {
        if (isInvincible)
        {
            return;
        }
        else if (health - amount <= 0)
        {
            stateManager.CancelAttack();
            gameObject.SetActive(false);
            return;
        }

        animator.SetTrigger("damageTaken");
        health -= amount;
    }
}
