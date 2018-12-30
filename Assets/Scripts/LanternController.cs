using System;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

public class LanternController : MonoBehaviour {
    // TODO: move this controller to the lantern game object (not the lights - add those as references)
    public bool enabledByDefault;
    public float maxFuelLevel = 100;
    public float fuelLevel = 100;
    public float fuelConsumptionRate;
    public float fuelGenerationRate;
    public float fuelToHealthRatio = 1;
    public CombatController combatController;
    public event Action<float, float> OnFuelLevelChanged;
    public event Action OnFuelDepleted;

    private bool isLit;
    private Color originalLightColor;
    private SLKWaveFunctions originalWaveFunction;
    private Dictionary<string, float> originalWaveFunctionParameters;
    private List<GameObject> enlightedSprites;
    private List<Color> enlightedSpritesColors;
    private SpriteRenderer spriteRenderer;
    private SpriteLightColorCycler colorCycler;

    // Use this for initialization
    void Start () {
        colorCycler = GetComponent<SpriteLightColorCycler>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.enabled = enabledByDefault;
        isLit = enabledByDefault;

        // Remember the original light effect settings to restore them later
        originalLightColor = spriteRenderer.color;
        originalWaveFunction = colorCycler.waveFunction;
        originalWaveFunctionParameters = new Dictionary<string, float>()
        {
            { "offset", colorCycler.offset },
            { "amplitude", colorCycler.amplitude },
            { "frequency", colorCycler.frequency }
        };

        // Compose a list of sprites affected by this light
        enlightedSprites = new List<GameObject>();
        enlightedSprites.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        enlightedSprites.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        enlightedSpritesColors = new List<Color>(enlightedSprites.Count);

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
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.E))
        {
            if (fuelLevel > 0)
            {
                ToggleLanternLit(!isLit);
            }
            else
            {
                // TODO: play some sound
            }
        }
        // Replenish fuel in exchange of health while a key is held down
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            spriteRenderer.color = Color.red;
            spriteRenderer.enabled = true;
            ExchangeLanternFuel(fuelGenerationRate * Time.deltaTime);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            if (!isLit)
            {
                spriteRenderer.enabled = false;
            }
            spriteRenderer.color = originalLightColor;
        }
        
        if (isLit)
        {
            ConsumeLanternFuel(fuelConsumptionRate * Time.deltaTime);

            // TODO: low fuel event
            if (fuelLevel < maxFuelLevel / 10)
            {
                colorCycler.offset = 0.875f;
                colorCycler.amplitude = 0.125f;
                colorCycler.frequency = 0.1f;
                colorCycler.waveFunction = SLKWaveFunctions.Random;
            }
            else if (colorCycler.waveFunction == SLKWaveFunctions.Random)
            {
                colorCycler.waveFunction = originalWaveFunction;
                colorCycler.offset = originalWaveFunctionParameters["offset"];
                colorCycler.amplitude = originalWaveFunctionParameters["amplitude"];
                colorCycler.frequency = originalWaveFunctionParameters["frequency"];
            }
        }
    }

    private void ToggleLanternLit(bool newState)
    {
        isLit = newState;
        spriteRenderer.enabled = (isLit ? true : false);

        for (int i = 0; i < enlightedSprites.Count; i++)
        {
            var sprite = enlightedSprites[i];
            sprite.GetComponent<SpriteRenderer>().color = (isLit ? enlightedSpritesColors[i] : Color.black);
        }
    }

    private void ConsumeLanternFuel(float amount)
    {
        // Stop rendering light & notify subscribers there's no fuel left
        if (fuelLevel - amount <= 0)
        {
            ToggleLanternLit(false);

            if (OnFuelDepleted != null)
            {
                OnFuelDepleted();
            }

            return;
        }

        fuelLevel -= amount;

        // Notify subscribers about fuel level decrease
        if (OnFuelLevelChanged != null)
        {
            OnFuelLevelChanged(fuelLevel, maxFuelLevel);
        }
    }

    private void AddLanternFuel(float amount)
    {
        // Prevent exceeding the maximum fuel limit
        fuelLevel = (fuelLevel + amount >= maxFuelLevel ? maxFuelLevel : fuelLevel + amount);

        // Notify subscribers about fuel level increase
        if (OnFuelLevelChanged != null)
        {
            OnFuelLevelChanged(fuelLevel, maxFuelLevel);
        }
    }

    private void ExchangeLanternFuel(float amount)
    {
        combatController.DecreaseHealth(amount * fuelToHealthRatio);
        AddLanternFuel(amount);
    }
}
