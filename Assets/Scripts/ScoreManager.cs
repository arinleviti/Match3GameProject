using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int CurrentScore {  get; private set; }
    public event Action OnScoreChanged;
    public GameSettings gameSettings;
    

    public static ScoreManager instance;
    public static ScoreManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("ScoreManager");
                instance = go.AddComponent<ScoreManager>();
            }           
            return instance;
        }      
    }
    public void Initialize(GameSettings gameSettings)
    {
        instance.gameSettings = gameSettings;
    }
    
    public void AddPoints(List<GameObject> listOfMatches)
    {
        CurrentScore += CalculatePoints(listOfMatches);
        //Debug.Log("Invoking OnScoreChanged, current subscribers: " + OnScoreChanged?.GetInvocationList().Length);
        OnScoreChanged?.Invoke();
        
    }
    
    public int CalculatePoints(List<GameObject> listOfMatches)
    {
        if (listOfMatches.Count < 3)
            throw new InvalidOperationException("List must contain at least 3 elements to calculate points.");

        switch (listOfMatches.Count)
        
        {
            case 3: return gameSettings.pointsFor3;
            case 4: return gameSettings.pointsFor4;
            case 5: return gameSettings.pointsFor5;
            case 6: return gameSettings.pointsFor6OrHigher;
            default: return listOfMatches.Count >= 6 ? gameSettings.pointsFor6OrHigher : 0;
        }
    }

   
}
