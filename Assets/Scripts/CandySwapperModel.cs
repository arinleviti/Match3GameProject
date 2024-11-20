using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CandySwapperModel 
{
    private CandyViewer _selectedCandy;
    private GridManagerViewer _gridManager;
    public GameSettings _gameSettings; 
    private CandyPool _candyPool;
    private CandySwapperViewer _swapperViewer;
    public List<GameObject> matchesVer = new List<GameObject>();
    public List<GameObject> matchesHor = new List<GameObject>();
    private bool scoreManagerInitialized = false;
    private DestroyMatches _destroyMatches; // New field for dependency injection
    public CandySwapperModel(CandyViewer selectedCandy, GridManagerViewer gridManager, GameSettings gameSettings, CandyPool candyPool, CandySwapperViewer swapperViewer)
    {
        _selectedCandy = selectedCandy;
        _gridManager = gridManager;
        _gameSettings = gameSettings;
        _candyPool = candyPool;
        _swapperViewer = swapperViewer;
    }
    // New constructor for testing
    public CandySwapperModel(CandyViewer selectedCandy, GridManagerViewer gridManager, GameSettings gameSettings, CandyPool candyPool, CandySwapperViewer swapperViewer, DestroyMatches destroyMatches)
    {
        _selectedCandy = selectedCandy;
        _gridManager = gridManager;
        _gameSettings = gameSettings;
        _candyPool = candyPool;
        _swapperViewer = swapperViewer;
        _destroyMatches = destroyMatches; // Use the injected instance
    }
    public void SwapCandies(Vector2 direction)
    {
        if (_selectedCandy != null)
        {

            // Get the current grid coordinates (I, J) of the selected candy
            int currentI = _selectedCandy.CandyModel.PosInArrayI;
            int currentJ = _selectedCandy.CandyModel.PosInArrayJ;

            // Determine new coordinates based on direction
            int newI = currentI;
            int newJ = currentJ;
            Vector2 newCoordinates = _swapperViewer.DetermineCoordinatesFromDirection(direction, newI, newJ);
            newI = (int)newCoordinates.x;
            newJ = (int)newCoordinates.y;


            // Ensure the new position is within grid bounds
            if (newI >= 0 && newI < _gridManager.gameSettings.tilesNumberI && newJ >= 0 && newJ < _gridManager.gameSettings.tilesNumberJ)
            {
                // Get the second candy to be swapped with the selected candy
                CandyViewer secondCandy = _swapperViewer.GetCandyComponent(newI, newJ);

                // Swap the candies in the array
                _gridManager.CandiesArray[currentI, currentJ] = secondCandy.gameObject;
                _gridManager.CandiesArray[newI, newJ] = _selectedCandy.gameObject;

                // Update their properties

                _selectedCandy.SetArrayPosition(_selectedCandy.gameObject, _gridManager.CandiesArray, newI, newJ);
                secondCandy.SetArrayPosition(secondCandy.gameObject, _gridManager.CandiesArray, currentI, currentJ);
                matchesHor.Clear();
                matchesVer.Clear();

                var horizontalCheckVar = _swapperViewer.CheckRowAndColumns(_selectedCandy.gameObject, _gridManager.CandiesArray, true, matchesHor);
                var verticalCheckVar = _swapperViewer.CheckRowAndColumns(_selectedCandy.gameObject, _gridManager.CandiesArray, false, matchesVer);
                bool horizontalCheck = horizontalCheckVar.horVerCheck;
                matchesHor = horizontalCheckVar.CheckRowAndColumn;
                bool verticalCheck = verticalCheckVar.horVerCheck;
                matchesVer = verticalCheckVar.CheckRowAndColumn;
                
               

                if (horizontalCheck || verticalCheck)
                {
                    _swapperViewer.SwapPositions(newI, newJ, currentI, currentJ, secondCandy);

                    List<GameObject> rotationList = new List<GameObject>();
                    rotationList = _swapperViewer.CreateRotationList(matchesHor, matchesVer, _gameSettings, _candyPool);
                    if (!scoreManagerInitialized)
                    {
                        _swapperViewer.InitializeScoreManager(_gameSettings);
                        scoreManagerInitialized = true;
                    }

                    _swapperViewer.AddPoints(rotationList);
                    _swapperViewer.RotationCoroutineWrapper(rotationList);
                    DestroyFirstMatches();

                }
                else
                {
                    //No match, swapping back the array and candy positions.

                    _selectedCandy.SetArrayPosition(_selectedCandy.gameObject, _gridManager.CandiesArray, currentI, currentJ);
                    secondCandy.SetArrayPosition(secondCandy.gameObject, _gridManager.CandiesArray, newI, newJ);
                }
            }
            // Deselect the candy after moving
            _selectedCandy = null;
        }
    }
   
    public void DestroyFirstMatches()
    {
        // Create a HashSet to store unique candies
        HashSet<GameObject> uniqueMatches = new HashSet<GameObject>();

        // Add horizontal matches if they meet the criteria
        if (matchesHor.Count >= _gameSettings.candiesToMatch)
        {
            foreach (GameObject candy in matchesHor)
            {
                uniqueMatches.Add(candy);
            }
        }

        // Add vertical matches if they meet the criteria
        if (matchesVer.Count >= _gameSettings.candiesToMatch)
        {
            foreach (GameObject candy in matchesVer)
            {
                uniqueMatches.Add(candy);
            }
        }

        // Convert HashSet back to a list
        List<GameObject> combinedMatches = new List<GameObject>(uniqueMatches);

        // Call ReturnMatchesInList with the combined list
        DestroyMatches.Instance.ReturnMatchesInList(combinedMatches);

        // Clear both match lists
        matchesHor.Clear();
        matchesVer.Clear();
    }

}
