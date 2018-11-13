using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativeScrollingObject : MonoBehaviour {
    public Camera cameraToFollow;
    private float horizontalWidth;
    private float cameraWidth;
    private float cameraHeight;

    private void AdjustPositionToRight()
    {
        transform.position = new Vector2(transform.position.x + 3 * this.horizontalWidth, 0);
    }

	// Use this for initialization
	void Start () {
        this.horizontalWidth = GetComponent<BoxCollider2D>().size.x;
        this.cameraHeight = 2f * cameraToFollow.orthographicSize;
        this.cameraWidth = this.cameraHeight * cameraToFollow.aspect;
    }
	
	// Update is called once per frame
	void Update () {
		if (cameraToFollow.transform.position.x + (this.cameraWidth / 2) - transform.position.x >= this.horizontalWidth)
        {
            AdjustPositionToRight();
        }
	}
}
