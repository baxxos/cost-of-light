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
    public Action OnJump;

    // Other player controls
    public Action OnPauseMenuRequested;
    public Action OnAttack;

    // Lantern Controls
    public Action OnLanternToggle;
    public Action OnLanternExchange;
    public Action OnLanternStopExchange;

    private SceneController sceneController;

	// Use this for initialization
	void Start () {
        sceneController = GetComponent<SceneController>();
    }
	
	// Update is called once per frame
	void Update () {
        HandleKeyboardControls();
        HandleMouseControls();
    }

    private void HandleMouseControls()
    {
        if (Input.GetMouseButtonDown(0))
        {
            NotifySubscribers(OnAttack);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            NotifySubscribers(OnLanternToggle);
        }
    }

    private void HandleKeyboardControls()
    {
        HandleKeyPressControls();
        HandleKeyHoldControls();
        HandleKeyReleaseControls();
    }

    private void HandleKeyPressControls()
    {
        // Pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            NotifySubscribers(OnPauseMenuRequested);
        }

        // Movement
        if (Input.GetKeyDown(KeyCode.A))
        {
            NotifySubscribers(OnTurnLeft);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            NotifySubscribers(OnTurnRight);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            NotifySubscribers(OnStartRunning);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            NotifySubscribers(OnJump);
        }

        // Lantern
        if (Input.GetKeyDown(KeyCode.E))
        {
            NotifySubscribers(OnLanternToggle);
        }
    }

    private void HandleKeyHoldControls()
    {
        // Movement
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            NotifySubscribers(OnMove);
        }

        // Lantern
        if (Input.GetKey(KeyCode.Q))
        {
            NotifySubscribers(OnLanternExchange);
        }
    }

    private void HandleKeyReleaseControls()
    {
        // Movement
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            NotifySubscribers(OnStopMoving);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            NotifySubscribers(OnStopRunning);
        }

        // Lantern
        if (Input.GetKeyUp(KeyCode.Q))
        {
            NotifySubscribers(OnLanternStopExchange);
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
