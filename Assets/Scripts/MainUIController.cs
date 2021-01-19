using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIController : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private MovesManager movesManager;

    [SerializeField] private SpriteRenderer background;
    [SerializeField] private List<Sprite> listOfBackgroundSprites; 

    [SerializeField] private Text lblScore;
    [SerializeField] private Text lblGoal;
    [SerializeField] private Text lblGameOver;
    [SerializeField] private Text lblMoves;

    [SerializeField] private Button btnRestart;
    [SerializeField] private Button btnExit;

    private void OnEnable()
    {
        scoreManager.ScoreUpdated += UpdateScore;
        movesManager.AvailableMovesCountUpdated += UpdateMoves;
    }

    private void Start()
    {
        TextInit();
        background.sprite = listOfBackgroundSprites[Random.Range(0, listOfBackgroundSprites.Count)];
    }

    private void TextInit()
    {
        lblScore.text = $"Score:\n{scoreManager.Score}";
        lblGoal.text = $"Goal:\n{scoreManager.Goal}";
        lblMoves.text = $"Moves:\n{movesManager.AvailableMovesCount}";
        lblGameOver.text = "";
        
        Text lblRestart = btnRestart.GetComponentInChildren(typeof(Text)) as Text;
        lblRestart.text = "Restart";
        
        Text btnExit = this.btnExit.GetComponentInChildren(typeof(Text)) as Text;
        btnExit.text = "Exit";
    }

    public void SetPauseUI()
    {
        btnExit.gameObject.SetActive(GameController.IsOnPause);
    }

    public void SetGameOverUI()
    {
        HideUI();
        lblGameOver.text = GameController.IsGameOver ? "Game Over" : $"You win!\nMoves left: {movesManager.AvailableMovesCount}";
        lblGameOver.gameObject.SetActive(true);
        btnRestart.gameObject.SetActive(true);
    }

    private void HideUI()
    {
        lblScore.gameObject.SetActive(false);
        lblMoves.gameObject.SetActive(false);
        lblGoal.gameObject.SetActive(false);
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
