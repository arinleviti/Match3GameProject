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

    private GridModel gridModel;

    // Start is called before the first frame update
    void Start()
    {
        gridModel = new GridModel();
        candyPoolGO = Instantiate(Resources.Load<GameObject>("Prefabs/CandyPoolPrefab"));
        candyPoolScript = candyPoolGO.GetComponent<CandyPool>();
        gridParent = new GameObject("GridParent");
        candyParent = new GameObject("CandyParent");
        Vector2 firstTilePos =
            gridModel.CalculateFirstTileXY(gameSettings.tilesNumberI, gameSettings.tilesNumberJ, gameSettings.tileSize);
        gridCellsArray = new GameObject[gameSettings.tilesNumberI, gameSettings.tilesNumberJ];
        candiesArray = new GameObject[gameSettings.tilesNumberI, gameSettings.tilesNumberJ];
        PopulateBackdropGrid(gridCellScript, firstTilePos);
        MatchHandler.Instance.Initialize(gameSettings, candiesArray, candyParent, candyPoolGO);
        // option 1 passed as paramerer. CheckAndFixAllMatches will use FixMatch();
        StartCoroutine(MatchHandler.Instance.CheckAndFixAllMatches(true));
        Debug.Log($"candiesArray dimensions: {candiesArray.GetLength(0)} x {candiesArray.GetLength(1)}");
        //DebugLogCandyArray();
    }

    //j corresponds to the row index (vertical position), which is equivalent to the X-coordinate
    //i corresponds to the column index (horizontal position), which is equivalent to the Y-coordinate
    private void PopulateBackdropGrid(GridCell gridCellScript, Vector2 firstTilePos)
    {
        for (int i = 0; i < gameSettings.tilesNumberI; i++)
        {
            for (int j = 0; j < gameSettings.tilesNumberJ; j++)
            {
                Vector2 position = new Vector2(firstTilePos.x + j * gameSettings.tileSize,
                    firstTilePos.y - i * gameSettings.tileSize);
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
                //Debug.Log("Cell in position: X: " + gridCellScript.PosX + " Y: " + gridCellScript.PosY);
                //Debug.Log($"GridCell Created at ({i}, {j}): PosInArrayJ = {gridCellScript.PosInArrayJ}, PosInArrayI = {gridCellScript.PosInArrayI}");
                PopulateCandiesArray(position, gridCellGO, i, j);
            }
        }
    }

    private void PopulateCandiesArray(Vector3 position, GameObject gridCellGO, int i, int j)
    {
        CandyType candyType = DetermineCandyType();
        GameObject randomCandy = candyPoolScript.GetCandy(candyType);
        Candy randomCandyScript = randomCandy.GetComponent<Candy>();
        position = new Vector3(position.x, position.y, -1);
        randomCandyScript.SetArrayPosition(randomCandy, candiesArray, i, j);
        randomCandyScript.SetPhysicalPosition(randomCandy, position);
        randomCandy.transform.SetParent(candyParent.transform);
    }

    private CandyType DetermineCandyType()
    {
        int randomIndex = random.Next(gameSettings.candies.Count);
        return gameSettings.candies[randomIndex].GetComponent<Candy>().CandyType;
    }
}