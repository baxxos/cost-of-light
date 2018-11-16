using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelBarController : MonoBehaviour {
    public LanternController lanternController;

	// Use this for initialization
	void Start () {
        transform.localScale = new Vector2(
            lanternController.fuelLevel / lanternController.maxFuelLevel, 
            transform.localScale.y
        );
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnEnable()
    {
        lanternController.OnFuelLevelChanged += HandleFuelLevelChanged;
    }

    void OnDisable()
    {
        lanternController.OnFuelLevelChanged -= HandleFuelLevelChanged;
    }

    private void HandleFuelLevelChanged(float fuelLevel, float maxFuelLevel)
    {
        transform.localScale = new Vector2(fuelLevel / maxFuelLevel, transform.localScale.y);
    }
}
