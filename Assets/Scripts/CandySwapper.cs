using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandySwapper : MonoBehaviour
{
    private Candy _selectedCandy;
    private GridManager _gridManager;
    public List<GameObject> matchesVer = new List<GameObject>();
    public List<GameObject> matchesHor = new List<GameObject>();
    private GameSettings _gameSettings;
    private CandyPool _candyPool;
    private bool scoreManagerInitialized = false;
    private static CandySwapper instance;
    public static CandySwapper Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("CandySwapper");
                instance = go.AddComponent<CandySwapper>();
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
    public void Initialize( Candy selectedCandy, GridManager gridManager, GameSettings gameSettings, CandyPool candyPool)
    {
        _selectedCandy = selectedCandy;
        _gridManager = gridManager;
        _gameSettings = gameSettings;
        _candyPool = candyPool;
    }
    public void SwapCandies(Vector2 direction)
    {
        if (_selectedCandy != null)
        {

            // Get the current grid coordinates (I, J) of the selected candy
            int currentI = _selectedCandy.PosInArrayI;
            int currentJ = _selectedCandy.PosInArrayJ;

            // Determine new coordinates based on direction
            int newI = currentI;
            int newJ = currentJ;
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

            // Ensure the new position is within grid bounds
            if (newI >= 0 && newI < _gridManager.gameSettings.tilesNumberI && newJ >= 0 && newJ < _gridManager.gameSettings.tilesNumberJ)
            {
                // Get the second candy to be swapped with the selected candy
                Candy secondCandy = _gridManager.candiesArray[newI, newJ].GetComponent<Candy>();

                // Swap the candies in the array
                _gridManager.candiesArray[currentI, currentJ] = secondCandy.gameObject;
                _gridManager.candiesArray[newI, newJ] = _selectedCandy.gameObject;

                // Update their properties
                secondCandy.PosInArrayI = currentI;
                secondCandy.PosInArrayJ = currentJ;

                _selectedCandy.PosInArrayI = newI;
                _selectedCandy.PosInArrayJ = newJ;

                matchesHor.Clear();
                matchesVer.Clear();

                bool horizontalCheck = PreMovementChecks.Instance.CheckRowAndColumn(_selectedCandy.gameObject, _gridManager.candiesArray, true, out matchesHor);
                bool verticalCheck = PreMovementChecks.Instance.CheckRowAndColumn(_selectedCandy.gameObject, _gridManager.candiesArray, false, out matchesVer);
                //List<GameObject> combinedMatches = new List<GameObject>(matchesHor);
                //combinedMatches.AddRange(matchesVer);


                if (horizontalCheck || verticalCheck)
                {
                    // Swap their world positions
                    Vector3 selectedCandyTargetPos = _gridManager.gridCellsArray[newI, newJ].transform.position;
                    Vector3 secondCandyTargetPos = _gridManager.gridCellsArray[currentI, currentJ].transform.position;

                    _selectedCandy.transform.position = selectedCandyTargetPos;
                    secondCandy.transform.position = secondCandyTargetPos;

                    Vector3 selectedCandyRaisedPos = new Vector3(_selectedCandy.transform.position.x, _selectedCandy.transform.position.y, -1f);
                    Vector3 secondCandyRaisedPos = new Vector3(secondCandy.transform.position.x, secondCandy.transform.position.y, -1f);
                    _selectedCandy.transform.position = selectedCandyRaisedPos;
                    secondCandy.transform.position = secondCandyRaisedPos;
                    
                    List<GameObject> rotationList = new List<GameObject>();
                    rotationList = CandyAnimationsController.Instance.CreateRotationList(matchesHor, matchesVer, _gameSettings, _candyPool);
                    if (!scoreManagerInitialized)
                    {
                        ScoreManager.Instance.Initialize(_gameSettings);
                        scoreManagerInitialized = true;
                    }

                    StartCoroutine(ScoreManager.Instance.AddPoints(rotationList));
                    RotationCoroutineWrapper(rotationList);
                    //StartCoroutine(RotationCoroutineWrapper(rotationList));
                    DestroyFirstMatches();
                    
                }
                else
                {
                    Debug.Log("No match, swapping back the array and candy positions.");
                    _gridManager.candiesArray[currentI, currentJ] = _selectedCandy.gameObject;
                    _gridManager.candiesArray[newI, newJ] = secondCandy.gameObject;
                    _selectedCandy.PosInArrayI = currentI;
                    _selectedCandy.PosInArrayJ = currentJ;
                    secondCandy.PosInArrayI = newI;
                    secondCandy.PosInArrayJ = newJ;
                }
            }
            // Deselect the candy after moving
            _selectedCandy = null;
        }
    }
    
    private void RotationCoroutineWrapper(List <GameObject> clonedList)
    {
        foreach (GameObject candy in clonedList)
        {
            StartCoroutine(CandyAnimationsController.Instance.RotateMatchingCandies(candy, _gameSettings.rotationDuration, _gameSettings.numberOfRotations));
            //_candyPool.ReturnCandy(candy);
        }
    }
    private void DestroyFirstMatches()
    {
        if (matchesHor.Count >= 3)
        {
            DestroyMatches.Instance.ReturnMatchesInList(matchesHor);
        }
        matchesHor.Clear();
        if (matchesVer.Count >= 3)
        {
            DestroyMatches.Instance.ReturnMatchesInList(matchesVer);
        }
        matchesVer.Clear();
    }
        
    

}
