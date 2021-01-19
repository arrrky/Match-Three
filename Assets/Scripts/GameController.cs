using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private FieldManager fieldManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private MovesManager movesManager;
    [SerializeField] private MainUIController mainUIController;

    public static bool IsGameOver;
    public static bool IsOnPause;
    public static event Action GameOver;

    private void OnEnable()
    {
        scoreManager.WinScoreReached += GameOverActions;
        movesManager.LastMove += GameOverActions;
    }

    private void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsOnPause = !IsOnPause;
            mainUIController.SetPauseUI();
        }
    }

    private void GameOverActions()
    {
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

    public void ExitGame()
    {
        Application.Quit();
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
