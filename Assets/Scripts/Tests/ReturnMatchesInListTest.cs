using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class ReturnMatchesInListTest
{

    private IGridManagerViewer _gridManagerViewer;
    private ICandyPool _candyPool;
    private DestroyMatches _destroyMatches;
    List<GameObject> matches;

    public GameObject candy1;
    public GameObject candy2;
    public GameObject candy3;
    public GameObject candy4;
    public GameObject candy5;
    public GameObject candy6;
    
    [SetUp]
    public void SetUp()
    {
        GameObject gameObject = new GameObject();

        _gridManagerViewer = new MockGridManagerViewer();
        _candyPool = new MockCandyPool();
        _destroyMatches = gameObject.AddComponent<DestroyMatches>();
        _destroyMatches.gridManager = _gridManagerViewer;
        _destroyMatches.candyPool = _candyPool;

        candy1 = CreateCandy("Candy1", CandyType.Pumpkin, 0, 0, 4.5f, 4.5f);
        candy2 = CreateCandy("Candy2", CandyType.Bat, 1, 1, 5.5f, 5.5f);
        candy3 = CreateCandy("Candy3", CandyType.FrankenDead, 2, 2, 6.5f, 6.5f);
        candy4 = CreateCandy("Candy4", CandyType.Vampire, 3, 3, 7.5f, 7.5f);
        candy5 = CreateCandy("Candy1", CandyType.Pumpkin, 0, 0, 4.5f, 4.5f);
        candy6 = CreateCandy("Candy3", CandyType.FrankenDead, 2, 2, 6.5f, 6.5f);


        matches = new List<GameObject> { candy1, candy2, candy3, candy4, candy5, candy6 };

    }
    [Test]
    public void ReturnMatches_AddsUniqueCandiesToPool()
    {
        //Act
        _destroyMatches.ReturnMatchesInList(matches);

        // Act
        HashSet<GameObject> expectedReturnedCandies = new HashSet<GameObject>(matches);

        // Assert
        Assert.AreEqual(expectedReturnedCandies.Count, _candyPool.ReturnedCandies.Count, "Returned candies count does not match expected unique count.");

        // Verify that all returned candies are in the expected set
        foreach (var candy in expectedReturnedCandies)
        {
            Assert.IsTrue(_candyPool.ReturnedCandies.Contains(candy), $"Returned candies do not include expected candy: {candy.name}");
        }
    }

    [TearDown]
    public void Cleanup()
    {
        // Optionally destroy test GameObjects
        foreach (var candy in new[] { candy1, candy2, candy3, candy4, candy5, candy6 })
        {
            if (candy != null)
            {
                Object.Destroy(candy);
            }
        }

        // Also destroy the DestroyMatches component if necessary
        Object.Destroy(_destroyMatches.gameObject);
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
