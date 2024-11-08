using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchHandlerViewer : MonoBehaviour
{
    private static MatchHandlerViewer instance;
    public MatchHandlerModel MatchHandlerModel { get; private set; }
    public List<GameObject> Matches { get; private set; } = new List<GameObject>();
    private GameSettings _gameSettings;
    private GameObject[,] _candiesArray;
    private GameObject[,] _gridCellsArray;
    private GameObject _candyParent;
    private CandyPool _candyPool;
    public bool keepLooking; 

    public static MatchHandlerViewer Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("MatchHandlerViewer");
                instance = go.AddComponent<MatchHandlerViewer>();
            }
            return instance;
        }
    }

    public void Initialize(GameSettings gameSettings, GameObject[,] candiesArray, GameObject candyParent, GameObject candyPool)
    {
        _gameSettings = gameSettings;
        _candiesArray = candiesArray;
        _candyParent = candyParent;
        _candyPool = candyPool.GetComponent<CandyPool>();
        MatchHandlerModel = new MatchHandlerModel(_gameSettings, _candiesArray, this, _candyPool,_candyParent);
    }
    
    public void CoroutineWrapper(GameObject candy)
    {
        StartCoroutine(CandyAnimationsController.Instance.RotateMatchingCandies(candy, _gameSettings.rotationDuration, _gameSettings.numberOfRotations));
    }
    public GameObject[] LoadAllPrefabs(string folderPath)
    {
        GameObject[] prefabs = Resources.LoadAll<GameObject>(folderPath);
        return prefabs;
    }
    public CandyViewer GetCandyComponent(GameObject candy)
    {
        CandyViewer candyViewerScript = candy.GetComponent<CandyViewer>();
        return candyViewerScript; 
    }

    public GameObject SelectRandomPrefab(List<GameObject> availablePrefabs)
    {
        GameObject newCandyPrefab = availablePrefabs[UnityEngine.Random.Range(0, availablePrefabs.Count)];
        if (newCandyPrefab == null)
        {
            Debug.LogError("Selected new candy prefab is null.");           
        }
        return newCandyPrefab;
    }
    public Vector3 TransferPositionAndScale(GameObject oldCandy, GameObject newCandyPrefab)
    {
        Vector3 position = oldCandy.transform.position;
        newCandyPrefab.transform.localScale = oldCandy.transform.localScale;
        return position;
    }
    public void SetCandyParent(GameObject newCandy, GameObject parent)
    {
        newCandy.transform.SetParent(parent.transform);
    }
}
