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
	void Update () {
        // TODO: move this to a separate class taking care of death/pausing the game etc.
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
        {
            lightsCamera.backgroundColor = Color.black;
            Time.timeScale = 0;
        }
    }

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
        // Turn off the lights and trigger the darken animation
        lightsCamera.cullingMask = 0;
        animator.SetTrigger("healthZero");
    }

    private void PlayDamageTakenAnimation(float healthChange, float currentHealth, float maxhealth)
    {
        if (healthChange < 0)
        {
            animator.SetTrigger("healthLowered");
        }
    }
}
