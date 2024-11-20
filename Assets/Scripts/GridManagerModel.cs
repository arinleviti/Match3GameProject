using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class GridManagerModel
{
    private GameSettings _gameSettings;
    private GridManagerViewer _gridManagerViewer;
    private GameObject[,] _gridCellsArray;

    public GridManagerModel(GameSettings gameSettings, GridManagerViewer gridManagerViewer, GameObject[,] gridCellsArray)
    {
        _gameSettings = gameSettings;
        _gridManagerViewer = gridManagerViewer;
        _gridCellsArray = gridCellsArray;
    }
    
    public void PopulateBackdropGrid(GameObject gridCellGO, Vector2 firstTilePos, GameObject[,] candiesArray)
    {
        for (int i = 0; i < _gameSettings.tilesNumberI; i++)
        {
            for (int j = 0; j < _gameSettings.tilesNumberJ; j++)
            {
                gridCellGO = _gridManagerViewer.SetGridCellPosition(firstTilePos, i, j);
                _gridManagerViewer.SetGridCellParent(gridCellGO);
                _gridCellsArray[i, j] = gridCellGO;
                _gridManagerViewer.InitializeGridCell(i, j);
                PopulateCandiesArray(gridCellGO, candiesArray, i, j);
            }
        }
    }
    private void PopulateCandiesArray(GameObject gridCellGO, GameObject[,] candiesArray, int i, int j)
    {
        CandyType candyType = _gridManagerViewer.DetermineCandyType();
        CandyViewer randomCandyScript = _gridManagerViewer.SetCandyInPlace(candyType);
        randomCandyScript.SetArrayPosition(randomCandyScript.gameObject, candiesArray, i, j);
    }
    public Vector2 CalculateFirstTileXY(float tilesNumberX, float tilesNumberY, float tileSize)
    {
        float tileX = -(tilesNumberX / 2) * tileSize + (tileSize / 2);
        float tileY = (tilesNumberY / 2) * tileSize - (tileSize / 2);
        Vector2 firstTilePosition = new Vector2(tileX, tileY);
        return firstTilePosition;
    }

}
