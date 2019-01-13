using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsCameraController : MonoBehaviour {
    public PlayerCombatController combatController;

    private Camera lightsCamera;
    private Animator animator;

    // Use this for initialization
    void Start () {
        lightsCamera = GetComponent<Camera>();
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {}

    void OnEnable()
    {
        combatController.OnPlayerHealthZero += PlayDeathAnimation;
        combatController.OnPlayerHealthChanged += PlayDamageTakenAnimation;
    }

    void OnDisable()
    {
        combatController.OnPlayerHealthZero -= PlayDeathAnimation;
        combatController.OnPlayerHealthChanged -= PlayDamageTakenAnimation;
    }

    private void PlayDeathAnimation()
    {
        // Turn off the lights
        lightsCamera.cullingMask = 0;
    }

    private void PlayDamageTakenAnimation(float healthChange, float currentHealth, float maxhealth)
    {
        if (healthChange < 0)
        {
            animator.SetTrigger("healthLowered");
        }
    }
}
