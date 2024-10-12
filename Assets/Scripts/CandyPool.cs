using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CandyPool : MonoBehaviour
{
    private readonly object getCandyLock = new object();
    private readonly object returnCandyLock = new object();
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

        // Initialize the queues for each candy type
        //foreach (CandyType candy in gameSettings.candyTypes)
        //{

        //    Candy candyComponent = candyPrefab.GetComponent<Candy>();
        //    if (candyComponent != null && !arrayOfcandyQueues.ContainsKey(candyComponent.CandyType))
        //    {
        //        candyQueues[candyComponent.CandyType] = new Queue<GameObject>();
        //    }
        //}
    }

    public GameObject GetCandy(CandyType candyType)
    {
        lock (getCandyLock)
        {
            if (arrayOfcandyQueues[(int)candyType] != null && arrayOfcandyQueues[(int)candyType].Count > 0)
            {
                GameObject dequeuedCandy = arrayOfcandyQueues[(int)candyType].Dequeue();
                Debug.Log($"Candy {dequeuedCandy.name} dequeued from queue n{(int)candyType} / {candyType}");
                dequeuedCandy.SetActive(true);
                return dequeuedCandy;
            }
            else
            {
                GameObject prefab = gameSettings.candies.Find(c => c.GetComponent<Candy>().CandyType == candyType);
                if (prefab != null)
                {
                    var newCandy = Instantiate(prefab);

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
            Candy candyComponent = candyGO.GetComponent<Candy>();
            if (candyComponent == null)
            {
                Debug.LogError("This GameObject does not have a Candy component.");
                return;
            }

            CandyType candyType = candyComponent.CandyType;

            if ((int)candyType >= 0 && (int)candyType < gameSettings.candyTypesCount)
            {
                var queue = arrayOfcandyQueues[(int)candyType];

                candyComponent.ResetProperties();
                candyGO.SetActive(false);
                candyGO.transform.position = new Vector3(-gameSettings.tilesNumberJ * 1.5f, gameSettings.tilesNumberI * 1.5f, 0);
                candyGO.transform.parent = this.transform;

                queue.Enqueue(candyGO);
            }
            else
            {
                Debug.LogError("Candy type is not recognized or not managed by this pool.");
            }
            


        }
    }


}