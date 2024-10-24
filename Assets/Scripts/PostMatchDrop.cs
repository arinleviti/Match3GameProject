using System;
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
    private bool keepChecking = true;

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

    private void Start()
    {
        GridManager gridManager = _gridManagerGO.GetComponent<GridManager>();
        CandySpawner.Instance.Initialize(_gameSettings, gridManager, _candyPool);
    }

    public void Initialize(MatchHandler matchHandler, CandyPool candyPool, GameSettings gameSettings, GridManager gridManager, GameObject movementController)
    {
        MatchHandler = matchHandler;
        _candyPool = candyPool;
        _gameSettings = gameSettings;
        _gridManagerGO = gridManager;
        _movementControllerGO = movementController;
        _movementController = _movementControllerGO.GetComponent<MovementController>();
        _movementController.OnMovePerformedComplete += EventWrapper;
    }
    private void EventWrapper()
    {
        StartCoroutine(HandlePostMoveCheck());
    }
    public IEnumerator HandlePostMoveCheck()
    {
        //This do-while loop stops when there are no more matches in the grid
        do
        {
            keepChecking = true; // Set to true to start checking for matches
            while (keepChecking)
            {
                //Checks for matches and destroys them.
                yield return StartCoroutine(MatchHandler.Instance.CheckAndFixAllMatches(false));
                //Scans the grid for empty tiles and drops the tiles above the empty slots.
                yield return StartCoroutine(ScanGridforEmptyTiles());
            }
            //Spawns the candies
            CandySpawner.Instance.CheckEmptiesReplaceSpawn();
            //yield return StartCoroutine(CandySpawner.Instance.SpawnObjects());
            //Debug.Log("Performed one cicle of post move checks");
        } while (CheckForMatches());
        //CheckCandiesArrayForNulls();
    }

    public bool CheckForMatches()
    {
        for (int i = 0; i < _gameSettings.tilesNumberI; i++)
        {
            for (int j = 0; j < _gameSettings.tilesNumberJ - 2; j++)
            {
                if (MatchHandler.IsMatch(i, j, i, j + 1, i, j + 2))
                {
                    return true;
                }
            }
        }
        for (int j = 0; j < _gameSettings.tilesNumberJ; j++)
        {
            for (int i = 0; i < _gameSettings.tilesNumberI - 2; i++)
            {
                if (MatchHandler.IsMatch(i, j, i + 1, j, i + 2, j))
                {
                    return true;
                }
            }
        }
        return false;
    }






    //After the matches have been found and destroyed, this stores the empty tiles' detail. 
    public IEnumerator ScanGridforEmptyTiles()
    {
        keepChecking = false;
        //droppingCandiesCoordinates.Clear();
        // Check columns for nulls, from bottom to top.
        for (int j = 0; j < _gameSettings.tilesNumberJ; j++)
        {
            int dropIndex = 1;
            for (int i = _gameSettings.tilesNumberI - 2; i >= 0; i--)
            {
                if (_gridManagerGO.candiesArray[i, j] != null && _gridManagerGO.candiesArray[i + 1, j] == null)
                {
                    keepChecking = true;
                    yield return StartCoroutine(DropCandy(_gridManagerGO.candiesArray[i, j], dropIndex));
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
                else  Debug.Log("Drop type can't be determined.");
            }
        }
        yield return null;
    }

    public IEnumerator DropCandy(GameObject candy, int dropIndex)
    {
        Candy candyScript = candy.GetComponent<Candy>();
        int oldPositionI = candyScript.PosInArrayI;
        int oldPositionJ = candyScript.PosInArrayJ;

        int newPositionI = candyScript.PosInArrayI + dropIndex;
        int newPositionJ = candyScript.PosInArrayJ;
        Vector3 oldPosition = candy.transform.position;
       
        candyScript.SetArrayPosition(candy, _gridManagerGO.candiesArray, newPositionI, newPositionJ);
        candyScript.SetPhysicalPosition(candy, _gridManagerGO.gridCellsArray[candyScript.PosInArrayI, candyScript.PosInArrayJ].transform.position);
        
        GameObject gridCellUnderCandy = _gridManagerGO.gridCellsArray[candyScript.PosInArrayI, candyScript.PosInArrayJ];
        GridCell gridCellScript = gridCellUnderCandy.GetComponent<GridCell>();
        Vector3 newPosition = new Vector3(gridCellScript.PosX, gridCellScript.PosY, -1);
        yield return StartCoroutine(CandyAnimationsController.Instance.MoveCandy(candy, oldPosition, newPosition, _gameSettings.dropSpeed));
        
        
        _gridManagerGO.candiesArray[oldPositionI, oldPositionJ] = null;
        

    }
    private void OnDestroy()
    {
        if (_movementController != null)
        {
            _movementController.OnMovePerformedComplete -= EventWrapper;
        }
    }

    public void CheckCandiesArrayForNulls()
    {
        
        int rows = _gridManagerGO.candiesArray.GetLength(0); // Number of rows (I dimension)
        int columns = _gridManagerGO.candiesArray.GetLength(1); // Number of columns (J dimension)

        // Iterate through the array and check for null values
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (_gridManagerGO.candiesArray[i, j] == null)
                {
                    // Log the position of the null value
                    Debug.LogWarning($"Null found at position: ({i}, {j})");
                }
            }
        }
    }
}
