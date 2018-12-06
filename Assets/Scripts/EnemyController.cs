using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    public GameObject playerObject;
    public float rangeOfView;
    public float approachSpeed;
    public float approachDistance;

    private SpriteRenderer spriteRenderer;
    private bool approaching;

	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (approaching)
        {
            ApproachPlayer(playerObject);
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        approaching = true;
    }

    private void FollowPlayer(GameObject player)
    {
        // TODO: this
    }

    private void ApproachPlayer(GameObject player)
    {
        var currentPosition = transform.position;
        var distance = currentPosition.x - player.transform.position.x;
        float approachDirectionX = (distance > 0f ? -1f : 1f);  // Equal to 1 when going right and -1 when going left

        transform.position = new Vector2(
            currentPosition.x + (approachDirectionX * approachSpeed * Time.deltaTime),
            currentPosition.y
        );

        if (approachDirectionX > 0 && spriteRenderer.flipX)
        {
            spriteRenderer.flipX = false;
        }
        else if (approachDirectionX < 0 && !spriteRenderer.flipX)
        {
            spriteRenderer.flipX = true;
        }

        if (Math.Abs(distance) < Math.Abs(approachDistance))
        {
            approaching = false;
        }
    }
}
