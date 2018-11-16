using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternFlickerController : MonoBehaviour {
    [Tooltip("Sprite which the flickering effect should follow")]
    public SpriteRenderer spriteToFollow;
    [Tooltip("Sprite bone to which the flickering effect should adjust during animations")]
    public GameObject boneToFollow;
    [Tooltip("X offset from the bone to follow")]
    public float yOffset;
    [Tooltip("Y offset from the bone to follow")]
    public float xOffset;
    private SpriteRenderer spriteRenderer;
    private bool flippedLeft = false; 
    
	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.position.Set(100, 100, 100);
	}

    // Update is called once per frame
    void Update() {
        if (flippedLeft && Input.GetKeyDown("d"))
        {
            flippedLeft = false;
        }
        else if (!flippedLeft && Input.GetKeyDown("a"))
        {
            flippedLeft = true;
        }

        // TODO: refactor this mess
        if (flippedLeft)
        {
            transform.position = new Vector3(
                2 * spriteToFollow.transform.position.x - boneToFollow.transform.position.x - xOffset, 
                boneToFollow.transform.position.y + yOffset, 
                boneToFollow.transform.position.z
            );
        }
        else
        {
            transform.position = new Vector3(
                boneToFollow.transform.position.x + xOffset, 
                boneToFollow.transform.position.y + yOffset,
                boneToFollow.transform.position.z
            );
        }
    }
}
