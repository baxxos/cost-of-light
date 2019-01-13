using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyMovementController : MonoBehaviour {
    [Tooltip("Speed at which the enemy approaches the player.")]
    public float approachSpeed;
    [Tooltip("Distance between player object and the enemy.")]
    public float approachDistance;
    [Tooltip("Speed at which the enemy follows the player.")]
    public float followSpeed;
    [Tooltip("How long (horizontal distance) should the enemy follow the player.")]
    public float followRange;
    [Tooltip("Speed at which the enemy retreats when the player is out of follow range.")]
    public float returnSpeed;
    [Tooltip("Reference to the sprite of the enemy character.")]
    public SpriteRenderer spriteRenderer;

    private Rigidbody2D rb2d;
    private Vector2 spawnCoords;
    private EnemyStateManager stateManager;
    private EnemyCombatController combatController;

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        stateManager = GetComponent<EnemyStateManager>();
        combatController = GetComponent<EnemyCombatController>();

        spawnCoords = transform.position;
    }
	
	// Update is called once per frame
	void Update() {}

    private void StopMoving()
    {
        rb2d.velocity = new Vector2(0, rb2d.velocity.y);
    }

    public void ReturnToSpawnCoords()
    {
        float tolerance = .15f;

        // Check if the spawn position has been reached  (ignore the vertical position since we can't do anything about it)
        if ((transform.position.x >= (spawnCoords.x - tolerance)) && (transform.position.x <= (spawnCoords.x + tolerance)))
        {
            StopMoving();
            stateManager.CurrentState = EnemyStateManager.EnemyState.idle;
            spriteRenderer.flipX = false;
        }
        else
        {
            // Keep a fixed velocity while retreating
            var velocity = rb2d.velocity;
            velocity.x = (transform.position.x < spawnCoords.x ? returnSpeed : -returnSpeed);
            rb2d.velocity = velocity;

            // Flip the sprite according to the movement direction
            spriteRenderer.flipX = (velocity.x < 0);
        }
    }

    public void FollowPlayer(GameObject player)
    {
        if (Math.Abs(transform.position.x - spawnCoords.x) <= followRange)
        {
            // Keep a fixed velocity while following the player
            var velocity = rb2d.velocity;
            velocity.x = (transform.position.x < player.transform.position.x ? followSpeed : -followSpeed);
            rb2d.velocity = velocity;

            // If we catch the player stop moving and start attacking
            if (Math.Abs(transform.position.x - player.transform.position.x) <= approachDistance)
            {
                StopMoving();
                combatController.AttackPlayer();
            }
        }
        else
        {
            stateManager.CurrentState = EnemyStateManager.EnemyState.returning;
        }
    }

    public void ApproachPlayer(GameObject player)
    {
        // Generate velocity
        var velocity = rb2d.velocity;
        velocity.x = (transform.position.x < player.transform.position.x ? approachSpeed : -approachSpeed);
        rb2d.velocity = velocity;

        // Flip the sprite according to the movement direction
        var distance = transform.position.x - player.transform.position.x;
        spriteRenderer.flipX = (distance > 0f);

        if (Math.Abs(distance) <= approachDistance)
        {
            StopMoving();
            combatController.AttackPlayer();
        }
    }
}
