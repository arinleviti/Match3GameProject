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
    public GameObject GetCandy(CandyType candyType)
    {
        return null;
    }
}

public class MockGridManagerViewer : IGridManagerViewer
{
    public GameObject[,] CandiesArray { get; set; }= new GameObject[10,10];
    

}

public class MockCandyFactory : ICandyFactory
{

    //public GameObject CreateCandy(CandyType candyType, GameSettings gameSettings)
    //{
    //    GameObject prefab = gameSettings.candies.Find(c => GetCandyComponent(c, candyType).CandyType == candyType);
    //    if (prefab != null)
    //    {
    //        GameObject newCandy = InstantiateAndActivateGO(prefab, gameSettings);
    //        return newCandy;
    //    }
    //    else
    //    {
    //        return null;
    //    }
    //}
    public GameObject CreateCandy(CandyType candyType, GameSettings gameSettings)
    {
        // Create a mock GameObject for testing purposes
        GameObject mockCandy = new GameObject($"MockCandy_{candyType}");
        CandyViewer mockCandyScript = mockCandy.AddComponent<CandyViewer>();
        mockCandyScript.CandyType = candyType;
        mockCandyScript.InitializeForTest(candyType);


        return mockCandy;
    }
    //public CandyViewer GetCandyComponent(GameObject c, CandyType candyType)
    //{
    //    CandyViewer candyScript = c.GetComponent<CandyViewer>();
    //    candyScript.CandyType = candyType;
    //    return candyScript;
    //}
    //public GameObject InstantiateAndActivateGO(GameObject prefab, GameSettings gameSettings)
    //{
    //    GameObject newCandy = prefab;
    //    return newCandy;
    //}
}
public class MockMatchHandlerViewer : IMatchHandlerViewer
{
    public void CoroutineWrapper(GameObject candy)
    {
        
    }
    public CandyViewer GetCandyComponent(GameObject candy)
    {
        CandyViewer candyViewerScript = candy.GetComponent<CandyViewer>();
        return candyViewerScript;
    }
    public GameObject[] LoadAllPrefabs(string folderPath)
    {
        return null;
    }
    public GameObject SelectRandomPrefab(List<GameObject> availablePrefabs)
    {
        return null;
    }
    public Vector3 TransferPositionAndScale(GameObject oldCandy, GameObject newCandyPrefab)
    {
        Vector3 vector = new Vector3();
        
        return vector;
    }
    public void SetCandyParent(GameObject newCandy, GameObject parent)
    { }
}
public class MockScoreManagerViewer : IScoreManagerViewer
{
    public void PlaySoundMatch3()
    {
        
    }
    public void PlaySoundMatch4()
    {
       
    }
    public void PlaySoundMatch5()
    {
       
    }
    public void PlaySoundMatch6()
    {
       
    }
}
