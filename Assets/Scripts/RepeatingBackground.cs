using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingBackground : MonoBehaviour {
    public Camera cameraToFollow;
    private float cameraExtentHorizontal;
    private float cameraExtentVertical;
    private float width;
    private float height;

    private void AdjustPositionToRight()
    {
        transform.position = new Vector2(
            transform.position.x + 3 * this.width, 
            transform.position.y
        );
    }

    private void AdjustPositionToLeft()
    {
        transform.position = new Vector2(
            transform.position.x - 3 * this.width,
            transform.position.y
        );
    }

    private float GetRightCameraEdge()
    {
        return cameraToFollow.transform.position.x + (this.cameraExtentHorizontal);
    }

    private float GetLeftCameraEdge()
    {
        return cameraToFollow.transform.position.x - (this.cameraExtentHorizontal);
    }

    // Use this for initialization
    void Start () {
        this.width = GetComponent<SpriteRenderer>().bounds.size.x;
        this.height = GetComponent<BoxCollider2D>().size.y;
        this.cameraExtentVertical = cameraToFollow.orthographicSize;
        this.cameraExtentHorizontal = this.cameraExtentVertical * Screen.width / Screen.height;
    }
	
	// Update is called once per frame
	void Update () {
		if (GetRightCameraEdge() >= transform.position.x + 2.5 * this.width)
        {
            AdjustPositionToRight();
        }
        else if (GetLeftCameraEdge() <= transform.position.x - 2.5 * this.width)
        {
            AdjustPositionToLeft();
        }
	}
}
