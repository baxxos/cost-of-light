using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenuController : MonoBehaviour {
    public bool IsGameOver { get; set; }
    public GameObject gameOverPanel;
    public PlayerCombatController combatController;

    // Use this for initialization
    void Start () {
        IsGameOver = false;  // TODO: update to C# v6.X to set this at the declaration
    }
	
	// Update is called once per frame
	void Update () {	}

    void OnEnable ()
    {
        combatController.OnPlayerHealthZero += ShowGameOverMenu;
    }

    void OnDisable ()
    {
        combatController.OnPlayerHealthZero -= ShowGameOverMenu;
    }

    public void ShowGameOverMenu ()
    {
        IsGameOver = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void RetryGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        IsGameOver = false;
        gameOverPanel.SetActive(false);
        Time.timeScale = 1;
    }
}
