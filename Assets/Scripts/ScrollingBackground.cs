using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour {
    [Tooltip("Game object whose RigidBody2D will be used as a reference for scrolling.")]
    public GameObject objectToFollow;
    [Tooltip("Scrolling speed (0 for no scrolling and 1 for natural scrolling)")]
    public float scrollSpeed;
    private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
        this.rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        // Keep only a fixed portion of the velocity of the followed object
        Vector2 referenceVelocity = objectToFollow.GetComponent<Rigidbody2D>().velocity;
        this.rb2d.velocity = referenceVelocity - (referenceVelocity * scrollSpeed);
	}
}
