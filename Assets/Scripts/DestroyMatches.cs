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
                    int i = go.GetComponent<CandyViewer>().CandyModel.PosInArrayI;
                    int j = go.GetComponent<CandyViewer>().CandyModel.PosInArrayJ;
                    float x = go.GetComponent<CandyViewer>().PosX;
                    float y = go.GetComponent<CandyViewer>().PosY;
                    Debug.Log($"Candy about to return in list PosInArrayI:{i} and PosInArrayJ: {j}, PosX {x} PosY {y}");
                    gridManager.CandiesArray[i, j] = null;
                    candyPool.ReturnCandy(go);                                      
                }
            }
        }
    }
}
