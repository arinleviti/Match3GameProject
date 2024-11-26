using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandySpawnerModel
{
    private GameSettings _gameSettings;
    private CandyPool _candyPool;
    private GridManagerViewer _gridManager;
    private CandySpawnerViewer _candySpawnerViewer;


    public CandySpawnerModel(GameSettings gameSettings, CandyPool candyPool, GridManagerViewer gridManager, CandySpawnerViewer candySpawnerViewer)
    {
        _gameSettings = gameSettings;
        _candyPool = candyPool;
        _gridManager = gridManager;
        _candySpawnerViewer = candySpawnerViewer;
    }
    public void CheckEmptiesReplaceSpawn(int candyTypeIndex)
    {
        //newCandiesList.Clear();
        for (int i = 0; i < _gameSettings.tilesNumberI; i++)
        {
            for (int j = 0; j < _gameSettings.tilesNumberJ; j++)
            {
                if (_gridManager.CandiesArray[i, j] == null)
                {
                    int randomIndex = Random.Range(0, candyTypeIndex );
                    GameObject newCandy = _candyPool.GetCandy((CandyType)randomIndex);
                    CandyViewer newCandyScript = newCandy.GetComponent<CandyViewer>();
                    GameObject gridCell = _gridManager.gridCellsArray[i, j];
                    Vector3 endPos = new Vector3(gridCell.transform.position.x, gridCell.transform.position.y, gridCell.transform.position.z/* - 2*/);
                    Vector3 startPos = new Vector3(endPos.x, (_gameSettings.tilesNumberI / 2) + 1,0 /*endPos.z - 2*/);
                    newCandyScript.SetArrayPosition(newCandy, _gridManager.CandiesArray, i, j);
                    newCandyScript.SetPhysicalPosition(endPos);
                    _candySpawnerViewer.CoroutineWrapper(newCandy, startPos, endPos);

                }
            }
        }
    }
}
