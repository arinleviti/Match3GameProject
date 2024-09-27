using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICandyPoolNotifier
{
    //Called when candy is returned to the pool.
    void OnEnqueuedToPool();
    //Called when candy is leaving the pool or has just been created.
    //If true, the object has just been created so it's not recycled.
    void OnCreatedOrDequeuedFromPool(bool created);
}

public class CandyPool : MonoBehaviour
{
   
    public Dictionary<CandyType, Queue<GameObject>> candyQueues = new Dictionary<CandyType, Queue<GameObject>>();
    [SerializeField] private GameSettings gameSettings;

    private void Awake()
    {
        //gameSettings = Resources.Load<GameSettings>("ScriptableObjects/GameSettings");
        // Initialize the queues for each candy type
        foreach (GameObject candyPrefab in gameSettings.candies)
        {
            Candy candyComponent = candyPrefab.GetComponent<Candy>();
            if (candyComponent != null && !candyQueues.ContainsKey(candyComponent.CandyType))
            {
                candyQueues[candyComponent.CandyType] = new Queue<GameObject>();
            }
        }
    }

    public GameObject GetCandy(CandyType candyType)
    {
        //If the key is found, TryGetValue returns true, and the corresponding Queue<GameObject> is assigned to queue.
        if (candyQueues.TryGetValue(candyType, out var queue))
        {
            if (queue.Count > 0)
            {
                //Dequeue(): removes the item at the front of the queue
                GameObject dequeuedCandy = queue.Dequeue();
                dequeuedCandy.transform.parent = null;
                dequeuedCandy.SetActive(true);

                // Notify any components that the candy was created or dequeued
                var notifiers = dequeuedCandy.GetComponents<ICandyPoolNotifier>();
                foreach (var notifier in notifiers)
                {
                    notifier.OnCreatedOrDequeuedFromPool(false);
                }

                return dequeuedCandy;
            }
            else
            {
                GameObject prefab = gameSettings.candies.Find(c => c.GetComponent<Candy>().CandyType == candyType);
                if (prefab != null)
                {
                    var newCandy = Instantiate(prefab);
                    
                    var notifiers = newCandy.GetComponents<ICandyPoolNotifier>();
                    foreach (var notifier in notifiers)
                    {
                        notifier.OnCreatedOrDequeuedFromPool(true);
                    }

                    return newCandy;
                }
                else
                {
                    Debug.LogError($"No prefab found for CandyType: {candyType}");
                    return null;
                }
            }
        }
        else
        {
            Debug.LogError($"Candy type {candyType} is not defined in the pool.");
            return null;
        }
    }

    public void ReturnCandy(GameObject candyGO)
    {
        Candy candyComponent = candyGO.GetComponent<Candy>();
        if (candyComponent == null)
        {
            Debug.LogError("This GameObject does not have a Candy component.");
            return;
        }

        CandyType candyType = candyComponent.CandyType;

        if (candyQueues.ContainsKey(candyType))
        {
            var queue = candyQueues[candyType];

            // Notify any components that the candy was returned to the pool
            var notifiers = candyGO.GetComponents<ICandyPoolNotifier>();
            foreach (var notifier in notifiers)
            {
                notifier.OnEnqueuedToPool();
            }
            candyComponent.ResetProperties();
            candyGO.SetActive(false);
            candyGO.transform.parent = this.transform;

            queue.Enqueue(candyGO);
        }
        else
        {
            Debug.LogError("Candy type is not recognized or not managed by this pool.");
        }
    }

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
