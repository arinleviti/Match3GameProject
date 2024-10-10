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
    public GameObject[,] gridCellsArray;
    public GameObject[,] candiesArray;
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
        gridParent = new GameObject("GridParent");
        candyParent = new GameObject("CandyParent");
        Vector2 firstTilePos = CalculateFirstTileXY(gameSettings.tilesNumberI, gameSettings.tilesNumberJ, gameSettings.tileSize);      
        gridCellsArray = new GameObject[gameSettings.tilesNumberI, gameSettings.tilesNumberJ];   
        candiesArray = new GameObject[gameSettings.tilesNumberI, gameSettings.tilesNumberJ];
        PopulateBackdropGrid(firstTilePos);
        MatchHandler.Instance.Initialize(gameSettings, candiesArray, candyParent, candyPoolGO);
        // option 1 passed as paramerer. CheckAndFixAllMatches will use FixMatch();
        MatchHandler.Instance.CheckAndFixAllMatches(true);
        Debug.Log($"candiesArray dimensions: {candiesArray.GetLength(0)} x {candiesArray.GetLength(1)}");
        DebugLogCandyArray();
    }
    //j corresponds to the row index (vertical position), which is equivalent to the X-coordinate
    //i corresponds to the column index (horizontal position), which is equivalent to the Y-coordinate
    private void PopulateBackdropGrid(Vector2 firstTilePos)
    {
        for (int i = 0; i < gameSettings.tilesNumberI; i++)
        {
            for (int j = 0; j < gameSettings.tilesNumberJ; j++)
            {
                Vector2 position = new Vector2(firstTilePos.x + j * gameSettings.tileSize, firstTilePos.y - i * gameSettings.tileSize);
                gridCellGO = Instantiate(Resources.Load<GameObject>("Prefabs/GridCellPrefab"));
                gridCellGO.transform.position = position;
                gridCellGO.transform.localScale = new Vector3(gameSettings.tileSize, gameSettings.tileSize, 1);
                gridCellGO.transform.SetParent(gridParent.transform);
                gridCellsArray[i, j] = gridCellGO;
                gridCellScript = gridCellGO.GetComponent<GridCell>();
                gridCellScript.PosX = gridCellGO.transform.position.x;
                gridCellScript.PosY = gridCellGO.transform.position.y;
                gridCellScript.PosInArrayJ = j;
                gridCellScript.PosInArrayI = i;
                Debug.Log("Cell in position: X: " + gridCellScript.PosX + " Y: " + gridCellScript.PosY);
                Debug.Log($"GridCell Created at ({i}, {j}): PosInArrayJ = {gridCellScript.PosInArrayJ}, PosInArrayI = {gridCellScript.PosInArrayI}");
                PopulateCandiesArray(position, gridCellGO, i, j);
                
            }
        }
    }
    private void PopulateCandiesArray(Vector3 position, GameObject gridCellGO, int i, int j)
    {
      
        CandyType candyType = DetermineCandyType();
        GameObject randomCandy = candyPoolScript.GetCandy(candyType);
        position = new Vector3(position.x, position.y, -1);
        randomCandy.transform.position = position;
        randomCandy.transform.localScale = gridCellGO.transform.localScale * gameSettings.candyScaleFactor;
        candiesArray[i, j] = randomCandy;
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

    public void DebugLogCandyArray()
    {
        // Iterate through the dimensions of the candiesArray
        for (int i = 0; i < gameSettings.tilesNumberI; i++)
        {
            for (int j = 0; j < gameSettings.tilesNumberJ; j++)
            {
                // Check if the current slot in candiesArray is not null
                if (candiesArray != null && i < candiesArray.GetLength(0) && j < candiesArray.GetLength(1))
                {
                    if (candiesArray[i, j] != null)
                    {
                        // Assuming each candy GameObject has a Candy component
                        Candy candy = candiesArray[i, j].GetComponent<Candy>();

                        if (candy != null)
                        {
                            // Log the position and type of candy
                            Debug.Log($"Candy at ({i}, {j}): Type = {candy.CandyType}, GameObject = {candiesArray[i, j].name}");
                        }
                        else
                        {
                            // Log if there's a GameObject but no Candy component
                            Debug.Log($"Candy at ({i}, {j}): GameObject present but no Candy component. GameObject = {candiesArray[i, j].name}");
                        }
                    }
                    else
                    {
                        // Log if the slot is empty
                        Debug.Log($"Candy at ({i}, {j}): No candy present.");
                    }
                }
                else
                {
                    // Log if the slot does not exist
                    Debug.Log($"Candy at ({i}, {j}): Slot does not exist.");
                }
            }
        }
    }
}
