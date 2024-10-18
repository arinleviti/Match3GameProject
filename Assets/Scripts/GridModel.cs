using UnityEngine;

public class GridModel
{
    public Vector2 CalculateFirstTileXY(float tilesNumberX, float tilesNumberY, float tileSize)
    {
        var tileX = -(tilesNumberX / 2) * tileSize + (tileSize / 2);
        var tileY = (tilesNumberY / 2) * tileSize - (tileSize / 2);
        var firstTilePosition = new Vector2(tileX, tileY);
        return firstTilePosition;
    }
}