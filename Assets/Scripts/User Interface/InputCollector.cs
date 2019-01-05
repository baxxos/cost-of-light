using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputCollector : MonoBehaviour {
    // Player movement controls
    public Action OnTurnLeft;
    public Action OnTurnRight;
    public Action OnMove;
    public Action OnStopMoving;
    public Action OnStartRunning;
    public Action OnStopRunning;

    // Other player controls
    public Action OnPauseMenuRequested;
    public Action OnAttack;
    public Action OnLanternToggle;
    public Action OnLanternChanneling;

    private SceneController sceneController;

	// Use this for initialization
	void Start () {
        sceneController = GetComponent<SceneController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            NotifySubscribers(OnPauseMenuRequested);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            NotifySubscribers(OnTurnLeft);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            NotifySubscribers(OnTurnRight);
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            NotifySubscribers(OnStartRunning);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            NotifySubscribers(OnLanternToggle);
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            NotifySubscribers(OnMove);
        }

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            NotifySubscribers(OnStopMoving);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            NotifySubscribers(OnStopRunning);
        }

        if (Input.GetMouseButtonDown(0))
        {
            NotifySubscribers(OnAttack);
        }
    }

    private void NotifySubscribers(Action OnAction)
    {
        // Process player actions only if the game is running (except for the pause action)
        if (sceneController.IsScenePaused && (OnAction != OnPauseMenuRequested))
        {
            return;
        }

        if (OnAction != null)
        {
            OnAction();
        }
    }
}
