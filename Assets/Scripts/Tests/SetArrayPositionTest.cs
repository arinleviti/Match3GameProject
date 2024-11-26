using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SetArrayPositionTest
{

    private CandyModel _candyModel;
    private GameObject _candyGO;
    private GameObject[,] _candiesArray;

    [SetUp]
    public void SetUp ()
    {
        _candyModel = new CandyModel (CandyType.Pumpkin);
        _candyGO = new GameObject();

        _candiesArray = new GameObject[3,3] ;
    }

    [Test]
    public void SetArrayPositionTest_TestPosition()
    {
        _candyModel.SetArrayPosition(_candyGO, _candiesArray, 1, 2);

        Assert.AreEqual(_candyGO, _candiesArray[1, 2]);
        Assert.AreEqual(1, _candyModel.PosInArrayI);
        Assert.AreEqual(2, _candyModel.PosInArrayJ);
    }

    [TearDown]
    public void TearDown()
    {
        if (_candyGO != null)
        {
            Object.DestroyImmediate(_candyGO);
        }
    }
}
