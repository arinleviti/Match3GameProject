using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Codice.CM.Common.CmCallContext;




public class CandySwapperViewer :  MonoBehaviour
{
    private CandyViewer _selectedCandy;
    private GridManagerViewer _gridManager;
    //public List<GameObject> matchesVer = new List<GameObject>();
    //public List<GameObject> matchesHor = new List<GameObject>();
    public GameSettings _gameSettings; 
    private CandyPool _candyPool;
    
    public CandySwapperModel SwapperModel { get; private set; }
    private static CandySwapperViewer instance;
    public static CandySwapperViewer Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("CandySwapperViewer");
                instance = go.AddComponent<CandySwapperViewer>();
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
            Destroy(gameObject);
        }
    }
    public void Initialize( CandyViewer selectedCandy, GridManagerViewer gridManager, GameSettings gameSettings, CandyPool candyPool)
    {
        _selectedCandy = selectedCandy;
        _gridManager = gridManager;
        _gameSettings = gameSettings;
        _candyPool = candyPool;
        SwapperModel = new CandySwapperModel(_selectedCandy, _gridManager, _gameSettings, _candyPool, this);
    }
  
    public void RotationCoroutineWrapper(List <GameObject> clonedList)
    {
        foreach (GameObject candy in clonedList)
        {
            StartCoroutine(CandyAnimationsController.Instance.RotateMatchingCandies(candy, _gameSettings.rotationDuration, _gameSettings.numberOfRotations));
            //_candyPool.ReturnCandy(candy);
        }
    }
    public Vector2 DetermineCoordinatesFromDirection(Vector2 direction, int newI, int newJ)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // Move horizontally (left or right)
            if (direction.x > 0)
            {
                newJ++; // Move right               
            }
            else if (direction.x < 0)
            {
                newJ--; // Move left
            }
        }
        else
        {
            // Move vertically (up or down)
            if (direction.y > 0)
            {
                newI--; // Move up
            }
            else if (direction.y < 0)
            {
                newI++; // Move down
            }
        }
        Vector2 newCoordinates = new Vector2(newI,newJ);
        return newCoordinates;
    }
    public CandyViewer GetCandyComponent(int newI, int newJ )
    {
        CandyViewer secondCandy =  _gridManager.CandiesArray[newI, newJ].GetComponent<CandyViewer>();
        return secondCandy;
    }
    public void SwapPositions(int newI, int newJ, int currentI, int currentJ, CandyViewer secondCandy)
    {
        // Swap their world positions
        Vector3 selectedCandyTargetPos = _gridManager.gridCellsArray[newI, newJ].transform.position;
        Vector3 secondCandyTargetPos = _gridManager.gridCellsArray[currentI, currentJ].transform.position;

        _selectedCandy.SetPhysicalPosition(selectedCandyTargetPos);
        secondCandy.SetPhysicalPosition(secondCandyTargetPos);
    }
    public void InitializeScoreManager(GameSettings gameSettings)
    {
        ScoreManager.Instance.Initialize(gameSettings);
    }
    public void AddPoints(List<GameObject> rotationList)
    {
        ScoreManager.Instance.AddPoints(rotationList);
    }
    public (List<GameObject> CheckRowAndColumn, bool horVerCheck) CheckRowAndColumns (GameObject selectedCandy, GameObject[,] candiesArray, bool isHorizontal, List<GameObject> matchesHorVer)
    {
        bool horVerCheck = PreMovementChecksViewer.Instance._preMovementChecksModel.CheckRowAndColumn(_selectedCandy.gameObject, _gridManager.CandiesArray, isHorizontal, out matchesHorVer);
        return (matchesHorVer, horVerCheck);
    }
    public List<GameObject> CreateRotationList(List<GameObject> matchesHor, List<GameObject> matchesVer, GameSettings gameSettings, CandyPool candyPool)
    {
        List<GameObject> rotationList = CandyAnimationsController.Instance.CreateRotationList(matchesHor, matchesVer, gameSettings, candyPool);
        return rotationList;
    }
}
