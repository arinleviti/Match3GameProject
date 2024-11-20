using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CalculatePointsTests
{
    private ScoreManagerModel _scoreManagerModel;
    private ScoreManagerViewer _scoreManagerViewer;
    private GameSettings _gameSettings;
    private GameObject _gameObject1;
    private GameObject _gameObject2;
    private GameObject _gameObject3;
    private GameObject _gameObject4;
    private GameObject _gameObject5;
    private GameObject _gameObject6;
    private GameObject _gameObject7;
    private GameObject _gameObject8;
    List<GameObject> listOfMatches = new List<GameObject>();
    [SetUp]
    public void SetUp()
    {
        GameObject someGameObject = new GameObject();
        _gameObject1 = CreateCandy(CandyType.Pumpkin);
        _gameObject2 = CreateCandy(CandyType.Pumpkin);
        _gameObject3 = CreateCandy(CandyType.Pumpkin);
        _gameObject4 = CreateCandy(CandyType.Pumpkin);
        _gameObject5 = CreateCandy(CandyType.Pumpkin);
        _gameObject6 = CreateCandy(CandyType.Pumpkin);
        _gameObject7 = CreateCandy(CandyType.Pumpkin);
        _gameObject8 = CreateCandy(CandyType.Pumpkin);
        _scoreManagerViewer = someGameObject.AddComponent<ScoreManagerViewer>();
        _gameSettings = ScriptableObject.CreateInstance<GameSettings>();
        _gameSettings.pointsFor3 = 10;
        _gameSettings.pointsFor4 = 20;
        _gameSettings.pointsFor5 = 50;
        _gameSettings.pointsFor6OrHigher = 70;
        _scoreManagerModel = new ScoreManagerModel(_gameSettings, _scoreManagerViewer);
        
        //_scoreManagerModel._gameSettings = _gameSettings;
    }

    [Test]
    public void CalculatePointsTests_three()
    {
        AddObjectsToList(_gameObject1, _gameObject2, _gameObject3);
        int points = _scoreManagerModel.CalculatePoints(listOfMatches);
        Assert.AreEqual(_gameSettings.pointsFor3, points);
    }

    [Test]
    public void CalculatePointsTests_four()
    {
        AddObjectsToList(_gameObject1, _gameObject2, _gameObject3, _gameObject4);
        int points =_scoreManagerModel.CalculatePoints(listOfMatches);
        Assert.AreEqual(_gameSettings.pointsFor4, points);
    }

    [Test]
    public void CalculatePointsTests_five()
    {
        AddObjectsToList(_gameObject1, _gameObject2, _gameObject3, _gameObject4, _gameObject5);
        int points = _scoreManagerModel.CalculatePoints(listOfMatches);
        Assert.AreEqual(_gameSettings.pointsFor5, points);
    }

    [Test]
    public void CalculatePointsTests_six()
    {
        AddObjectsToList(_gameObject1, _gameObject2, _gameObject3, _gameObject4, _gameObject5, _gameObject6);
        int points = _scoreManagerModel.CalculatePoints(listOfMatches);
        Assert.AreEqual(_gameSettings.pointsFor6OrHigher, points);
    }

    [Test]
    public void CalculatePointsTests_eight()
    {
        AddObjectsToList(_gameObject1, _gameObject2, _gameObject3, _gameObject4, _gameObject5, _gameObject6, _gameObject7, _gameObject8);
        int points = _scoreManagerModel.CalculatePoints(listOfMatches);
        Assert.AreEqual(_gameSettings.pointsFor6OrHigher, points);
    }

    [Test]
    public void CalculatePointsTests_twoThrowsException()
    {
        AddObjectsToList(_gameObject1, _gameObject2);

        var ex = Assert.Throws<InvalidOperationException>(() => _scoreManagerModel.CalculatePoints(listOfMatches));
        Assert.AreEqual("List must contain at least 3 elements to calculate points.", ex.Message);
    }
    [TearDown]
    public void TearDown()
    {
        if (_gameObject1 != null) GameObject.DestroyImmediate(_gameObject1);
        if (_gameObject2 != null) GameObject.DestroyImmediate(_gameObject2);
        if (_gameObject3 != null) GameObject.DestroyImmediate(_gameObject3);
        if (_gameObject4 != null) GameObject.DestroyImmediate(_gameObject4);
        if (_gameObject5 != null) GameObject.DestroyImmediate(_gameObject5);
        if (_gameObject6 != null) GameObject.DestroyImmediate(_gameObject6);   
        if (_gameObject7 != null) GameObject.DestroyImmediate(_gameObject7);
        if (_gameObject8 != null) GameObject.DestroyImmediate(_gameObject8);
    }
    private GameObject CreateCandy(CandyType type)
    {
        GameObject candy = new GameObject();
        CandyViewer candyScript = candy.AddComponent<CandyViewer>();
        candyScript.CandyType = type;
        return candy;
    }
    private void AddObjectsToList(params GameObject[] gameObjects)
    {
        listOfMatches.Clear();
        listOfMatches.AddRange(gameObjects);
    }
}
