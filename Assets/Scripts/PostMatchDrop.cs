using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostMatchDrop : MonoBehaviour
{
    //public MatchHandler MatchHandler;
    private static PostMatchDrop instance;
    private CandyPool CandyPool;
    public MatchHandler MatchHandler { get; set; }
    public static PostMatchDrop Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("PostMatchDrop");
                instance = go.AddComponent<PostMatchDrop>();
                Debug.Log("PostMatchDrop instance created.");
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instance
        }
    }
    public void Initialize(MatchHandler matchHandler, CandyPool candyPool)
    {
        MatchHandler = matchHandler;
        CandyPool = candyPool;
    }
    public void PostMovementMatchCheck()
    {
        MatchHandler.Instance.CheckAndFixAllMatches(false);
        //foreach (GameObject match in MatchHandler.Matches)
        //{
        //    instance.CandyPool.ReturnCandy(match);
        //}
    }
}
