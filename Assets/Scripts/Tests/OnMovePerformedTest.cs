using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class OnMovePerformedTest
{
    private MovementModelController _movementModelController;
    private GridManagerViewer _gridManager;
    private GameSettings _gameSettings;
    private CandyPool _candyPool;
    private MatchHandlerViewer _matchHandler;
    private MovementViewer _movementViewer;
    private CandySwapperViewer _candySwapperViewer;
    private CandyViewer _candyViewer;

    [SetUp]
    public void Setup()
    {
        // Create a GameObject to hold MonoBehaviours
        GameObject gameObject = new GameObject();

        // Initialize all the dependencies
        _gridManager = gameObject.AddComponent<GridManagerViewer>();
        _gameSettings = ScriptableObject.CreateInstance<GameSettings>();
        //_candyPool = gameObject.AddComponent<CandyPool>();
        _matchHandler = gameObject.AddComponent<MatchHandlerViewer>();
        _movementViewer = gameObject.AddComponent<MovementViewer>();
        _candySwapperViewer = gameObject.AddComponent<CandySwapperViewer>();



        GameObject blueCandy = new GameObject();
        CandyViewer blueCandyScript = blueCandy.AddComponent<CandyViewer>();
        blueCandyScript.CandyType = CandyType.Blue;
        blueCandyScript.InitializeForTest(CandyType.Blue);
        GameObject greenCandy = new GameObject();
        CandyViewer greenCandyScript = greenCandy.AddComponent<CandyViewer>();
        greenCandyScript.CandyType = CandyType.Green;
        greenCandyScript.InitializeForTest(CandyType.Green);
        GameObject yellowCandy = new GameObject();
        CandyViewer yellowCandyScript = yellowCandy.AddComponent<CandyViewer>();
        yellowCandyScript.CandyType = CandyType.Yellow;
        yellowCandyScript.InitializeForTest(CandyType.Yellow);
        GameObject redCandy = new GameObject();
        CandyViewer redCandyScript = redCandy.AddComponent<CandyViewer>();
        redCandyScript.CandyType = CandyType.Red;
        redCandyScript.InitializeForTest(CandyType.Red);

        _gameSettings.tilesNumberI = 8;
        _gameSettings.tilesNumberJ = 8;
        _gameSettings.tileSize = 1;
        _gameSettings.candyScaleFactor = 1;
        _gameSettings.candyTypesCount = 4;
        _gameSettings.candyTypes = new List<CandyType> { CandyType.Blue, CandyType.Red, CandyType.Green, CandyType.Yellow };
        _gameSettings.candies = new List<GameObject> { blueCandy, greenCandy, yellowCandy, redCandy };
        _gameSettings.deltaMovementThreshold = 2;
        _gameSettings.candiesToMatch = 3;
        _gameSettings.dropSpeed = 0.09f;
        _gameSettings.rotationDuration = 0.3f;
        _gameSettings.numberOfRotations = 4;

        _gridManager.gameSettings = _gameSettings;
        //_candyPool.InitializeForTesting();
        _gridManager.StartForTest();

        _candyPool = _gridManager.candyPoolScript;
        _candyPool.gridManager = _gridManager;
       
        

        _candyViewer = gameObject.AddComponent<CandyViewer>();
       
        // Create an instance of MovementModelController
        _movementModelController = new MovementModelController(
            _gridManager,
            _gameSettings,
            _candyPool,
            _matchHandler,
            _movementViewer
        );

        GameObject mockBlueCandy = new GameObject();
        CandyViewer mockBlueCandyScript = mockBlueCandy.AddComponent<CandyViewer>();
        mockBlueCandyScript.CandyType = CandyType.Blue;
        mockBlueCandyScript.InitializeForTest(CandyType.Blue);

        _movementModelController.SelectedCandy = mockBlueCandyScript;
        _gridManager.gameSettings = _gameSettings;
        
       
        _candySwapperViewer.Initialize(_candyViewer, _gridManager, _gameSettings, _candyPool);
       

    }
        
        [Test]
    public void OnMovePerformed_WithValidInput_ShouldInvokeSwapCandies()
    {
        // Arrange
        Vector2 validInput = new Vector2(2, 10); // Assuming this is greater than deltaMovementThreshold
        _gameSettings.deltaMovementThreshold = 0.9f; // Set the threshold lower than the input magnitude

        // Act
        _movementModelController.OnMovePerformed(validInput);

        // Assert
        Assert.IsTrue(_movementModelController.IsMoving);
        
    }
    [TearDown]
    public void TearDown()
    {
        // Clean up the GameObject after tests
        GameObject.DestroyImmediate(_gridManager.gameObject);
    }
}