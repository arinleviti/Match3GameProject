using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockCandyPool : ICandyPool
{
    public List<GameObject> ReturnedCandies { get; set; } = new List<GameObject>();
    public void ReturnCandy(GameObject go)
    {
        ReturnedCandies.Add(go);
    }
}

public class MockGridManagerViewer : IGridManagerViewer
{
    public GameObject[,] CandiesArray { get; set; }= new GameObject[10,10];
    

}
