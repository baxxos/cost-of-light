using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
    public GameObject pausePanel;
    public bool IsScenePaused { get; set; }

    private InputCollector inputCollector;

	// Use this for initialization
	void Start () {
        inputCollector = GetComponent<InputCollector>();
        inputCollector.OnPauseMenuRequested += TogglePause;
        IsScenePaused = false;  // TODO: update to C# v6.X to set this at the declaration
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void TogglePause()
    {
        Time.timeScale = (IsScenePaused ? 1 : 0);
        IsScenePaused = !IsScenePaused;
        pausePanel.SetActive(IsScenePaused);
    }

    public void ResumeScene()
    {
        Time.timeScale = 1;
        IsScenePaused = false;
        pausePanel.SetActive(false);
    }

    public void PauseScene()
    {
        Time.timeScale = 0;
        IsScenePaused = true;
        pausePanel.SetActive(true);
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ResumeScene();
    }

    public void QuitScene()
    {
        Application.Quit();
    }
}
