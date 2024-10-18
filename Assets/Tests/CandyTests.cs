using NUnit.Framework;
using UnityEngine;

public class CandyTests
{
    [Test]
    public void Candy_SetPhysicalPosition_PositionIsCorrect()
    {
        //Assign
        GameObject candyGameObject = new GameObject();
        GameObject testGameObject = new GameObject();
        Vector3 position = new Vector3(123, 123, 123);
        Candy candy = candyGameObject.AddComponent<Candy>();

        //Act
        candy.SetPhysicalPosition(testGameObject, position);

        //Assert
        Assert.AreEqual(new Vector3(123, 123, -1), testGameObject.transform.position);

        //CleanUp
        Object.DestroyImmediate(candyGameObject);
        Object.DestroyImmediate(testGameObject);
    }
}