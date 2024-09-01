using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TreeEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private GameObject gridCellGO;
    private int gridSize;
    //private int candyTypes;
    private GameObject[,] gameObjectsArray;
    private GameObject[,] candiesArray;
    private GameObject gridParent;
    private GameObject candyParent;
    private GridCell gridCellScript;
    public GameSettings gameSettings;
    private System.Random random = new System.Random();

    private List<List<GameObject>> ListsOfColors = new List<List<GameObject>>();

    // Start is called before the first frame update
    void Start()
    {
        //candyTypes = gameSettings.candyTypes.Count;
        gridParent = new GameObject("GridParent");
        candyParent = new GameObject("CandyParent");
        Vector2 firstTilePos = CalculateFirstTileXY(gameSettings.tilesNumberHor, gameSettings.tileSize);
        gridSize = gameSettings.tilesNumberHor * gameSettings.tilesNumberVert;
        gameObjectsArray = new GameObject[gameSettings.tilesNumberHor, gameSettings.tilesNumberVert];
        int perLine = Mathf.FloorToInt(gameSettings.tilesNumberHor /gameSettings.tileSize);
        candiesArray = new GameObject[gameSettings.tilesNumberVert, gameSettings.tilesNumberHor];
        PopulateBackdropGrid(firstTilePos);
        //CheckForMatches();
        CheckAndFixAllMatches();
    }
    private void PopulateBackdropGrid(Vector2 firstTilePos)
    {
        for (int i = 0; i < gameSettings.tilesNumberVert; i++)
        {
            for (int j = 0; j < gameSettings.tilesNumberHor; j++)
            {
                Vector2 position = new Vector2(firstTilePos.x + j * gameSettings.tileSize, firstTilePos.y - i * gameSettings.tileSize);
                gridCellGO = Instantiate(Resources.Load<GameObject>("Prefabs/GridCellPrefab"));
                gridCellGO.transform.position = position;
                gridCellGO.transform.SetParent(gridParent.transform);
                gameObjectsArray[i, j] = gridCellGO;
                gridCellScript = gridCellGO.GetComponent<GridCell>();
                gridCellScript.x = i;
                gridCellScript.y = j;
                Debug.Log("Cell in position: X: " + gridCellScript.x + " Y: " + gridCellScript.y);
                PopulatecandiesArray(position, i, j);
                
            }
        }
    }
    private void PopulatecandiesArray(Vector2 position, int i, int j)
    {
        int randomIndex = random.Next(gameSettings.candies.Count);
        GameObject randomCandy = Instantiate(gameSettings.candies[randomIndex]);
        randomCandy.transform.position = position;
        candiesArray[i, j] = randomCandy;
        randomCandy.transform.SetParent(candyParent.transform);
        Candy randomCandyScript = randomCandy.GetComponent<Candy>();
        randomCandyScript.PosInArrayX = i;
        randomCandyScript.PosInArrayY = j;
        //Debug.Log("Candy in position: X: " + randomCandyScript.PosInArrayX + " Y: " + randomCandyScript.PosInArrayY);
    }
    // Keeps the grind centered around the 0,0 cohordinates.
    private Vector2 CalculateFirstTileXY(float tilesNumber, float tileSize)
    {
        float tileX = -(tilesNumber / 2) + (tileSize/2);
        float tileY = (tilesNumber / 2) - (tileSize/2);
        Vector2 firstTilePosition = new Vector2(tileX, tileY);
        return firstTilePosition;
    }

    private void CheckAndFixAllMatches()
    {
        bool foundMatch;

        do
        {
            foundMatch = false;

            // Check rows for matches
            for (int i = 0; i < gameSettings.tilesNumberVert; i++)
            {
                for (int j = 0; j < gameSettings.tilesNumberHor - 2; j++)
                {
                    if (IsMatch(i, j, i, j + 1, i, j + 2))
                    {
                        FixMatch(i, j + 1);  // Replace the middle one
                        foundMatch = true;
                    }
                }
            }

            // Check columns for matches
            for (int j = 0; j < gameSettings.tilesNumberHor; j++)
            {
                for (int i = 0; i < gameSettings.tilesNumberVert - 2; i++)
                {
                    if (IsMatch(i, j, i + 1, j, i + 2, j))
                    {
                        FixMatch(i + 1, j);  // Replace the middle one
                        foundMatch = true;
                    }
                }
            }

        } while (foundMatch);
    }

    private bool IsMatch(int x1, int y1, int x2, int y2, int x3, int y3)
    {
        if (candiesArray[x1, y1] != null &&
            candiesArray[x2, y2] != null &&
            candiesArray[x3, y3] != null)
        {
            Candy c1 = candiesArray[x1, y1].GetComponent<Candy>();
            Candy c2 = candiesArray[x2, y2].GetComponent<Candy>();
            Candy c3 = candiesArray[x3, y3].GetComponent<Candy>();

            return c1.CandyType == c2.CandyType && c1.CandyType == c3.CandyType;
        }
        return false;
    }
    private void FixMatch(int i, int j)
    {
        if (i < 0 || i >= gameSettings.tilesNumberVert || j < 0 || j >= gameSettings.tilesNumberHor)
        {
            Debug.LogWarning($"Invalid position for fixing match: X: {i}, Y: {j}");
            return; // Exit if position is out of bounds
        }
        GameObject oldCandy = candiesArray[i, j];
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
        Destroy(oldCandy);
        GameObject newCandy = Instantiate(newCandyPrefab, position, Quaternion.identity);
        if (newCandy == null)
        {
            Debug.LogError("Failed to instantiate new candy.");
            return;
        }
        newCandy.transform.SetParent(candyParent.transform);

        candiesArray[i, j] = newCandy;
        Candy newCandyScript = newCandy.GetComponent<Candy>();
        newCandyScript.PosInArrayX = i;
        newCandyScript.PosInArrayY = j;

        Debug.Log($"Fixed match at position: X: {i} Y: {j}");

        // Check for missing candies after replacement
        if (candiesArray[i, j] == null)
        {
            Debug.LogError($"Candy missing at position X: {i}, Y: {j} after fixing match.");
        }
    }
    //private void FixMatch(int i, int j)
    //{
    //    GameObject oldCandy = candiesArray[i, j];
    //    string folderPath = "Prefabs/CandyPrefabs";
    //    GameObject[] prefabs = Resources.LoadAll<GameObject>(folderPath);

    //    // Ensure that the new candy is not of the same type as the current candy to avoid creating a new match
    //    GameObject randomPrefab;
    //    do
    //    {
    //        randomPrefab = prefabs[UnityEngine.Random.Range(0, prefabs.Length)];
    //    } while (randomPrefab.GetComponent<Candy>().CandyType == oldCandy.GetComponent<Candy>().CandyType);

    //    Vector2 position = oldCandy.transform.position;
    //    Destroy(oldCandy);
    //    GameObject newCandy = Instantiate(randomPrefab, position, Quaternion.identity);
    //    newCandy.transform.SetParent(candyParent.transform);

    //    candiesArray[i, j] = newCandy;
    //    Candy newCandyScript = newCandy.GetComponent<Candy>();
    //    newCandyScript.PosInArrayX = i;
    //    newCandyScript.PosInArrayY = j;
    //}

}
