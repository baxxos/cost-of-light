using System;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

public class LanternController : MonoBehaviour {
    // TODO: move this controller to the lantern game object (not the lights - add those as references)
    [Tooltip("Reference to the input collector class (component).")]
    public InputCollector inputCollector;
    [Tooltip("Should the lantern be lit when the game starts?")]
    public bool enabledByDefault;
    [Tooltip("Maximum amount of fuel stored in the lantern.")]
    public float maxFuelLevel = 100;
    [Tooltip("Current amount of fuel.")]
    public float fuelLevel = 100;
    [Tooltip("Amount of fuel consumed per unit of time.")]
    public float fuelConsumptionRate;
    [Tooltip("Speed at which the fuel can be exchanged for health.")]
    public float fuelGenerationRate;
    [Tooltip("Amount of health required for 1 generating unit of fuel.")]
    public float fuelToHealthRatio = 1;
    [Tooltip("Reference to the player combat controller (e. g. for decreasing health).")]
    public PlayerCombatController combatController;

    public event Action<float, float> OnFuelLevelChanged;
    public event Action OnFuelDepleted;

    private bool isLit;
    private Color originalLightColor;
    private SLKWaveFunctions originalWaveFunction;
    private Dictionary<string, float> originalWaveFunctionParameters;
    private List<GameObject> revealedSprites;
    private List<GameObject> enlightedSprites;
    private List<Color> enlightedSpritesColors;
    private SpriteRenderer spriteRenderer;
    private SpriteLightColorCycler colorCycler;

    // Use this for initialization
    void Start ()
    {
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
        enlightedSprites.AddRange(GameObject.FindGameObjectsWithTag("SpritePlayer"));
        enlightedSprites.AddRange(GameObject.FindGameObjectsWithTag("SpriteEnemy"));
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

        // Compose a list of sprites revealed by this light
        revealedSprites = new List<GameObject>();
        revealedSprites.AddRange(GameObject.FindGameObjectsWithTag("SpriteEnemyState"));

        foreach (GameObject sprite in revealedSprites)
        {
            if (!enabledByDefault)
            {
                sprite.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
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

    void OnEnable ()
    {
        inputCollector.OnLanternToggle += ToggleLanternLit;
        inputCollector.OnLanternExchange += ExchangeLanternFuel;
        inputCollector.OnLanternStopExchange += StopLanternFuelExchange;
    }

    void OnDisable ()
    {
        inputCollector.OnLanternToggle -= ToggleLanternLit;
        inputCollector.OnLanternExchange -= ExchangeLanternFuel;
        inputCollector.OnLanternStopExchange -= StopLanternFuelExchange;
    }

    private void ExtinguishLantern()
    {
        isLit = false;
        spriteRenderer.enabled = false;
        AdjustSpritesToLight();
    }

    private void LightLantern()
    {
        isLit = true;
        spriteRenderer.enabled = true;
        AdjustSpritesToLight();
    }

    private void ToggleLanternLit()
    {
        isLit = !isLit;
        spriteRenderer.enabled = isLit;
        AdjustSpritesToLight();
    }

    private void AdjustSpritesToLight()
    {
        // Adjust sprites which are displayed as silhouettes when the light is off
        for (int i = 0; i < enlightedSprites.Count; i++)
        {
            var sprite = enlightedSprites[i];
            sprite.GetComponent<SpriteRenderer>().color = (isLit ? enlightedSpritesColors[i] : Color.black);
        }

        // Adjust sprites which are hidden or revealed based on the light
        foreach (GameObject sprite in revealedSprites)
        {
            sprite.GetComponent<SpriteRenderer>().enabled = isLit;
        }
    }

    private void ConsumeLanternFuel(float amount)
    {
        // Stop rendering light & notify subscribers there's no fuel left
        if (fuelLevel - amount <= 0)
        {
            ExtinguishLantern();

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

    private void ExchangeLanternFuel()
    {
        // Turn the light red
        colorCycler.ChangeOriginalColor(Color.red);
        spriteRenderer.enabled = true;

        // Replenish fuel in exchange for health
        float amount = fuelGenerationRate * Time.deltaTime;
        combatController.DecreaseHealth(amount * fuelToHealthRatio);
        AddLanternFuel(amount);
    }

    private void StopLanternFuelExchange()
    {
        // Restore the regular light look
        spriteRenderer.enabled = isLit;
        colorCycler.ChangeOriginalColor(originalLightColor);
    }
}
