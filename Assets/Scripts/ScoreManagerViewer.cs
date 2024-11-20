using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScoreManagerViewer : MonoBehaviour
{
    public int CurrentScore {  get; private set; }
    public event Action OnScoreChanged;
    public event Action<int> OnLevelUp;
    public GameSettings gameSettings;
    public int currentLevel = 1;
    public ScoreManagerModel scoreManagerModel;
    public static ScoreManagerViewer instance;
    public int updatedLevel;
    public static ScoreManagerViewer Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("ScoreManagerViewer");
                instance = go.AddComponent<ScoreManagerViewer>();
            }           
            return instance;
        }      
    }
    public void Initialize(GameSettings gameSettings)
    {
        instance.gameSettings = gameSettings;
        if (scoreManagerModel == null)
        {
            scoreManagerModel = new ScoreManagerModel(gameSettings, this);
        }
    }
    
    public void AddPoints(List<GameObject> listOfMatches)
    {
        CurrentScore += scoreManagerModel.CalculatePoints(listOfMatches);
        //Debug.Log("Invoking OnScoreChanged, current subscribers: " + OnScoreChanged?.GetInvocationList().Length);
        OnScoreChanged?.Invoke();
        HasLevelChanged();
    }
    public void HasLevelChanged()
    {
        int oldLevel = currentLevel;
        var levelAndnumberOfCandies = scoreManagerModel.CandiesForCurrentLevel(CurrentScore, currentLevel);
        updatedLevel = levelAndnumberOfCandies.Item1;
        int numberOfCandies = levelAndnumberOfCandies.Item2;
        if (oldLevel < updatedLevel)
        {
            OnLevelUp?.Invoke(numberOfCandies);
        }
    }
    

}
