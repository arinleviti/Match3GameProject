using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private float tileSize = 1;
    private int tilesNumberHor = 4;
    private int tilesNumberVert = 4;
    private GameObject gridCellGO;
    private int gridSize;
    private GameObject[] gameObjects;
    private GameObject gridParent;

    // Start is called before the first frame update
    void Start()
    {
        gridParent = new GameObject("GridParent");
        Vector2 firstTilePos = CalculateFirstTileXY(tilesNumberHor, tileSize);
        gridSize = tilesNumberHor*tilesNumberVert;
        gameObjects = new GameObject[gridSize];
        int perLine = Mathf.FloorToInt(tilesNumberHor/tileSize);

        for (int i = 0; i < tilesNumberVert; i++)
        {
            int currentCellNumber = tilesNumberHor * i;
            
            for (int j = 0; j < perLine; j++)
            {
                Vector2 position = new Vector2(firstTilePos.x +j*tileSize, firstTilePos.y - i*tileSize);
                gridCellGO = Instantiate(Resources.Load<GameObject>("Prefabs/GridCellPrefab"));
                gridCellGO.transform.position = position;
                gridCellGO.transform.SetParent(gridParent.transform);
                gameObjects[currentCellNumber+j] = gridCellGO;
            }            
        }
    }
    // Keeps the grind centered around the 0,0 cohordinates.
    private Vector2 CalculateFirstTileXY(float tilesNumber, float tileSize)
    {
        float tileX = -(tilesNumber / 2) + (tileSize/2);
        float tileY = (tilesNumber / 2) - (tileSize/2);
        Vector2 firstTilePosition = new Vector2(tileX, tileY);
        return firstTilePosition;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
