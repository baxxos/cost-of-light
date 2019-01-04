using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour {
    public GameObject pausePanel;

    private bool isScenePaused = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
	}

    private void TogglePause()
    {
        Time.timeScale = (isScenePaused ? 1 : 0);
        isScenePaused = !isScenePaused;
        pausePanel.SetActive(isScenePaused);
    }

    public void ResumeScene()
    {
        Time.timeScale = 1;
        isScenePaused = false;
        pausePanel.SetActive(false);
    }

    public void PauseScene()
    {
        Time.timeScale = 0;
        isScenePaused = true;
        pausePanel.SetActive(true);
    }

    public void ExitScene()
    {

    }
}
