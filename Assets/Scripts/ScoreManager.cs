using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private FieldManager fieldManager;

    [SerializeField] [Range(10,1000)] private int minScoreIncrease = 100;
    [SerializeField] [Range(100, 10000)] private int goal = 5000;
    
    public int Score { get; private set; }

    public int Goal
    {
        get => goal;
        set => goal = value;
    }

    public event Action ScoreUpdated;
    public event Action WinScoreReached;

    private void OnEnable()
    {
        fieldManager.MatchesFound += AddScore;
    }

    private void AddScore(int matchedTilesCount)
    {
        Score += minScoreIncrease * (matchedTilesCount - 2); //минимум может прийти 3 и множитель будет 1
        OnScoreUpdated();

        if (Score >= Goal)
        {
            GameController._isGameOver = false;
            OnWinScoreReached();
        }
    }

    private void OnScoreUpdated()
    {
        ScoreUpdated?.Invoke();
    }

    private void OnWinScoreReached()
    {
        WinScoreReached?.Invoke();
    }
    
    private void OnDisable()
    {
        fieldManager.MatchesFound -= AddScore;
    }
}
