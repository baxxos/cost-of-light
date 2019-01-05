using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

public class LanternFlickerController : MonoBehaviour {
    [Tooltip("Reference to the input collector class (component).")]
    public InputCollector inputCollector;
    [Tooltip("Sprite bone to which the flickering effect should adjust during animations")]
    public GameObject boneToFollow;
    [Tooltip("X offset from the bone to follow")]
    public float yOffset;
    [Tooltip("Y offset from the bone to follow")]
    public float xOffset;

    private bool channelingEffectActive = false;
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
        AdjustPosition();
    }

    void OnEnable()
    {
        inputCollector.OnTurnLeft += FlipLeft;

        inputCollector.OnLanternExchange += SetLightEffectChanneling;
        inputCollector.OnLanternStopExchange += SetLightEffectDefault;
    }

    void OnDisable()
    {
        inputCollector.OnTurnLeft -= FlipRight;

        inputCollector.OnLanternExchange -= SetLightEffectChanneling;
        inputCollector.OnLanternStopExchange -= SetLightEffectDefault;
    }

    private void FlipLeft()
    {
        flippedLeft = true;
    }

    private void FlipRight()
    {
        flippedLeft = false;
    }

    private void SetLightEffectChanneling()
    {
        if (channelingEffectActive)
        {
            return;
        }

        channelingEffectActive = true;
        colorCycler.ChangeOriginalColor(Color.yellow);
        colorCycler.waveFunction = SLKWaveFunctions.SawTooth;
        colorCycler.offset = 0.25f;
        colorCycler.amplitude = 0.75f;
        colorCycler.frequency = 4f;
    }

    private void SetLightEffectDefault()
    {
        if (!channelingEffectActive)
        {
            return;
        }

        channelingEffectActive = false;
        colorCycler.ChangeOriginalColor(originalLightColor);
        colorCycler.waveFunction = originalWaveFunction;
        colorCycler.offset = originalWaveFunctionParameters["offset"];
        colorCycler.amplitude = originalWaveFunctionParameters["amplitude"];
        colorCycler.frequency = originalWaveFunctionParameters["frequency"];
    }

    private void AdjustPosition()
    {
        var newPosition = new Vector2(
            boneToFollow.transform.position.x + (flippedLeft? xOffset : -xOffset),
            boneToFollow.transform.position.y + yOffset
        );

        transform.position = newPosition;
    }
}
