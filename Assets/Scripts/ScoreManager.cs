using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int CurrentScore {  get; private set; }
    public event Action OnScoreChanged;
    private GameSettings gameSettings;
    

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
    
    public IEnumerator AddPoints(List<GameObject> listOfMatches)
    {
        CurrentScore += CalculatePoints(listOfMatches);
        //Debug.Log("Invoking OnScoreChanged, current subscribers: " + OnScoreChanged?.GetInvocationList().Length);
        OnScoreChanged?.Invoke();
        yield return null;
    }
    
    private int CalculatePoints(List<GameObject> listOfMatches)
    {       
        switch (listOfMatches.Count)
        
        {
            case 3: return gameSettings.pointsFor3;
            case 4: return gameSettings.pointsFor4;
            case 5: return gameSettings.pointsFor5;
            case 6: return gameSettings.pointsFor6OrHigher;
            default: return 0;
        }
    }

   
}
