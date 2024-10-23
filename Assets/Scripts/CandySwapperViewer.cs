using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandySwapperViewer : MonoBehaviour
{
    private CandyViewer _selectedCandy;
    private GridManagerViewer _gridManager;
    //public List<GameObject> matchesVer = new List<GameObject>();
    //public List<GameObject> matchesHor = new List<GameObject>();
    private GameSettings _gameSettings;
    private CandyPool _candyPool;
    //private bool scoreManagerInitialized = false;
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
    
}
