using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class contains the logic to decide whether a candy can be swapped or not, checking for matches in all 4 directions.
public class PreMovementChecksViewer : MonoBehaviour
{
    //public GridManager _gridManager;
    private GameSettings _gameSettings;
    private GameObject[,] candiesArray;
    public PreMovementChecksModel _preMovementChecksModel;
    private static PreMovementChecksViewer instance;
    public static PreMovementChecksViewer Instance
    { 
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("PreMovementChecksViewer");
                instance = go.AddComponent<PreMovementChecksViewer>(); 
                instance._gameSettings = Resources.Load<GameSettings>("ScriptableObjects/GridSettings");
                instance.InstantiateModel();
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // Initialize game settings here
            _gameSettings = Resources.Load<GameSettings>("ScriptableObjects/GridSettings");
            // Ensure this instance persists across scenes if needed
            //DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instance
        }
        
    }
    public void InstantiateModel()
    {
        _preMovementChecksModel = new PreMovementChecksModel(_gameSettings, this);
    }
    public CandyViewer GetCandyComponent(GameObject candy)
    {
        if(candy != null)
        {
            CandyViewer currentCandy = candy.GetComponent<CandyViewer>();
            return currentCandy;
        }
        else { return null; }
    }
    
}
