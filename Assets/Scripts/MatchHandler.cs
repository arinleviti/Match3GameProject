using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchHandler : MonoBehaviour
{
    private static MatchHandler instance;
    public List<GameObject> Matches { get; private set; } = new List<GameObject>();
    private GameSettings _gameSettings;
    private GameObject[,] _candiesArray;
    private GameObject[,] _gridCellsArray;
    private GameObject _candyParent;
    private CandyPool _candyPool;


    public static MatchHandler Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("MatchHandler");
                instance = go.AddComponent<MatchHandler>();
            }
            return instance;
        }
    }

    public void Initialize(GameSettings gameSettings, GameObject[,] candiesArray, GameObject candyParent, GameObject candyPool)
    {
        _gameSettings = gameSettings;
        _candiesArray = candiesArray;
        _candyParent = candyParent;
        _candyPool = candyPool.GetComponent<CandyPool>();
    }
    //Called by GridManager to remove matches when the game loads (using FixMatch (useFixMatch =true)), and after each user-prompted match (using CheckRowAndColumn (useFixMatch = false)
    //to check if the swap creates more matches.
    public void CheckAndFixAllMatches(bool useFixMatch)
    {
        bool foundMatch;
        do
        {           
            Matches.Clear();
            List<GameObject> tempMatches = new List<GameObject>(); // Ensure tempMatches is a new instance
            foundMatch = false;
            // Check rows for matches
            for (int i = 0; i < _gameSettings.tilesNumberI; i++)
            {
                for (int j = 0; j < _gameSettings.tilesNumberJ - 2; j++)
                {
                    if (IsMatch(i, j, i, j + 1, i, j + 2))
                    {
                        if (useFixMatch)
                        {
                            FixMatch(i, j + 1);  // Replace the middle one
                        }
                        else if (!useFixMatch)
                        {
                            Debug.Log($"matches found at:  {i}, {j} and  {i}, {j+1} and {i}, {j +2} ");
                            bool isMatch = PreMovementChecks.Instance.CheckRowAndColumn(_candiesArray[i, j + 1], _candiesArray, true, out tempMatches);
                            if (isMatch)
                            {
                                AddToMatchList(tempMatches);
                            }
                        }
                        foundMatch = true;
                    }
                }
                
            }
            // Check columns for matches
            for (int j = 0; j < _gameSettings.tilesNumberJ; j++)
            {
                for (int i = 0; i < _gameSettings.tilesNumberI - 2; i++)
                {
                    if (IsMatch(i, j, i + 1, j, i + 2, j))
                    {
                        if (useFixMatch)
                        {
                            FixMatch(i + 1, j);  // Replace the middle one
                        }
                        else if (!useFixMatch)
                        {
                            //List<GameObject> tempMatches;
                            Debug.Log($"matches found at:  {i}, {j} and  {i + 1}, {j} and {i + 2}, {j} ");
                            bool isMatch = PreMovementChecks.Instance.CheckRowAndColumn(_candiesArray[i + 1, j], _candiesArray, false, out tempMatches);
                            if (isMatch)
                            {
                                AddToMatchList(tempMatches);
                            }
                            
                        }

                        foundMatch = true;
                    }
                }
                
            }
            if ( Matches.Count > 0)
            {              
                DestroyMatches.Instance.ReturnMatchesInList(Matches);              
            }
        } while (foundMatch);
    }

    private void AddToMatchList(List<GameObject> tempMatches)
    {
        foreach (var match in tempMatches)
        {
            if (!Matches.Contains(match))
            {
                Matches.Add(match);
            }
        }
    }


    private bool IsMatch(int x1, int y1, int x2, int y2, int x3, int y3)
    {
        if (_candiesArray[x1, y1] != null && _candiesArray[x2, y2] != null && _candiesArray[x3, y3] != null)
        {
            Candy c1 = _candiesArray[x1, y1].GetComponent<Candy>();
            Candy c2 = _candiesArray[x2, y2].GetComponent<Candy>();
            Candy c3 = _candiesArray[x3, y3].GetComponent<Candy>();

            return c1.CandyType == c2.CandyType && c1.CandyType == c3.CandyType;
        }
        return false;
    }
    public void FixMatch(int i, int j)
    {

        GameObject oldCandy = _candiesArray[i, j];
        if (oldCandy == null)
        {
            Debug.LogWarning($"No candy to replace at position X: {i}, Y: {j}");
            return; // Exit if there's no candy to replace
        }

        string folderPath = "Prefabs/CandyPrefabs";
        GameObject[] prefabs = Resources.LoadAll<GameObject>(folderPath);

        if (prefabs.Length == 0)
        {
            Debug.LogError("No prefabs found in the specified path: " + folderPath);
            return;
        }

        Debug.Log("Loaded " + prefabs.Length + " prefabs from " + folderPath);

        Candy oldCandyScript = oldCandy.GetComponent<Candy>();
        List<GameObject> availablePrefabs = new List<GameObject>();

        // Print all candy types for debugging
        Debug.Log("Old candy type: " + oldCandyScript.CandyType);

        foreach (GameObject prefab in prefabs)
        {
            Candy candyPrefab = prefab.GetComponent<Candy>();
            Debug.Log("Prefab candy type: " + candyPrefab.CandyType); // Debugging output
            if (candyPrefab.CandyType != oldCandyScript.CandyType)
            {
                availablePrefabs.Add(prefab);
            }
        }

        if (availablePrefabs.Count == 0)
        {
            Debug.LogWarning("No available candy types for replacement. Current candy type: " + oldCandyScript.CandyType);
            return; // Exit if no different candy types are available
        }

        // Select a random candy from available prefabs
        GameObject newCandyPrefab = availablePrefabs[UnityEngine.Random.Range(0, availablePrefabs.Count)];
        if (newCandyPrefab == null)
        {
            Debug.LogError("Selected new candy prefab is null.");
            return;
        }
        Vector2 position = oldCandy.transform.position;
        newCandyPrefab.transform.localScale = oldCandy.transform.localScale;
        Debug.Log("Candy about to be destroyed in position X: " + oldCandyScript.PosX + " position Y: " + oldCandyScript.PosY);
        Destroy(oldCandy);
        GameObject newCandy = Instantiate(newCandyPrefab, new Vector3(position.x, position.y, -1f), Quaternion.identity);
        if (newCandy == null)
        {
            Debug.LogError("Failed to instantiate new candy.");
            return;
        }
        newCandy.transform.SetParent(_candyParent.transform);

        _candiesArray[i, j] = newCandy;
        Candy newCandyScript = newCandy.GetComponent<Candy>();
        newCandyScript.PosInArrayI = i;
        newCandyScript.PosInArrayJ = j;
        newCandyScript.PosX = newCandy.transform.position.x;
        newCandyScript.PosY = newCandy.transform.position.y;
        Debug.Log($"Fixed match at position: I: {i} J: {j}");

        // Check for missing candies after replacement
        if (_candiesArray[i, j] == null)
        {
            Debug.LogError($"Candy missing at position X: {i}, Y: {j} after fixing match.");
        }
        //}

    }

    
}
