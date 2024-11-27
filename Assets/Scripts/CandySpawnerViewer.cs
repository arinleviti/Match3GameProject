using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CandySpawnerViewer : MonoBehaviour
{
    private static CandySpawnerViewer instance;
    private GridManagerViewer _gridManager;
    private GameSettings _gameSettings;
    private CandyPool _candyPool;
    //List<GameObject> newCandiesList = new List<GameObject>();
    public CandySpawnerModel SpawnerModel { get; private set; }
    public static CandySpawnerViewer Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("CandySpawnerViewer");
                instance = go.AddComponent<CandySpawnerViewer>();
            }
            return instance;
        }

    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void Initialize(GameSettings gameSettings, GridManagerViewer gridManager, CandyPool candyPool)
    {
        _gameSettings = gameSettings;
        _gridManager = gridManager;
        _candyPool = candyPool;
        SpawnerModel = new CandySpawnerModel(_gameSettings, _candyPool, _gridManager, this);
    }
    //This method checks for empties and places the GameObjects in the array but doesn't physically move them.
    //public void CheckEmptiesReplaceSpawn()
    //{
    //    //newCandiesList.Clear();
    //    for (int i = 0; i < _gameSettings.tilesNumberI; i++)
    //    {
    //        for (int j = 0; j < _gameSettings.tilesNumberJ; j++)
    //        {
    //            if (_gridManager.CandiesArray[i, j] == null)
    //            {
    //                int randomIndex = Random.Range(0, _gameSettings.candyTypes.Count);
    //                GameObject newCandy = _candyPool.GetCandy((CandyType)randomIndex);
    //                CandyViewer newCandyScript = newCandy.GetComponent<CandyViewer>();
    //                GameObject gridCell = _gridManager.gridCellsArray[i, j];
    //                Vector3 endPos = new Vector3(gridCell.transform.position.x, gridCell.transform.position.y, gridCell.transform.position.z - 2);
    //                Vector3 startPos = new Vector3(endPos.x, (_gameSettings.tilesNumberI / 2) + 1, endPos.z - 2);
    //                newCandyScript.SetArrayPosition(newCandy,_gridManager.CandiesArray, i,j);
    //                newCandyScript.SetPhysicalPosition(endPos);
    //                StartCoroutine(CandyAnimationsController.Instance.MoveCandy(newCandy, startPos, endPos, _gameSettings.dropSpeed));
                    
    //            }
    //        }
    //    }    
    //}
    public void CoroutineWrapper(GameObject newCandy, Vector3 startPos, Vector3 endPos)
    {
        StartCoroutine(CandyAnimationsController.Instance.MoveCandy(newCandy, startPos, endPos, _gameSettings.dropSpeed));
    }
}
