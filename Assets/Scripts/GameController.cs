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

    private void OnEnable()
    {
        scoreManager.WinScoreReached += GameOver;
        movesManager.LastMove += GameOver;
    }

    private void GameOver()
    {
        fieldManager.EraseField();
        fieldManager.gameObject.SetActive(false);
        mainUIController.SetGameOverUI();
    }

    public void RestartScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    private void OnDisable()
    {
        scoreManager.WinScoreReached -= GameOver;
        movesManager.LastMove -= GameOver;
    }
}
