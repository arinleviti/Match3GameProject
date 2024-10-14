using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI scoreText;
    private ScoreManager scoreManager;
    //public static UIController instance;
    //public static UIController Instance
    //{
    //    get
    //    {
    //        if (instance == null)
    //        {
    //            GameObject go = new GameObject("UIController");
    //            instance = go.AddComponent<UIController>();
    //        }
    //        return instance;
    //    }
    //}

    private void Awake()
    {
        scoreManager = ScoreManager.Instance;
        if (scoreManager != null)
        {
            scoreManager.OnScoreChanged += UpdateScoreUI;
        }
    }

    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + scoreManager.CurrentScore.ToString();
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
