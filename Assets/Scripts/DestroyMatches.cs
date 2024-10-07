using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMatches : MonoBehaviour
{
    private static DestroyMatches instance;
    private CandyPool candyPool;
    private GridManager gridManager;
    public static DestroyMatches Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("DestroyMatches");
                instance = go.AddComponent<DestroyMatches>();
                instance.Initialize();
            }
            return instance;
        }
    }
    private void Awake()
    {
        // Check if an instance already exists
        if (instance == null)
        {
            instance = this;
            Initialize();
        }
        else if (instance != this)
        {
            // If a different instance already exists, destroy this one
            Destroy(gameObject);
        }
    }
    private void Initialize()
    {
        candyPool = FindAnyObjectByType<CandyPool>();
        //? checks if controller is null. If it is, returns the empty list. If controller.matchesHor is null, returns an empty list. If the value is not null, returns its value
        //matches = controller?.matches?? new List<GameObject>();
        gridManager = FindAnyObjectByType<GridManager>();
    }
   
    public void ReturnMatchesInList(List<GameObject> matches)
    {
        if (matches != null && matches.Count >= 3)
        {
            foreach (GameObject go in matches)
            {
                if (go != null) // Ensure the GameObject is not null
                {
                    Debug.Log("Candy returned to pool: " + go.GetComponent<Candy>().CandyType + " Position I: " + go.GetComponent<Candy>().PosInArrayI + " Position J: " + go.GetComponent<Candy>().PosInArrayJ);
                    int i = go.GetComponent<Candy>().PosInArrayI;
                    int j = go.GetComponent<Candy>().PosInArrayJ;
                    gridManager.candiesArray[i, j] = null;
                    candyPool.ReturnCandy(go);
                    // Update the array to reflect that the candy has been returned to the pool
                    
                }
            }
        }
    }
}
