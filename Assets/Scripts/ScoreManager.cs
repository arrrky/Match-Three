using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager
{
    public int Score { get; private set; }

    public ScoreManager()
    {
            
    }

    public void AddScore()
    {
        Debug.Log("Score added");
    }
}
