﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour {
    [SerializeField] float slowMoFactor = .2f;
    bool levelFinished =false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!levelFinished)
            {
                Debug.Log("Contact");
                levelFinished = true;
                Time.timeScale = slowMoFactor;
                FindObjectOfType<LevelLoader>().LoadNextlevel();
            }
        }
    }

}
