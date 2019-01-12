using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {
    public GameObject controlsPanel;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}

    public void ShowControls ()
    {
        controlsPanel.SetActive(true);
    }

    public void HideControls ()
    {
        controlsPanel.SetActive(false);
    }

    public void StartGame ()
    {
        SceneManager.LoadScene("DemoScene");
    }
    
    public void QuitGame ()
    {
        Application.Quit();
    }
}
