using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI levelText;
    private ScoreManagerViewer scoreManager;
    

    private void Awake()
    {
        scoreManager = ScoreManagerViewer.Instance;
        if (scoreManager != null)
        {
            scoreManager.OnScoreChanged += UpdateScoreUI;
            scoreManager.OnLevelUp += UpdateLevel;
        }
        scoreText.text = "Score: 0";
        levelText.text = "Level: 1";
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + scoreManager.CurrentScore.ToString();
    }
    private void UpdateLevel(int placeholder)
    {
        levelText.text = "Level: " + scoreManager.updatedLevel.ToString();

    }
    private void OnDestroy()
    {
        // Unsubscribe from the event when this object is destroyed
        if (scoreManager != null)
        {
            scoreManager.OnScoreChanged -= UpdateScoreUI;
        }
    }
}
