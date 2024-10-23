using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CandyPool : MonoBehaviour
{
    private readonly object getCandyLock = new object();
    private readonly object returnCandyLock = new object();
    private GridManagerViewer gridManager;
    //public Dictionary<CandyType, Queue<GameObject>> candyQueues = new Dictionary<CandyType, Queue<GameObject>>();
    private Queue<GameObject>[] arrayOfcandyQueues;
    [SerializeField] private GameSettings gameSettings;

    private void Awake()
    {
        arrayOfcandyQueues = new Queue<GameObject>[gameSettings.candyTypesCount];
        for (int i = 0; i < gameSettings.candyTypesCount; i++)
        {
            arrayOfcandyQueues[i] = new Queue<GameObject>();
        }

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
                //Debug.Log($"Candy {dequeuedCandy.name} dequeued from queue n{(int)candyType} / {candyType}");
                dequeuedCandy.SetActive(true);
                

                return dequeuedCandy;
            }
            else
            {
                GameObject prefab = gameSettings.candies.Find(c => c.GetComponent<CandyViewer>().CandyType == candyType);
                if (prefab != null)
                {
                    var newCandy = Instantiate(prefab);
                    newCandy.SetActive(true);
                    return newCandy;
                }
                else
                {
                    Debug.LogError($"No prefab found for CandyType: {candyType}");
                    return null;
                }
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

            if ((int)candyType >= 0 && (int)candyType < gameSettings.candyTypesCount)
            {
                var queue = arrayOfcandyQueues[(int)candyType];

                candyScript.ResetProperties();
                candyScript.SetPhysicalPosition(new Vector3(-gameSettings.tilesNumberJ * 1.5f, gameSettings.tilesNumberI * 1.5f, 0));
                //candyGO.transform.position = ;
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