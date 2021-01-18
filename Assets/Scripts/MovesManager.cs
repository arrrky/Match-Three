using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovesManager : MonoBehaviour
{
    [SerializeField] private FieldManager fieldManager;
    
    [SerializeField] [Range(5, 100)] private int maxMovesCount = 100;

    public event Action AvailableMovesCountUpdated;
    public event Action LastMove;

    public int AvailableMovesCount { get; private set; } 

    private void OnEnable()
    {
        fieldManager.TilesSwapped += DecreaseMovesCount;
    }

    private void Awake()
    {
        AvailableMovesCount = maxMovesCount;
    }

    private void DecreaseMovesCount()
    {
        AvailableMovesCount--;
        OnAvailableMovesCountUpdated();
        
        if (AvailableMovesCount == 0)
        {
            OnLastMove();
        }
    }

    private void OnAvailableMovesCountUpdated()
    {
        AvailableMovesCountUpdated?.Invoke();
    }

    private void OnLastMove()
    {
        LastMove?.Invoke();
    }

    private void OnDisable()
    {
        fieldManager.TilesSwapped += DecreaseMovesCount;
    }
}
