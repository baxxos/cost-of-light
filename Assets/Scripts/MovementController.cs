using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float baseSpeed;
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
        HandleControls();
    }

    private void HandleControls()
    {
        HandleKeyControls();
        HandleMouseControls();
    }

    private void HandleKeyControls()
    {
        if (Input.GetKey("d"))
        {
            spriteRenderer.flipX = false;
            var velocity = rb.velocity;
            velocity.x = baseSpeed;
            rb.velocity = velocity;
        }

        if (Input.GetKey("a"))
        {
            spriteRenderer.flipX = true;
            var velocity = rb.velocity;
            velocity.x = -baseSpeed;
            rb.velocity = velocity;
        }

        if (Input.GetKeyUp("d") || Input.GetKeyUp("a"))
        {
            rb.velocity = Vector2.zero;
        }

        animator.SetFloat("horizontalMovement", Math.Abs(rb.velocity.x));
    }

    private void HandleMouseControls()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("attack");
        }
    }
}
