using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenuController : MonoBehaviour {
    public GameObject gameOverPanel;
    public PlayerCombatController combatController;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnEnable ()
    {
        combatController.OnPlayerHealthZero += ShowGameOverMenu;
    }

    void OnDisable ()
    {
        combatController.OnPlayerHealthZero -= ShowGameOverMenu;
    }

    private void ShowGameOverMenu ()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 1 - 0;
    }
}
