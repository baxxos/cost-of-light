﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Tooltip("Walking speed of the player character.")]
    public float walkingSpeed;

    private bool flippedLeft = false;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Animator animator;

    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleKeyControls();

        // To determine if the walking animation should be played
        animator.SetFloat("horizontalMovement", Math.Abs(rb.velocity.x));
    }

    private void HandleKeyControls()
    {
        if ((!flippedLeft && Input.GetKeyDown(KeyCode.A)) || (flippedLeft && Input.GetKeyDown(KeyCode.D)))
        {
            FlipCharacter();
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            var velocity = rb.velocity;
            velocity.x = (flippedLeft ? -walkingSpeed : walkingSpeed);
            rb.velocity = velocity;
        }

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void FlipCharacter()
    {
        /* Instead of flipping the sprite (which does not actually flip its skeleton?!?),
         * we have to reverse the horizontal local scale so the bone animations will work properly */
        Vector3 newLocalScale = transform.localScale;
        newLocalScale.x *= -1;
        transform.localScale = newLocalScale;

        flippedLeft = !flippedLeft;
    }
}
