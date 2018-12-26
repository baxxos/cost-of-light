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

    private bool approaching = false;
    private GameObject playerObject;
    private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
		if (approaching)
        {
            ApproachPlayer(playerObject);
        }
	}

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            approaching = true;
        }
    }

    private void FollowPlayer(GameObject player)
    {
        // TODO: this
    }

    private void ApproachPlayer(GameObject player)
    {
        var currentPosition = transform.position;
        var distance = currentPosition.x - player.transform.position.x;
        float directionHorizontal = (distance > 0f ? -1f : 1f);  // Equal to 1 when going right and -1 when going left

        transform.position = new Vector2(
            currentPosition.x + (directionHorizontal * approachSpeed * Time.deltaTime),
            currentPosition.y
        );

        if (directionHorizontal > 0 && spriteRenderer.flipX)
        {
            spriteRenderer.flipX = false;
        }
        else if (directionHorizontal < 0 && !spriteRenderer.flipX)
        {
            spriteRenderer.flipX = true;
        }

        if (Math.Abs(distance) <= Math.Abs(approachDistance))
        {
            approaching = false;
        }
    }
}
