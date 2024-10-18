using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class GridModelTests
    {
        [Test]
        public void GridModel_CalculateFirstTileXY_ResultIsCorrect()
        {
            //Assign
            var gridModel = new GridModel();
            const float tilesNumberX = 10;
            const float tilesNumberY = 12;
            const float tileSize = 13;

            //Act
            var result = gridModel.CalculateFirstTileXY(tilesNumberX, tilesNumberY, tileSize);

            //Assert
            Assert.AreEqual(new Vector2(-58.50f, 71.50f), result);
        }
    }
}