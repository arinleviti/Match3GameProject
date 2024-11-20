using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class IsMatchTest
{
    private GameObject candy1;
    private GameObject candy2;
    private GameObject candy3;
    private GameObject candy4;
    private GameObject candy5;
    private GameObject candy6;
    private GameObject candyParent;
    private GameObject[,] candiesArray;
    private MatchHandlerModel matchHandlerModel;
    private GameSettings gameSettings;
    private IMatchHandlerViewer matchHandlerViewer;
    private ICandyPool candyPool;
    [SetUp]
    public void SetUp()
    {
        candy1 = CreateCandy("Candy1", CandyType.Blue, 0, 0, 4.5f, 4.5f);
        candy2 = CreateCandy("Candy2", CandyType.Blue, 0, 1, 4.5f, 4.5f);
        candy3 = CreateCandy("Candy3", CandyType.Blue, 0, 2, 6.5f, 6.5f);
        candy1 = CreateCandy("Candy4", CandyType.Blue, 0, 0, 4.5f, 4.5f);
        candy2 = CreateCandy("Candy5", CandyType.Blue, 0, 1, 4.5f, 4.5f);
        candy3 = CreateCandy("Candy6", CandyType.Yellow, 0, 2, 6.5f, 6.5f);

        // Initialize a 2D array with 1 row and 3 columns
        candiesArray = new GameObject[1, 3];

        gameSettings = ScriptableObject.CreateInstance<GameSettings>();
        matchHandlerViewer = new MockMatchHandlerViewer();
        candyPool = new MockCandyPool();
        candyParent = new GameObject();
        matchHandlerModel = new MatchHandlerModel(gameSettings, candiesArray,matchHandlerViewer,candyPool,candyParent);
        //matchHandlerModel._candiesArray = candiesArray;
    }
    [Test]
    public void IsMatch_ReturnTrueIfMatch()
    {
        candiesArray[0,0] = candy1;
        candiesArray[0,1] = candy2;
        candiesArray[0,2] = candy3;

        bool result = matchHandlerModel.IsMatch(0, 0, 0, 1, 0, 2);
        Assert.IsTrue(result);
    }
    [Test]
    public void IsMatch_ReturnFalseIfNoMatch()
    {
        candiesArray[0, 0] = candy4;
        candiesArray[0, 1] = candy5;
        candiesArray[0, 2] = candy6;

        bool result = matchHandlerModel.IsMatch(0, 0, 0, 1, 0, 2);
        Assert.IsFalse(result);
    }
    [TearDown]
    public void TearDown()
    {
        if (candy1 != null) GameObject.DestroyImmediate(candy1);
        if (candy2 != null) GameObject.DestroyImmediate(candy2);
        if (candy3 != null) GameObject.DestroyImmediate(candy3);
        if (candy4 != null) GameObject.DestroyImmediate(candy4);
        if (candy5 != null) GameObject.DestroyImmediate(candy5);
        if (candy6 != null) GameObject.DestroyImmediate(candy6);
        if (candyParent != null) GameObject.DestroyImmediate(candyParent);
    }
    private GameObject CreateCandy(string name, CandyType type, int posI, int posJ, float posX, float posY)
    {
        GameObject candy = new GameObject(name);
        CandyViewer candyScript = candy.AddComponent<CandyViewer>();
        candyScript.InitializeForTest(type);
        candyScript.CandyModel.PosInArrayI = posI;
        candyScript.CandyModel.PosInArrayJ = posJ;
        candyScript.PosX = posX;
        candyScript.PosY = posY;
        candy.SetActive(true);
        return candy;
    }
}
