using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMatches : MonoBehaviour
{
    private static DestroyMatches instance;
    public ICandyPool candyPool;
    public IGridManagerViewer gridManager;
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
        gridManager = FindAnyObjectByType<GridManagerViewer>();
    }
   
    public void ReturnMatchesInList(List<GameObject> matches)
    {
        if (matches != null && matches.Count >= 3)
        {
            HashSet<GameObject> processedCandies = new HashSet<GameObject>();
            foreach (GameObject go in matches)
            {
                if (go != null && !processedCandies.Contains(go)) // Ensure the GameObject is not null
                {
                    processedCandies.Add(go);
                    //Debug.Log("Candy returned to pool: " + go.GetComponent<Candy>().CandyType + " Position I: " + go.GetComponent<Candy>().PosInArrayI + " Position J: " + go.GetComponent<Candy>().PosInArrayJ);
                    int i = go.GetComponent<CandyViewer>().CandyModel.PosInArrayI;
                    int j = go.GetComponent<CandyViewer>().CandyModel.PosInArrayJ;
                    float x = go.GetComponent<CandyViewer>().PosX;
                    float y = go.GetComponent<CandyViewer>().PosY;
                    Debug.Log($"Candy about to return in list PosInArrayI:{i} and PosInArrayJ: {j}, PosX {x} PosY {y}");
                    gridManager.CandiesArray[i, j] = null;
                    candyPool.ReturnCandy(go);
                    // Update the array to reflect that the candy has been returned to the pool
                    
                }
            }
        }
    }
}
