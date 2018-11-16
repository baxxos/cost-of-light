using System;
using System.Collections.Generic;
using UnityEngine;

public class LanternController : MonoBehaviour {
    // TODO: move this controller to the lantern game object (not the lights - add those as references)
    public bool enabledByDefault;
    public float maxFuelLevel;
    public float fuelLevel;
    public float fuelConsumption;
    public event Action<float, float> OnFuelLevelChanged;
    public event Action OnFuelDepleted;

    private SpriteRenderer spriteRenderer;
    private GameObject[] enlightedSprites;
    private List<Color> enlightedSpritesColors;

	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = enabledByDefault;

        enlightedSprites = GameObject.FindGameObjectsWithTag("Player");
        enlightedSpritesColors = new List<Color>(enlightedSprites.Length);

        // Remember the original colors of all sprites interacting with light
        foreach (GameObject sprite in enlightedSprites)
        {
            enlightedSpritesColors.Add(sprite.GetComponent<SpriteRenderer>().color);

            if (!enabledByDefault)
            {
                sprite.GetComponent<SpriteRenderer>().color = Color.black;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        // Toggle the light rendering on mouse click or key press
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown("e"))
        {
            if (fuelLevel >= 0)
            {
                ToggleLanternState(!spriteRenderer.enabled);
            }
            else
            {
                // TODO: play some sound
            }
        }

        if (spriteRenderer.enabled && fuelLevel > 0)
        {
            ConsumeLanternFuel();
        }
    }

    private void ToggleLanternState(bool newState)
    {
        spriteRenderer.enabled = newState;

        for (int i = 0; i < enlightedSprites.Length; i++)
        {
            var sprite = enlightedSprites[i];
            sprite.GetComponent<SpriteRenderer>().color = (spriteRenderer.enabled ? enlightedSpritesColors[i] : Color.black);
        }
    }

    private void ConsumeLanternFuel()
    {
        fuelLevel -= fuelConsumption * Time.deltaTime;

        // Notify subscribers about fuel level decrease
        if (OnFuelLevelChanged != null)
        {
            OnFuelLevelChanged(fuelLevel, maxFuelLevel);
        }

        // Stop rendering light & notify subscribers there's no fuel left
        if (fuelLevel <= 0)
        {
            ToggleLanternState(false);

            if (OnFuelDepleted != null) {
                OnFuelDepleted();
            }
        }
    }
}
