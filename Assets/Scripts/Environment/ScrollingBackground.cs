using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScrollingBackground : MonoBehaviour {
    [Tooltip("Game object whose RigidBody2D will be used as a reference for scrolling.")]
    public GameObject objectToFollow;
    [Tooltip("Scrolling speed (0 for no scrolling and 1 for natural scrolling)")]
    public float scrollSpeed;
    [Tooltip("Horizontal position change threshold required to move the background.")]
    public float positionChangeThreshold;

    private Vector2 previousPosition;
    private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
        this.rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        var currentPosition = objectToFollow.transform.position;

        // Scroll only if the reference object actually changes its horizontal position
        if (Math.Abs(currentPosition.x - previousPosition.x) >= positionChangeThreshold)
        {
            // Keep only a fixed portion of the velocity of the followed object
            Vector2 referenceVelocity = objectToFollow.GetComponent<Rigidbody2D>().velocity;
            this.rb2d.velocity = referenceVelocity - (referenceVelocity * scrollSpeed);

            // Remember the last position for which we moved the background
            previousPosition = currentPosition;
        }
        else
        {
            this.rb2d.velocity = Vector2.zero;
        }
	}
}
