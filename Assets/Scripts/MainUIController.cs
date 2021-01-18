using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainUIController : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private MovesManager movesManager;
    
    [SerializeField] private Text lblScore;
    [SerializeField] private Text lblGoal;
    [SerializeField] private Text lblGameOver;
    [SerializeField] private Text lblMoves;

    [SerializeField] private Button btnRestart;

    private void OnEnable()
    {
        scoreManager.ScoreUpdated += UpdateScore;
        movesManager.AvailableMovesCountUpdated += UpdateMoves;
    }

    private void Start()
    {
        TextInit();
    }

    private void TextInit()
    {
        lblScore.text = $"Score:\n{scoreManager.Score}";
        lblGoal.text = $"Goal:\n{scoreManager.Goal}";
        lblMoves.text = $"Moves:\n{movesManager.AvailableMovesCount}";
        lblGameOver.text = "";
        Text lblRestart = btnRestart.GetComponentInChildren(typeof(Text)) as Text;
        lblRestart.text = "Restart";
    }

    public void SetGameOverUI()
    {
        HideUI();
        lblGameOver.text = movesManager.AvailableMovesCount == 0 ? "Game Over" : "You win!";
        lblGameOver.gameObject.SetActive(true);
        btnRestart.gameObject.SetActive(true);
    }

    private void HideUI()
    {
        lblScore.gameObject.SetActive(false);
        lblMoves.gameObject.SetActive(false);
    }
    
    private void UpdateScore()
    {
        lblScore.text = $"Score: \n {scoreManager.Score}";
    }
    
    private void UpdateMoves()
    {
        lblMoves.text = $"Moves: \n {movesManager.AvailableMovesCount}";
    }

    private void OnDisable()
    {
        scoreManager.ScoreUpdated -= UpdateScore;
        movesManager.AvailableMovesCountUpdated -= UpdateMoves;
    }
}
