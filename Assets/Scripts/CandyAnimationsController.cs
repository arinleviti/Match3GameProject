using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyAnimationsController : MonoBehaviour
{
    private CandyPool pool;
    private List<GameObject> combinedLists;
    private List<GameObject> clonedLists;
    private static CandyAnimationsController instance;
    public static CandyAnimationsController Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("AnimationsController");
                instance = go.AddComponent<CandyAnimationsController>();
                instance.pool = FindAnyObjectByType<CandyPool>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        combinedLists = new List<GameObject>();
        clonedLists = new List<GameObject>();
    }

    public IEnumerator MoveCandy(GameObject candy, Vector3 startPosition, Vector3 targetPosition, float duration)
    {
        //Vector3 startPosition = candy.transform.position;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            candy.transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime/duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        candy.transform.position = targetPosition;
    }
    public IEnumerator RotateMatchingCandies(GameObject candy, float duration, int rotations)
    {
        float timePerRotation = duration / rotations;

            for (int i = 0; i < rotations; i++)
            {
                float elapsedTime = 0f;
                float startRotation = candy.transform.rotation.eulerAngles.z;
                float endRotation = startRotation + 360;


                while (elapsedTime < timePerRotation)
                {
                    float zRotation = Mathf.Lerp(startRotation, endRotation, (elapsedTime / duration));
                    candy.transform.rotation = Quaternion.Euler(0, 0, zRotation);
                    elapsedTime += Time.deltaTime;
                    //If the method were void and you didn't yield, all the calculations would be executed in a single frame.
                    

                    yield return null;
                }
                candy.transform.rotation = Quaternion.Euler(0, 0, endRotation);

            }
        pool.ReturnCandy(candy);
    }
    public List<GameObject> CreateRotationList( List<GameObject> matchesHor, List<GameObject> matchesVer, GameSettings gameSettings, CandyPool candyPool)
    {
        clonedLists.Clear();
        HashSet<GameObject> uniqueCandies = new HashSet<GameObject>();

        if (matchesHor.Count >= gameSettings.candiesToMatch)
        {
            uniqueCandies.UnionWith(matchesHor);
        }
        if (matchesVer.Count >= gameSettings.candiesToMatch)
        {
            uniqueCandies.UnionWith(matchesVer);
        }
        foreach (GameObject candy in uniqueCandies)
        {
            Candy parentCandyScript = candy.GetComponent<Candy>();
            CandyType parentCandyType = parentCandyScript.CandyType;
            GameObject clonedCandy = candyPool.GetCandy(parentCandyType);
            //Candy cloneCandyScript = cloneCandy.GetComponent<Candy>();
            clonedCandy.transform.position = new Vector3(candy.transform.position.x, candy.transform.position.y, candy.transform.position.z - 2); ;
            clonedLists.Add(clonedCandy);
            //Debug.Log($"Cloned candy {clonedCandy.name} created at {System.DateTime.Now}");
        }

        return clonedLists;

    }

}
