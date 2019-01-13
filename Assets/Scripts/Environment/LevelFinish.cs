using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelFinish : MonoBehaviour {
    public Action OnLevelFinish;

	// Use this for initialization
	void Start() {}
	
	// Update is called once per frame
	void Update() {}

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.isTrigger && (col.gameObject.tag == "SpritePlayer") && (OnLevelFinish != null))
        {
            OnLevelFinish();
        }
    }
}
