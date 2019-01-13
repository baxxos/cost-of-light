using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteMenuController : MonoBehaviour {
    public GameObject levelCompletePanel;
    public LevelFinish levelFinishTrigger;

	// Use this for initialization
	void Start() {}
	
	// Update is called once per frame
	void Update() {}

    private void OnEnable()
    {
        levelFinishTrigger.OnLevelFinish += ShowLevelCompleteMenu;
    }

    private void OnDisable()
    {
        levelFinishTrigger.OnLevelFinish -= ShowLevelCompleteMenu;
    }

    private void ShowLevelCompleteMenu()
    {
        Time.timeScale = 0;
        levelCompletePanel.SetActive(true);
    }
}
