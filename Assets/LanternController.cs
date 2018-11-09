using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternController : MonoBehaviour {
    public bool enabledByDefault;
    private SpriteRenderer spriteRenderer;
    private GameObject[] enlightedSprites;
    private List<Color> enlightedSpritesColors;

	// Use this for initialization
	void Start () {
        gameObject.SetActive(true);
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = enabledByDefault;

        enlightedSprites = GameObject.FindGameObjectsWithTag("Player");
        enlightedSpritesColors = new List<Color>(enlightedSprites.Length);

        if (!enabledByDefault)
        {
            foreach (GameObject sprite in enlightedSprites)
            {
                enlightedSpritesColors.Add(sprite.GetComponent<SpriteRenderer>().color);
                sprite.GetComponent<SpriteRenderer>().color = Color.black;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown("e"))
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;

            // TODO: refactor this mess
            int index = 0;
            foreach (GameObject sprite in enlightedSprites)
            {
                sprite.GetComponent<SpriteRenderer>().color = (spriteRenderer.enabled ? enlightedSpritesColors[index]: Color.black);
                index++;
            }
        }
    }
}
