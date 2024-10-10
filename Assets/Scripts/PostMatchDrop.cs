using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PostMatchDrop : MonoBehaviour
{
    //public MatchHandler MatchHandler;
    private static PostMatchDrop instance;
    private CandyPool _candyPool;
    private GameSettings _gameSettings;
    private GridManager _gridManagerGO;
    private GameObject _movementControllerGO;
    private MovementController _movementController;
    //private List<(int,int)> droppingCandiesCoordinates = new List<(int,int)> ();
    public MatchHandler MatchHandler { get; set; }
    public static PostMatchDrop Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("PostMatchDrop");
                instance = go.AddComponent<PostMatchDrop>();
                DontDestroyOnLoad(go); // Ensure this new GameObject persists
                Debug.Log("PostMatchDrop instance created.");
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instance
        }
    }

    public void Initialize(MatchHandler matchHandler, CandyPool candyPool, GameSettings gameSettings, GridManager gridManager, GameObject movementController)
    {
        MatchHandler = matchHandler;
        _candyPool = candyPool;
        _gameSettings = gameSettings;
        _gridManagerGO = gridManager;
        _movementControllerGO = movementController;
        _movementController = _movementControllerGO.GetComponent<MovementController>();
        _movementController.OnMovePerformedComplete += HandlePostMoveCheck;
    }
    public void HandlePostMoveCheck()
    {
        PostMovementMatchCheck();
        ScanGridforEmptyTiles();

    }
    public void PostMovementMatchCheck()
    {
        // useFixMatch = false passed as paramerer. CheckAndFixAllMatches will use CheckRowAndColumn();
        MatchHandler.Instance.CheckAndFixAllMatches(false);
        
    }  
    //After the matches have been found and destroyed, this stores the empty tiles' detail. 
    public void ScanGridforEmptyTiles()
    {
        //droppingCandiesCoordinates.Clear();
        // Check columns for nulls, from bottom to top.
        for (int j = 0; j < _gameSettings.tilesNumberJ; j++)
        {
            int dropIndex = 1;
            for (int i = _gameSettings.tilesNumberI-2; i >= 0; i--)
            {               
                if (_gridManagerGO.candiesArray[i, j] != null && _gridManagerGO.candiesArray[i+1, j] == null)
                {                                      
                    DropCandy(_gridManagerGO.candiesArray[i, j], dropIndex);
                    
                }
                else if (_gridManagerGO.candiesArray[i, j] == null && _gridManagerGO.candiesArray[i + 1, j] == null)
                {  
                    dropIndex++;
                }
                else if (_gridManagerGO.candiesArray[i, j] == null && _gridManagerGO.candiesArray[i + 1, j] != null)
                {
                    continue;
                }
                else if ((_gridManagerGO.candiesArray[i, j] != null && _gridManagerGO.candiesArray[i + 1, j] != null))
                {
                    continue;
                }
            }
        }
    }

    public void DropCandy(GameObject candy, int dropIndex)
    {
        Candy candyScript = candy.GetComponent<Candy>();
        int oldPositionI = candyScript.PosInArrayI;
        int oldPositionJ = candyScript.PosInArrayJ;
        _gridManagerGO.candiesArray[candyScript.PosInArrayI + dropIndex, candyScript.PosInArrayJ] = candy;
        candyScript.PosInArrayI = candyScript.PosInArrayI + dropIndex;
        GameObject gridCellUnderCandy = _gridManagerGO.gridCellsArray[candyScript.PosInArrayI, candyScript.PosInArrayJ];
        GridCell gridCellScript = gridCellUnderCandy.GetComponent<GridCell>();
        candyScript.PosX = gridCellScript.PosX;
        candyScript.PosY = gridCellScript.PosY;
        Vector3 newPosition = new Vector3 (gridCellScript.PosX, gridCellScript.PosY, -1);
        //candy.transform.position = newPosition;
        _gridManagerGO.candiesArray[oldPositionI, oldPositionJ] = null;
        StartCoroutine(CandyAnimationsController.Instance.MoveCandy(candy, newPosition, _gameSettings.dropSpeed));
        Debug.Log($" Candy Dropped: {candyScript.CandyType}, from {oldPositionI}, {oldPositionJ} to {candyScript.PosInArrayI},  {candyScript.PosInArrayJ} usind drop index {dropIndex}");
    }
}
