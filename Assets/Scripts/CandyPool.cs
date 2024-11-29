using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CandyPool : MonoBehaviour, ICandyPool
{
    private readonly object getCandyLock = new object();
    private readonly object returnCandyLock = new object();
    public GridManagerViewer gridManager;
    private Queue<GameObject>[] arrayOfcandyQueues;
    [SerializeField] public GameSettings _gameSettings;
    private ICandyFactory _candyFactory;
    
    //Only needed for the mock class
    public List<GameObject> ReturnedCandies { get; set; }

    private void Awake()
    {
        arrayOfcandyQueues = new Queue<GameObject>[_gameSettings.candyTypesCount];
        for (int i = 0; i < _gameSettings.candyTypesCount; i++)
        {
            arrayOfcandyQueues[i] = new Queue<GameObject>();
        }
        _candyFactory = new CandyFactory();
    }
    public void InitializeForTesting( GameSettings gameSettings, ICandyFactory candyFactory)
    {
        _gameSettings = gameSettings;
        arrayOfcandyQueues = new Queue<GameObject>[_gameSettings.candyTypesCount];
        for (int i = 0; i < _gameSettings.candyTypesCount; i++)
        {
            arrayOfcandyQueues[i] = new Queue<GameObject>();
        }
        _candyFactory = candyFactory;
    }
    private void Start()
    {
        gridManager = FindAnyObjectByType<GridManagerViewer>();
    }
    public GameObject GetCandy(CandyType candyType)
    {
        lock (getCandyLock)
        {
            if (arrayOfcandyQueues[(int)candyType] != null && arrayOfcandyQueues[(int)candyType].Count > 0)
            {
                GameObject dequeuedCandy = arrayOfcandyQueues[(int)candyType].Dequeue();                
                dequeuedCandy.SetActive(true);
                

                return dequeuedCandy;
            }
            else
            {
                return _candyFactory.CreateCandy(candyType, _gameSettings);                
            }
            
        }
    }

    public void ReturnCandy(GameObject candyGO)
    {
        lock (returnCandyLock)
        {
            CandyViewer candyScript = candyGO.GetComponent<CandyViewer>();
            if (candyScript == null)
            {
                Debug.LogError("This GameObject does not have a Candy component.");
                return;
            }

            CandyType candyType = candyScript.CandyType;

            if ((int)candyType >= 0 && (int)candyType < _gameSettings.candyTypesCount)
            {
                var queue = arrayOfcandyQueues[(int)candyType];

                candyScript.ResetProperties();
                candyScript.SetPhysicalPosition(new Vector3(-_gameSettings.tilesNumberJ * 1.5f, _gameSettings.tilesNumberI * 1.5f, 0));

                GameObject candyParent = GameObject.Find("CandyParent");

                if (candyParent != null)
                {
                    // Set candyGO's parent to CandyParent
                    candyGO.transform.parent = candyParent.transform;
                }
                candyGO.SetActive(false);
                queue.Enqueue(candyGO);
            }
            else
            {
                Debug.LogError("Candy type is not recognized or not managed by this pool.");
            }
        }
    }

}

public interface ICandyFactory
{
    GameObject CreateCandy(CandyType candyType, GameSettings gameSettings);    
}

public class CandyFactory : ICandyFactory
{
    public GameObject CreateCandy(CandyType candyType, GameSettings gameSettings)
    {
        GameObject prefab = gameSettings.candies.Find(c => GetCandyComponent(c, candyType).CandyType == candyType);
        if (prefab != null)
        {
            GameObject newCandy = InstantiateAndActivateGO(prefab, gameSettings);
            return newCandy;
        }
        else
        {
            return null;
        }
    }
    public CandyViewer GetCandyComponent(GameObject c, CandyType candyType) //candyType is for testing purposes only.
    {
        CandyViewer candyViewer = c.GetComponent<CandyViewer>();
        return candyViewer;
    }
    public GameObject InstantiateAndActivateGO(GameObject prefab, GameSettings gameSettings)
    {
        var newCandy = Object.Instantiate(prefab);
        newCandy.transform.localScale = new Vector3(gameSettings.candyScaleFactor, gameSettings.candyScaleFactor, gameSettings.candyScaleFactor);
        newCandy.SetActive(true);
        return newCandy;
    }
}


public interface ICandyPool
{
    public List<GameObject> ReturnedCandies { get; set; }
    public void ReturnCandy(GameObject candyGO);
    public GameObject GetCandy(CandyType candyType);
}