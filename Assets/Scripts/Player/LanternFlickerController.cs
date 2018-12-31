﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

public class LanternFlickerController : MonoBehaviour {
    [Tooltip("Sprite which the flickering effect should follow")]
    public SpriteRenderer spriteToFollow;
    [Tooltip("Sprite bone to which the flickering effect should adjust during animations")]
    public GameObject boneToFollow;
    [Tooltip("X offset from the bone to follow")]
    public float yOffset;
    [Tooltip("Y offset from the bone to follow")]
    public float xOffset;

    private bool flippedLeft = false; 
    private Color originalLightColor;
    private SLKWaveFunctions originalWaveFunction;
    private Dictionary<string, float> originalWaveFunctionParameters;
    private SpriteLightColorCycler colorCycler;
    private SpriteRenderer spriteRenderer;
    
	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        colorCycler = GetComponent<SpriteLightColorCycler>();

        // Remember the original light effect settings to restore them later
        originalLightColor = spriteRenderer.color;
        originalWaveFunction = colorCycler.waveFunction;
        originalWaveFunctionParameters = new Dictionary<string, float>()
        {
            { "offset", colorCycler.offset },
            { "amplitude", colorCycler.amplitude },
            { "frequency", colorCycler.frequency }
        };
	}

    // Update is called once per frame
    void Update()
    {
        if (flippedLeft && Input.GetKeyDown("d"))
        {
            flippedLeft = false;
        }
        else if (!flippedLeft && Input.GetKeyDown("a"))
        {
            flippedLeft = true;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            SetLightEffectChanneling();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            SetLightEffectDefault();
        }

        AdjustPosition();
    }

    private void SetLightEffectChanneling()
    {
        colorCycler.ChangeOriginalColor(Color.yellow);
        colorCycler.waveFunction = SLKWaveFunctions.SawTooth;
        colorCycler.offset = 0.25f;
        colorCycler.amplitude = 0.75f;
        colorCycler.frequency = 4f;
    }

    private void SetLightEffectDefault()
    {
        colorCycler.ChangeOriginalColor(originalLightColor);
        colorCycler.waveFunction = originalWaveFunction;
        colorCycler.offset = originalWaveFunctionParameters["offset"];
        colorCycler.amplitude = originalWaveFunctionParameters["amplitude"];
        colorCycler.frequency = originalWaveFunctionParameters["frequency"];
    }

    private void AdjustPosition()
    {
        var newPosition = new Vector2(
            boneToFollow.transform.position.x + xOffset,
            boneToFollow.transform.position.y + yOffset
        );

        if (flippedLeft)
        {
            newPosition.x = 2 * spriteToFollow.transform.position.x - boneToFollow.transform.position.x - xOffset;
        }

        transform.position = newPosition;
    }
}