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

    private bool returning = false;
    private bool following = false;
    private bool approaching = false;
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
    }
	
	// Update is called once per frame
	void Update () {
		if (approaching)
        {
            ApproachPlayer(playerObject);
        }
        else if (following)
        {
            FollowPlayer(playerObject);
        }
	}

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!following && col.gameObject.tag == "Player")
        {
            approaching = true;
        }
    }

    private void FollowPlayer(GameObject player)
    {
        // TODO: simplify
        var targetPosition = new Vector2(player.transform.position.x, transform.position.y);

        if (transform.position.x < targetPosition.x)
        {
            targetPosition.x -= approachDistance;
        }
        else
        {
            targetPosition.x += approachDistance;
        }

        if (Math.Abs(transform.position.x - spawnCoords.x) <= followRange)
        {
            // Debug.Log(Math.Abs(transform.position.x - targetPosition.x));
            if (Math.Abs(transform.position.x - targetPosition.x) > approachDistance)
            {
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    targetPosition,
                    followSpeed * Time.deltaTime
                );
            }
        }
        else
        {
            // TODO: returning method
            transform.position = Vector2.MoveTowards(
                transform.position,
                spawnCoords,
                followSpeed * Time.deltaTime
            );
        }

        // Stop following
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
            approaching = false;
            following = true;
            // TODO: attacking instead of following (follow only when player walks away)
        }
    }
}
