using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private FieldManager fieldManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private MovesManager movesManager;
    [SerializeField] private MainUIController mainUIController;

    public static bool _isGameOver;
    public static event Action GameOver;

    private void OnEnable()
    {
        scoreManager.WinScoreReached += GameOverActions;
        movesManager.LastMove += GameOverActions;
    }

    private void GameOverActions()
    {
        Debug.Log("Game Over!");
        OnGameOver();
        fieldManager.EraseField();
        fieldManager.gameObject.SetActive(false);
        mainUIController.SetGameOverUI();
    }

    public void RestartScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    private void OnGameOver()
    {
        GameOver?.Invoke();
    }

    private void OnDisable()
    {
        scoreManager.WinScoreReached -= GameOverActions;
        movesManager.LastMove -= GameOverActions;
    }
}
