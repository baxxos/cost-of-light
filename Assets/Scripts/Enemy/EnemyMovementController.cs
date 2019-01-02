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
    [Tooltip("Reference to the actual sprite of the enemy character.")]
    public SpriteRenderer spriteRenderer;

    private enum EnemyState { approaching, attacking, following, returning, idle };
    private EnemyState currentState;
    private Rigidbody2D rb2d;
    private GameObject playerObject;
    private Vector2 spawnCoords;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        playerObject = GameObject.FindGameObjectWithTag("SpritePlayer");

        spawnCoords = transform.position;
        currentState = EnemyState.idle;
    }
	
	// Update is called once per frame
	void Update () {
        switch (currentState)
        {
            case EnemyState.approaching:
                ApproachPlayer(playerObject);
                break;
            case EnemyState.following:
                FollowPlayer(playerObject);
                break;
            case EnemyState.returning:
                ReturnToSpawnCoords();
                break;
            default:
                break;
        }
	}

    private void OnTriggerEnter2D(Collider2D col)
    {
        if ((currentState == EnemyState.idle) && (col.gameObject.tag == "SpritePlayer"))
        {
            currentState = EnemyState.approaching;
        }
    }

    private void StopMoving()
    {
        rb2d.velocity = new Vector2(0, rb2d.velocity.y);
    }

    private void ReturnToSpawnCoords()
    {
        float tolerance = .15f;

        // Check if the spawn position has been reached  (ignore the vertical position since we can't do anything about it)
        if ((transform.position.x >= (spawnCoords.x - tolerance)) && (transform.position.x <= (spawnCoords.x + tolerance)))
        {
            StopMoving();
            currentState = EnemyState.idle;
            spriteRenderer.flipX = false;
        }
        else
        {
            // Keep a fixed velocity while retreating
            var velocity = rb2d.velocity;
            velocity.x = (transform.position.x < spawnCoords.x ? followSpeed : -followSpeed);
            rb2d.velocity = velocity;

            // Flip the sprite according to the movement direction
            spriteRenderer.flipX = (velocity.x < 0);
        }
    }

    private void FollowPlayer(GameObject player)
    {
        if (Math.Abs(transform.position.x - spawnCoords.x) <= followRange)
        {
            // Keep a fixed velocity while following the player
            var velocity = rb2d.velocity;
            velocity.x = (transform.position.x < player.transform.position.x ? followSpeed : -followSpeed);
            rb2d.velocity = velocity;

            // If we catch the player, set the velocity to 0 and start attacking
            if (Math.Abs(transform.position.x - player.transform.position.x) <= approachDistance)
            {
                StopMoving();
                // currentState = EnemyState.attacking;
            }
        }
        else
        {
            currentState = EnemyState.returning;
        }
    }

    private void ApproachPlayer(GameObject player)
    {
        var velocity = rb2d.velocity;
        velocity.x = (transform.position.x < player.transform.position.x ? approachSpeed : -approachSpeed);
        rb2d.velocity = velocity;

        // Flip the sprite according to the movement direction
        var distance = transform.position.x - player.transform.position.x;
        spriteRenderer.flipX = (distance > 0f);

        if (Math.Abs(distance) <= approachDistance)
        {
            StopMoving();
            currentState = EnemyState.following;
            // TODO: attacking instead of following (follow only when player walks away)
        }
    }
}
