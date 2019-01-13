using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour {
    // List of available enemy states and a property holding the current one
    public enum EnemyState { approaching, attacking, following, returning, idle };
    public EnemyState CurrentState { get; set; }

    private Animator animator;
    private GameObject playerObject;
    private EnemyCombatController combatController;
    private EnemyMovementController movementController;

	// Use this for initialization
	void Start()
    {
        animator = GetComponent<Animator>();
        combatController = GetComponent<EnemyCombatController>();
        movementController = GetComponent<EnemyMovementController>();
        playerObject = GameObject.FindGameObjectWithTag("SpritePlayer");

        CurrentState = EnemyState.idle;
    }
	
	// Update is called once per frame
	void Update()
    {
        switch (CurrentState)
        {
            case EnemyState.approaching:
                movementController.ApproachPlayer(playerObject);
                break;
            case EnemyState.following:
                movementController.FollowPlayer(playerObject);
                break;
            case EnemyState.returning:
                movementController.ReturnToSpawnCoords();
                break;
            case EnemyState.attacking:
                PerformAttackOrFollow(playerObject);
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if ((CurrentState == EnemyState.idle) && (col.gameObject.tag == "SpritePlayer"))
        {
            // Play approaching animation (only once) and change state respectively
            animator.SetTrigger(
                (transform.position.x < playerObject.transform.position.x ? "approachingRight" : "approachingLeft")
            );

            CurrentState = EnemyState.approaching;
        }
    }

    private void PerformAttackOrFollow(GameObject playerObject)
    {
        // Trigger attack if the player is in the approach distance, otherwise follow
        if ((Vector2.Distance(transform.position, playerObject.transform.position) <= movementController.approachDistance))
        {
            if (!IsInvoking("PerformAttack"))
            {
                Invoke("PerformAttack", combatController.attackCooldown);
            }
        }
        else
        {
            CancelAttack();
            CurrentState = EnemyState.following;
        }
    }

    public void PerformAttack()
    {
        combatController.AttackPlayer();
    }

    public void CancelAttack()
    {
        CancelInvoke("PerformAttack");
    }
}
