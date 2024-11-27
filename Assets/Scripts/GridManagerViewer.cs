using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using TreeEditor;
//using UnityEditor.Experimental.GraphView;
//using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridManagerViewer : MonoBehaviour, IGridManagerViewer
{
    private GameObject gridCellGO;
    //private int gridSize;
    //private int candyTypes;
    public GameObject[,] gridCellsArray;
    public GameObject[,] CandiesArray { get; set; }
    private GameObject gridParent;
    private GameObject candyParent;
    private GridCell gridCellScript;
    public GameSettings gameSettings;
    private System.Random random = new System.Random();

    //private List<List<GameObject>> ListsOfColors = new List<List<GameObject>>();
    private GameObject candyPoolGO;
    public CandyPool candyPoolScript;
    private GameObject movementViewer;
    private MovementViewer movementViewerScript;
    private GridManagerModel gridManagerModel;
    [SerializeField] private GameObject backgroundImage;
    private AnchorsManager anchorsManager;
    [SerializeField] private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        candyPoolGO = Instantiate(Resources.Load<GameObject>("Prefabs/CandyPoolPrefab"));
        candyPoolScript = candyPoolGO.GetComponent<CandyPool>();
        gridParent = new GameObject("GridParent");
        candyParent = new GameObject("CandyParent");
        gridCellGO = Instantiate(Resources.Load<GameObject>("Prefabs/GridCellPrefab"));
        gridCellGO.SetActive(false);
        gridCellsArray = new GameObject[gameSettings.tilesNumberI, gameSettings.tilesNumberJ];
        CandiesArray = new GameObject[gameSettings.tilesNumberI, gameSettings.tilesNumberJ];
        gridManagerModel = new GridManagerModel(gameSettings, this, gridCellsArray);
        Vector2 firstTilePos = gridManagerModel.CalculateFirstTileXY(gameSettings.tilesNumberI, gameSettings.tilesNumberJ, gameSettings.tileSize);
        gridManagerModel.PopulateBackdropGrid(gridCellGO, firstTilePos, CandiesArray);
        MatchHandlerViewer.Instance.Initialize(gameSettings, CandiesArray, candyParent, candyPoolGO);
        movementViewerScript = Instantiate(Resources.Load<GameObject>("Prefabs/MovementControllerPrefab")).GetComponent<MovementViewer>();
        movementViewerScript.Initialize(candyPoolScript, audioManager);
        anchorsManager = backgroundImage.GetComponent<AnchorsManager>();
        anchorsManager.Initialize(gridCellsArray, gameSettings);
        anchorsManager.FindGridObjectsInCorners();
        // option 1 passed as paramerer. CheckAndFixAllMatches will use FixMatch();
        StartCoroutine(MatchHandlerViewer.Instance.MatchHandlerModel.CheckAndFixAllMatches(true));
        audioManager.PlayBackgroundMusic("Background Music");
    }
    //j corresponds to the row index(vertical position), which is equivalent to the X-coordinate
    //i corresponds to the column index(horizontal position), which is equivalent to the Y-coordinate
    
    public GameObject SetGridCellPosition(Vector2 firstTilePos, int i, int j)
    {
        Vector2 position = new Vector2(firstTilePos.x + j * gameSettings.tileSize, firstTilePos.y - i * gameSettings.tileSize);
        gridCellGO = InstantiateGridCell();
        gridCellGO.transform.position = position;
        gridCellGO.transform.localScale = new Vector3(gameSettings.tileSize, gameSettings.tileSize, 1);
        return gridCellGO;
    }
    public void InitializeGridCell(int i, int j)
    {
        gridCellScript = gridCellGO.GetComponent<GridCell>();
        gridCellScript.PosX = gridCellGO.transform.position.x;
        gridCellScript.PosY = gridCellGO.transform.position.y;
        gridCellScript.PosInArrayJ = j;
        gridCellScript.PosInArrayI = i;
    }
    public GameObject InstantiateGridCell()
    {
        GameObject gridCell = Instantiate(Resources.Load<GameObject>("Prefabs/GridCellPrefab"));
        return gridCell;
    }
    public void SetGridCellParent(GameObject gridCellGO)
    {
        gridCellGO.transform.SetParent(gridParent.transform);
    }
    public void SetCandyParent(GameObject candyCellGO)
    {
        candyCellGO.transform.SetParent(candyParent.transform);
    }

    public CandyType DetermineCandyType()
    {
        int randomIndex = random.Next(gameSettings.CandyTypesLevel1);
        return gameSettings.candies[randomIndex].GetComponent<CandyViewer>().CandyType;
    }
    public CandyViewer SetCandyInPlace(CandyType candyType)
    {
        Vector3 position = gridCellGO.transform.position;
        GameObject randomCandy = candyPoolScript.GetCandy(candyType);
        CandyViewer randomCandyScript = randomCandy.GetComponent<CandyViewer>();
        Vector3 newPosition = new Vector3(position.x, position.y, -1);
        randomCandyScript.SetPhysicalPosition(newPosition);
        SetCandyParent(randomCandy);
        return randomCandyScript;
    }

    public void PopulateBackdropGrid(Vector2 firstTilePos, GameObject gridCellGO, GameObject[,] candiesArray)
    {
        for (int i = 0; i < gameSettings.tilesNumberI; i++)
        {
            for (int j = 0; j < gameSettings.tilesNumberJ; j++)
            {
                Vector2 position = new Vector2(firstTilePos.x + j * gameSettings.tileSize, firstTilePos.y - i * gameSettings.tileSize);
                gridCellGO = InstantiateGridCell();
                gridCellGO.transform.position = position;
                gridCellGO.transform.localScale = new Vector3(gameSettings.tileSize, gameSettings.tileSize, 1);
                SetGridCellParent(gridCellGO);
                gridCellsArray[i, j] = gridCellGO;
                //
                gridCellScript = gridCellGO.GetComponent<GridCell>();
                gridCellScript.PosX = gridCellGO.transform.position.x;
                gridCellScript.PosY = gridCellGO.transform.position.y;
                gridCellScript.PosInArrayJ = j;
                gridCellScript.PosInArrayI = i;
                //
                //Debug.Log("Cell in position: X: " + gridCellScript.PosX + " Y: " + gridCellScript.PosY);
                //Debug.Log($"GridCell Created at ({i}, {j}): PosInArrayJ = {gridCellScript.PosInArrayJ}, PosInArrayI = {gridCellScript.PosInArrayI}");
                PopulateCandiesArray(position, gridCellGO, candiesArray, i, j);

            }
        }
    }
    private void PopulateCandiesArray(Vector3 position, GameObject gridCellGO, GameObject[,] candiesArray, int i, int j)
    {
        CandyType candyType = DetermineCandyType();
        GameObject randomCandy = candyPoolScript.GetCandy(candyType);
        CandyViewer randomCandyScript = randomCandy.GetComponent<CandyViewer>();
        position = new Vector3(position.x, position.y, -1);
        randomCandyScript.SetArrayPosition(randomCandy, candiesArray, i, j);
        randomCandyScript.SetPhysicalPosition(position);
        SetCandyParent(randomCandy);
    }
    public void RestartGame()
    {
        Destroy(GameObject.Find("PersistentObject"));
        // Reload the current scene
        SceneManager.LoadScene("SampleScene");
    }
}

public interface IGridManagerViewer
{
    public GameObject[,] CandiesArray { get; set; }
}