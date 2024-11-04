using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class GridManagerModel
{
   
    public Vector2 CalculateFirstTileXY(float tilesNumberX, float tilesNumberY, float tileSize)
    {
        float tileX = -(tilesNumberX / 2) * tileSize + (tileSize / 2);
        float tileY = (tilesNumberY / 2) * tileSize - (tileSize / 2);
        Vector2 firstTilePosition = new Vector2(tileX, tileY);
        return firstTilePosition;
    }
}
