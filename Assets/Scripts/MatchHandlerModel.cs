using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchHandlerModel
{
    private bool keepLooking;
    public GameObject[,] _candiesArray;
    private IMatchHandlerViewer _matchHandlerViewer;
    private ICandyPool _candyPool;
    private GameObject _candyParent;
    public List<GameObject> Matches { get; private set; } = new List<GameObject>();
    private GameSettings _gameSettings;

    
    public MatchHandlerModel(GameSettings gameSettings, GameObject[,] candiesArray, IMatchHandlerViewer matchHandlerViewer, ICandyPool candyPool, GameObject candyParent)
    {
        _gameSettings = gameSettings;
        _candiesArray = candiesArray;
        _matchHandlerViewer = matchHandlerViewer;
        _candyPool = candyPool;
        _candyParent = candyParent;
    }
    public IEnumerator CheckAndFixAllMatches(bool useFixMatch)
    {
        bool foundMatch;
        keepLooking = true;
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
                            //Debug.Log($"matches found at:  {i}, {j} and  {i}, {j+1} and {i}, {j +2} ");
                            bool isMatch = PreMovementChecksViewer.Instance._preMovementChecksModel.CheckRowAndColumn(_candiesArray[i, j + 1], _candiesArray, true, out tempMatches);
                            if (isMatch)
                            {
                                AddToMatchList(tempMatches);
                                ScoreManagerViewer.Instance.AddPoints(tempMatches);
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
                            //Debug.Log($"matches found at:  {i}, {j} and  {i + 1}, {j} and {i + 2}, {j} ");
                            bool isMatch = PreMovementChecksViewer.Instance._preMovementChecksModel.CheckRowAndColumn(_candiesArray[i + 1, j], _candiesArray, false, out tempMatches);
                            if (isMatch)
                            {
                                AddToMatchList(tempMatches);
                                ScoreManagerViewer.Instance.AddPoints(tempMatches);
                            }

                        }

                        foundMatch = true;
                    }
                }

            }
            if (Matches.Count >= _gameSettings.candiesToMatch)
            {
                List<GameObject> emptyList = new List<GameObject>();
                List<GameObject> clonedList = CandyAnimationsController.Instance.CreateRotationList(Matches, emptyList, _gameSettings, _candyPool);
                foreach (GameObject candy in clonedList)
                {
                    _matchHandlerViewer.CoroutineWrapper(candy);
                   
                }
                DestroyMatches.Instance.ReturnMatchesInList(Matches);
                Matches.Clear();
                keepLooking = true;
            }
            //else 
            //{
            //    keepLooking = false;
            //}
        } while (foundMatch);
        if (Matches.Count < _gameSettings.candiesToMatch)
        {
            keepLooking = false;
        }
        yield return null;
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

    public bool IsMatch(int x1, int y1, int x2, int y2, int x3, int y3)
    {
        if (_candiesArray[x1, y1] != null && _candiesArray[x2, y2] != null && _candiesArray[x3, y3] != null)
        {
            CandyViewer c1 = _matchHandlerViewer.GetCandyComponent(_candiesArray[x1, y1]);
            CandyViewer c2 = _matchHandlerViewer.GetCandyComponent(_candiesArray[x2, y2]);
            CandyViewer c3 = _matchHandlerViewer.GetCandyComponent(_candiesArray[x3, y3]);

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
        GameObject[] prefabs = _matchHandlerViewer.LoadAllPrefabs(folderPath);
        if (prefabs.Length == 0)
        {
            Debug.LogError("No prefabs found in the specified path: " + folderPath);
            return;
        }

        CandyViewer oldCandyScript = _matchHandlerViewer.GetCandyComponent(oldCandy);
        List<GameObject> availablePrefabs = new List<GameObject>();

        // Print all candy types for debugging
        Debug.Log("Old candy type: " + oldCandyScript.CandyType);

        foreach (GameObject prefab in prefabs)
        {
            CandyViewer candyPrefab = _matchHandlerViewer.GetCandyComponent(prefab);
            /*Debug.Log("Prefab candy type: " + candyPrefab.CandyType);*/ // Debugging output
            if (candyPrefab.CandyType != oldCandyScript.CandyType && (int)candyPrefab.CandyType<_gameSettings.CandyTypesLevel1)
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
        GameObject newCandyPrefab = _matchHandlerViewer.SelectRandomPrefab(availablePrefabs);
        Vector3 position = _matchHandlerViewer.TransferPositionAndScale(oldCandy, newCandyPrefab);

        _candyPool.ReturnCandy(oldCandy);
        CandyType newCandyType = _matchHandlerViewer.GetCandyComponent(newCandyPrefab).CandyType;
        
        GameObject newCandy = _candyPool.GetCandy(newCandyType);

        if (newCandy == null)
        {
            Debug.LogError("Failed to instantiate new candy.");
            return;
        }
        _matchHandlerViewer.SetCandyParent(newCandy, _candyParent);

        CandyViewer newCandyScript = _matchHandlerViewer.GetCandyComponent(newCandy);
        //CandyViewer newCandyScript = newCandy.GetComponent<CandyViewer>();

        newCandyScript.SetArrayPosition(newCandy, _candiesArray, i, j);
        newCandyScript.SetPhysicalPosition(position);


        // Check for missing candies after replacement
        if (_candiesArray[i, j] == null)
        {
            Debug.LogError($"Candy missing at position X: {i}, Y: {j} after fixing match.");
        }


    }
}
