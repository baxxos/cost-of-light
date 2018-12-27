using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    [Tooltip("Speed at which the enemy approaches the player.")]
    public float approachSpeed;
    [Tooltip("Distance between player object and the enemy.")]
    public float approachDistance;
    [Tooltip("Speed at which the enemy follows the player.")]
    public float followSpeed;
    [Tooltip("How long (horizontal distance) should the enemy follow the player.")]
    public float followRange;

    private enum EnemyState { approaching, attacking, following, returning, idle };
    private EnemyState currentState;
    private Rigidbody2D rb2d;
    private GameObject playerObject;
    private SpriteRenderer spriteRenderer;
    private Vector2 spawnCoords;

	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        playerObject = GameObject.FindGameObjectWithTag("Player");

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
        if ((currentState == EnemyState.idle) && (col.gameObject.tag == "Player"))
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
        // TODO: flip X
        float tolerance = .15f;

        // Check if the spawn position has been reached  (ignore the vertical position since we can't do anything about it)
        if ((transform.position.x >= (spawnCoords.x - tolerance)) && (transform.position.x <= (spawnCoords.x + tolerance)))
        {
            StopMoving();
            currentState = EnemyState.idle;
        }
        else
        {
            // Keep a fixed velocity while retreating
            var velocity = rb2d.velocity;
            velocity.x = (transform.position.x < spawnCoords.x ? followSpeed : -followSpeed);
            rb2d.velocity = velocity;
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
        var distance = transform.position.x - player.transform.position.x;
        float directionHorizontal = (distance > 0f ? -1f : 1f);  // 1 when going right and -1 when going left

        if (directionHorizontal > 0 && spriteRenderer.flipX)
        {
            spriteRenderer.flipX = false;
        }
        else if (directionHorizontal < 0 && !spriteRenderer.flipX)
        {
            spriteRenderer.flipX = true;
        }

        transform.position = Vector2.MoveTowards(
            transform.position, 
            player.transform.position,
            approachSpeed * Time.deltaTime
        );

        if (Math.Abs(distance) <= approachDistance)
        {
            currentState = EnemyState.following;
            // TODO: attacking instead of following (follow only when player walks away)
        }
    }
}
