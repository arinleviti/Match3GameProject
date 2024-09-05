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
    //private int gridSize;
    //private int candyTypes;
    private GameObject[,] gridCellsArray;
    private GameObject[,] candiesArray;
    private GameObject gridParent;
    private GameObject candyParent;
    private GridCell gridCellScript;
    public GameSettings gameSettings;
    private System.Random random = new System.Random();

    //private List<List<GameObject>> ListsOfColors = new List<List<GameObject>>();
    private GameObject candyPoolGO;
    private CandyPool candyPoolScript;

    // Start is called before the first frame update
    void Start()
    {
        candyPoolGO = Instantiate(Resources.Load<GameObject>("Prefabs/CandyPoolPrefab"));
        candyPoolScript = candyPoolGO.GetComponent<CandyPool>();
        //candyTypes = gameSettings.candyTypes.Count;
        gridParent = new GameObject("GridParent");
        candyParent = new GameObject("CandyParent");
        Vector2 firstTilePos = CalculateFirstTileXY(gameSettings.tilesNumberX, gameSettings.tilesNumberY, gameSettings.tileSize);
        //gridSize = gameSettings.tilesNumberHor * gameSettings.tilesNumberVert;
        gridCellsArray = new GameObject[gameSettings.tilesNumberX, gameSettings.tilesNumberY];
        //int perLine = Mathf.FloorToInt(gameSettings.tilesNumberHor /gameSettings.tileSize);
        candiesArray = new GameObject[gameSettings.tilesNumberX, gameSettings.tilesNumberY];
        PopulateBackdropGrid(firstTilePos);
        MatchHandler.Instance.Initialize(gameSettings, candiesArray, candyParent);
        MatchHandler.Instance.CheckAndFixAllMatches();

        LogCandyQueueReferences();
    }
    //j corresponds to the row index (vertical position), which is equivalent to the X-coordinate
    //i corresponds to the column index (horizontal position), which is equivalent to the Y-coordinate
    private void PopulateBackdropGrid(Vector2 firstTilePos)
    {
        for (int i = 0; i < gameSettings.tilesNumberY; i++)
        {
            for (int j = 0; j < gameSettings.tilesNumberX; j++)
            {
                Vector2 position = new Vector2(firstTilePos.x + j * gameSettings.tileSize, firstTilePos.y - i * gameSettings.tileSize);
                gridCellGO = Instantiate(Resources.Load<GameObject>("Prefabs/GridCellPrefab"));
                gridCellGO.transform.position = position;
                gridCellGO.transform.localScale = new Vector3(gameSettings.tileSize, gameSettings.tileSize, 1);
                gridCellGO.transform.SetParent(gridParent.transform);
                gridCellsArray[j, i] = gridCellGO;
                gridCellScript = gridCellGO.GetComponent<GridCell>();
                gridCellScript.PosX = gridCellGO.transform.position.x;
                gridCellScript.PosY = gridCellGO.transform.position.y;
                Debug.Log("Cell in position: X: " + gridCellScript.PosX + " Y: " + gridCellScript.PosY);
                PopulateCandiesArray(position, gridCellGO, i, j);

            }
        }
    }
    private void PopulateCandiesArray(Vector2 position, GameObject gridCellGO, int i, int j)
    {
      
        CandyType candyType = DetermineCandyType();
        GameObject randomCandy = candyPoolScript.GetCandy(candyType);
        
        randomCandy.transform.position = position;
        randomCandy.transform.localScale = gridCellGO.transform.localScale * gameSettings.candyScaleFactor;
        candiesArray[j, i] = randomCandy;
        randomCandy.transform.SetParent(candyParent.transform);
        Candy randomCandyScript = randomCandy.GetComponent<Candy>();
        randomCandyScript.PosInArrayI = i;
        randomCandyScript.PosInArrayJ = j;
        randomCandyScript.PosX = randomCandy.transform.position.x;
        randomCandyScript.PosY = randomCandy.transform.position.y;
        //Debug.Log("Candy in position: X: " + randomCandyScript.PosInArrayX + " Y: " + randomCandyScript.PosInArrayY);
    }
    // Keeps the grind centered around the 0,0 cohordinates.
    private Vector2 CalculateFirstTileXY(float tilesNumberX, float tilesNumberY, float tileSize)
    {
        float tileX = -(tilesNumberX / 2)* tileSize + (tileSize/2) ;
        float tileY = (tilesNumberY / 2) * tileSize - (tileSize/2) ;
        Vector2 firstTilePosition = new Vector2(tileX, tileY);
        return firstTilePosition;
    }

    private CandyType DetermineCandyType()
    {
        int randomIndex = random.Next(gameSettings.candies.Count);
        return gameSettings.candies[randomIndex].GetComponent<Candy>().CandyType;
    }
    private void LogCandyQueueReferences()
    {
        foreach (var candyQueueEntry in candyPoolScript.candyQueues)
        {
            CandyType candyType = candyQueueEntry.Key;
            Queue<GameObject> candyQueue = candyQueueEntry.Value;

            Debug.Log($"Candy Type: {candyType}, Queue Size: {candyQueue.Count}");

            int index = 0;
            foreach (GameObject candyGO in candyQueue)
            {
                // Log the reference for each candy in the queue
                Debug.Log($"Queue Index {index}: Candy GameObject Reference: {candyGO.GetInstanceID()}");
                index++;
            }
        }
    }
}
