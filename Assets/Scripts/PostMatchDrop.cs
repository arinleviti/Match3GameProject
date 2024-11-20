using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


public class PostMatchDrop : MonoBehaviour
{
    //public MatchHandlerViewer MatchHandlerViewer;
    private static PostMatchDrop instance;
    private CandyPool _candyPool;
    private GameSettings _gameSettings;
    private GridManagerViewer _gridManagerGO;
    private GameObject _movementViewerGO;
    private MovementViewer _movementViewer;
    //private List<(int,int)> droppingCandiesCoordinates = new List<(int,int)> ();
    private bool keepChecking = true;

    public MatchHandlerViewer MatchHandlerViewer { get; set; }
    public static PostMatchDrop Instance
    {
        get
        {

            if (instance == null)
            {
                GameObject go = new GameObject("PostMatchDrop");
                instance = go.AddComponent<PostMatchDrop>();
                /*DontDestroyOnLoad(go);*/ // Ensure this new GameObject persists
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
        GridManagerViewer gridManager = _gridManagerGO.GetComponent<GridManagerViewer>();
        CandySpawnerViewer.Instance.Initialize(_gameSettings, gridManager, _candyPool);
    }

    public void Initialize(MatchHandlerViewer matchHandler, CandyPool candyPool, GameSettings gameSettings, GridManagerViewer gridManager, GameObject movementViewerGO)
    {
        MatchHandlerViewer = matchHandler;
        _candyPool = candyPool;
        _gameSettings = gameSettings;
        _gridManagerGO = gridManager;
        _movementViewerGO = movementViewerGO;
        _movementViewer = _movementViewerGO.GetComponent<MovementViewer>();
        _movementViewer.OnMovePerformedComplete += EventWrapper;
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
                yield return StartCoroutine(MatchHandlerViewer.Instance.MatchHandlerModel.CheckAndFixAllMatches(false));
                //Scans the grid for empty tiles and drops the tiles above the empty slots.
                yield return StartCoroutine(ScanGridforEmptyTiles());
            }
            //Spawns the candies
            CandySpawnerViewer.Instance.SpawnerModel.CheckEmptiesReplaceSpawn();
            //yield return StartCoroutine(CandySpawnerViewer.Instance.SpawnObjects());
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
                if (MatchHandlerViewer.MatchHandlerModel.IsMatch(i, j, i, j + 1, i, j + 2))
                {
                    return true;
                }
            }
        }
        for (int j = 0; j < _gameSettings.tilesNumberJ; j++)
        {
            for (int i = 0; i < _gameSettings.tilesNumberI - 2; i++)
            {
                if (MatchHandlerViewer.MatchHandlerModel.IsMatch(i, j, i + 1, j, i + 2, j))
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
                if (_gridManagerGO.CandiesArray[i, j] != null && _gridManagerGO.CandiesArray[i + 1, j] == null)
                {
                    keepChecking = true;
                    yield return StartCoroutine(DropCandy(_gridManagerGO.CandiesArray[i, j], dropIndex));
                }
                else if (_gridManagerGO.CandiesArray[i, j] == null && _gridManagerGO.CandiesArray[i + 1, j] == null)
                {
                    dropIndex++;
                }
                else if (_gridManagerGO.CandiesArray[i, j] == null && _gridManagerGO.CandiesArray[i + 1, j] != null)
                {
                    continue;
                }
                else if ((_gridManagerGO.CandiesArray[i, j] != null && _gridManagerGO.CandiesArray[i + 1, j] != null))
                {
                    continue;
                }
                else Debug.Log("Drop type can't be determined.");
            }
        }
        yield return null;
    }

    public IEnumerator DropCandy(GameObject candy, int dropIndex)
    {
        CandyViewer candyScript = candy.GetComponent<CandyViewer>();
        int oldPositionI = candyScript.CandyModel.PosInArrayI;
        int oldPositionJ = candyScript.CandyModel.PosInArrayJ;

        int newPositionI = candyScript.CandyModel.PosInArrayI + dropIndex;
        int newPositionJ = candyScript.CandyModel.PosInArrayJ;
        Vector3 oldPosition = candy.transform.position;

        candyScript.SetArrayPosition(candy, _gridManagerGO.CandiesArray, newPositionI, newPositionJ);
        candyScript.SetPhysicalPosition(_gridManagerGO.gridCellsArray[candyScript.CandyModel.PosInArrayI, candyScript.CandyModel.PosInArrayJ].transform.position);

        GameObject gridCellUnderCandy = _gridManagerGO.gridCellsArray[candyScript.CandyModel.PosInArrayI, candyScript.CandyModel.PosInArrayJ];
        GridCell gridCellScript = gridCellUnderCandy.GetComponent<GridCell>();
        Vector3 newPosition = new Vector3(gridCellScript.PosX, gridCellScript.PosY, -1);
        yield return StartCoroutine(CandyAnimationsController.Instance.MoveCandy(candy, oldPosition, newPosition, _gameSettings.dropSpeed));


        _gridManagerGO.CandiesArray[oldPositionI, oldPositionJ] = null;


    }
    private void OnDestroy()
    {
        if (_movementViewer != null)
        {
            _movementViewer.OnMovePerformedComplete -= EventWrapper;
        }
    }

    public void CheckCandiesArrayForNulls()
    {

        int rows = _gridManagerGO.CandiesArray.GetLength(0); // Number of rows (I dimension)
        int columns = _gridManagerGO.CandiesArray.GetLength(1); // Number of columns (J dimension)

        // Iterate through the array and check for null values
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (_gridManagerGO.CandiesArray[i, j] == null)
                {
                    // Log the position of the null value
                    Debug.LogWarning($"Null found at position: ({i}, {j})");
                }
            }
        }
    }
}
