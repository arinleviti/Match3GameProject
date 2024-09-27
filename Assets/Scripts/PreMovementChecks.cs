using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class contains the logic to decide whether a candy can be swapped or not, checking for matches in all 4 directions.
public class PreMovementChecks : MonoBehaviour
{
    //public GridManager _gridManager;
    private GameSettings _gameSettings;
    private static PreMovementChecks instance;
    public static PreMovementChecks Instance
    { 
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("PreMovementChecks");
                instance = go.AddComponent<PreMovementChecks>(); 
                instance._gameSettings = Resources.Load<GameSettings>("ScriptableObjects/GridSettings");
            }
            return instance;
        }
    }

    //public void Initialize(GameSettings gameSettings)
    //{
    //    _gameSettings = gameSettings;
    //    //_gridManager = gridManager;
    //}
    //This checks if there are matches left or right (if isHorizontal==true) and up and down (if !isHorizontal)
    public bool CheckRowAndColumn(GameObject candy, GameObject[,] candyArray, bool isHorizontal, out List<GameObject> matches)
    {
        Candy currentCandy = candy.GetComponent<Candy>();
        int currentI = currentCandy.PosInArrayI;
        int currentJ = currentCandy.PosInArrayJ;
        matches = new List<GameObject>() { candy };
        int matchingCandiesCount = 1; 

        int[] offsets = new[] { -1, 1 };

        foreach (int offset in offsets)
        {
            int step = offset;
            

            while (true)
            {
                int nextI = isHorizontal ? currentI : currentI + step;
                int nextJ = isHorizontal ? currentJ + step : currentJ;

                // Check bounds
                if (nextI < 0 || nextI >= _gameSettings.tilesNumberI || nextJ < 0 || nextJ >= _gameSettings.tilesNumberJ)
                    break;

                GameObject neighborCandyGO = candyArray[nextI, nextJ];
                Candy neighborCandy = neighborCandyGO?.GetComponent<Candy>();

                if (neighborCandy != null && neighborCandy.CandyType == currentCandy.CandyType)
                {
                    matchingCandiesCount++;
                    matches.Add(neighborCandyGO);
                    step += offset; // Continue to the next candy in the same direction
                }
                else
                {
                    break; // Stop if no match
                }
            }
        }

        return matchingCandiesCount >= _gameSettings.candiesToMatch; // Return true if there are at least 3 matching candies
    }
    
}
