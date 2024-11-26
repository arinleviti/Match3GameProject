using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class CreateCandyTest
{
    private CandyFactory factory;
    private GameSettings settings;
    private CandyType type;
    private GameObject candy1;
    private GameObject candy2;
    [SetUp]
    public void Setup()
    {
        GameObject gameObject = new GameObject();
        factory = new CandyFactory();
        settings = ScriptableObject.CreateInstance<GameSettings>();
        type = CandyType.Pumpkin;
        candy1 = CreateCandy("Candy1", CandyType.Pumpkin, 0, 0, 4.5f, 4.5f);
        candy2 = CreateCandy("Candy2", CandyType.Vampire, 1, 1, 5.5f, 5.5f);
        settings.candies.Add(candy1);
        settings.candies.Add(candy2);
    }
    [Test]
    public void CreateCandyTest_CandyType_ShouldReturnSameType()
    {
        //Act
        GameObject newCandy = factory.CreateCandy(type, settings);
        CandyViewer newCandyScript = newCandy.GetComponent<CandyViewer>();


        // Assert
        Assert.AreEqual(type, newCandyScript.CandyType, "Returned candies count does not match expected unique count.");
    }
    private GameObject CreateCandy(string name, CandyType type, int posI, int posJ, float posX, float posY)
    {
        GameObject candy = new GameObject(name);
        CandyViewer candyScript = candy.AddComponent<CandyViewer>();
        candyScript.CandyType = type;
        return candy;
    }
}
