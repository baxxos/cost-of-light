﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Tooltip("Reference to the input collector class (component).")]
    public InputCollector inputCollector;
    [Tooltip("Walking speed of the player character.")]
    public float walkingSpeed;
    [Tooltip("Running speed of the player character. Should be higher than the walking speed.")]
    public float runningSpeed = 8;
    [Tooltip("Velocity generated on jump.")]
    public float jumpVelocity = 12;

    private bool flippedLeft = false;
    private bool isRunning = false;
    private bool isJumping = false;
    private Rigidbody2D rb2d;
    private Animator animator;

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // To determine which movement animations should be played
        animator.SetFloat("horizontalMovement", rb2d.velocity.x);
        animator.SetFloat("verticalMovement", rb2d.velocity.y);
    }

    void OnEnable()
    {
        // Subscribe to player input actions
        inputCollector.OnTurnLeft += TurnLeft;
        inputCollector.OnTurnRight += TurnRight;

        inputCollector.OnMove += Move;
        inputCollector.OnStopMoving += StopMoving;

        inputCollector.OnStartRunning += StartRunning;
        inputCollector.OnStopRunning += StopRunning;

        inputCollector.OnJump += Jump;
    }

    void OnDisable()
    {
        // Unsubscribe from player input actions
        inputCollector.OnTurnLeft -= TurnLeft;
        inputCollector.OnTurnRight -= TurnRight;

        inputCollector.OnMove -= Move;
        inputCollector.OnStopMoving -= StopMoving;

        inputCollector.OnStartRunning -= StartRunning;
        inputCollector.OnStopRunning -= StopRunning;

        inputCollector.OnJump -= Jump;
    }

    private void FlipLocalScale()
    {
        /* Instead of flipping the sprite (which does not actually flip its skeleton?!?),
         * we have to flip the horizontal local scale so the bone animations will work properly */
        Vector3 newLocalScale = transform.localScale;
        newLocalScale.x *= -1;
        transform.localScale = newLocalScale;
    }

    private void TurnLeft()
    {
        if (flippedLeft)
        {
            return;
        }

        FlipLocalScale();
        flippedLeft = true;
    }

    private void TurnRight()
    {
        if (!flippedLeft)
        {
            return;
        }

        FlipLocalScale();
        flippedLeft = false;
    }

    private void Move()
    {
        var velocity = rb2d.velocity;
        velocity.x = (isRunning ? runningSpeed : walkingSpeed);

        if (flippedLeft)
        {
            velocity.x *= -1;
        }

        rb2d.velocity = velocity;
    }

    private void StopMoving()
    {
        isRunning = false;
        rb2d.velocity = new Vector2(0, rb2d.velocity.y);
    }

    private void StartRunning()
    {
        if (!isJumping)
        {
            isRunning = true;
        }
    }

    private void StopRunning()
    {
        isRunning = false;
    }

    private void Jump()
    {
        if (isJumping)
        {
            return;
        }

        var velocity = rb2d.velocity;
        velocity.y = jumpVelocity;
        rb2d.velocity = velocity;

        isJumping = true;
    }

    private void Land()
    {
        isJumping = false;
    }
}
