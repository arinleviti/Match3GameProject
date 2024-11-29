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
    
    public void CoroutineWrapper(GameObject newCandy, Vector3 startPos, Vector3 endPos)
    {
        StartCoroutine(CandyAnimationsController.Instance.MoveCandy(newCandy, startPos, endPos, _gameSettings.dropSpeed));
    }
}
