using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class GridManagerModel
{
    private GameSettings _gameSettings;
    private GameObject[,] _gridCellsArray;
    private GridCell gridCellScript;
    private GridManagerViewer _gridManager;
    private CandyPool _candyPool;
    public GridManagerModel (GameSettings gameSettings, GameObject[,] gridCellsArray, GridManagerViewer gridManager, CandyPool candyPool)
    {
        _gameSettings = gameSettings;
        _gridCellsArray = gridCellsArray;
        _gridManager = gridManager;
        _candyPool = candyPool;
    }

    public void PopulateBackdropGrid(Vector2 firstTilePos, GameObject gridCellGO, GameObject[,] candiesArray)
    {
        for (int i = 0; i < _gameSettings.tilesNumberI; i++)
        {
            for (int j = 0; j < _gameSettings.tilesNumberJ; j++)
            {
                Vector2 position = new Vector2(firstTilePos.x + j * _gameSettings.tileSize, firstTilePos.y - i * _gameSettings.tileSize);
                gridCellGO = _gridManager.InstantiateGridCell();
                gridCellGO.transform.position = position;
                gridCellGO.transform.localScale = new Vector3(_gameSettings.tileSize, _gameSettings.tileSize, 1);
                _gridManager.SetGridCellParent(gridCellGO);
                _gridCellsArray[i, j] = gridCellGO;
                gridCellScript = gridCellGO.GetComponent<GridCell>();
                gridCellScript.PosX = gridCellGO.transform.position.x;
                gridCellScript.PosY = gridCellGO.transform.position.y;
                gridCellScript.PosInArrayJ = j;
                gridCellScript.PosInArrayI = i;
                //Debug.Log("Cell in position: X: " + gridCellScript.PosX + " Y: " + gridCellScript.PosY);
                //Debug.Log($"GridCell Created at ({i}, {j}): PosInArrayJ = {gridCellScript.PosInArrayJ}, PosInArrayI = {gridCellScript.PosInArrayI}");
                PopulateCandiesArray(position, gridCellGO, candiesArray, i, j);

            }
        }
    }
    private void PopulateCandiesArray(Vector3 position, GameObject gridCellGO, GameObject[,] candiesArray, int i, int j)
    {
        CandyType candyType = _gridManager.DetermineCandyType();
        GameObject randomCandy = _candyPool.GetCandy(candyType);
        CandyViewer randomCandyScript = randomCandy.GetComponent<CandyViewer>();
        position = new Vector3(position.x, position.y, -1);
        randomCandyScript.SetArrayPosition(randomCandy, candiesArray, i, j);
        randomCandyScript.SetPhysicalPosition( position);       
        _gridManager.SetCandyParent(randomCandy);
    }

    public Vector2 CalculateFirstTileXY(float tilesNumberX, float tilesNumberY, float tileSize)
    {
        float tileX = -(tilesNumberX / 2) * tileSize + (tileSize / 2);
        float tileY = (tilesNumberY / 2) * tileSize - (tileSize / 2);
        Vector2 firstTilePosition = new Vector2(tileX, tileY);
        return firstTilePosition;
    }
}
