using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsController : MonoBehaviour
{

    private static AnimationsController instance;
    public static AnimationsController Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("AnimationsController");
                instance = go.AddComponent<AnimationsController>();
            }
            return instance;
        }
    }

    
    public IEnumerator MoveCandy(GameObject candy, Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = candy.transform.position;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            candy.transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime/duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        candy.transform.position = targetPosition;
    }
    public IEnumerator RotateMatchingCandies(List<GameObject> matchList, float duration, int rotations)
    {
        float timePerRotation = duration / rotations;
        foreach (GameObject candy in matchList)
        {
            for (int i = 0; i < rotations; i++)
            {
                float elapsedTime = 0f;
                float startRotation = candy.transform.rotation.eulerAngles.z;
                float endRotation = startRotation + 360;

                while (elapsedTime < timePerRotation)
                {
                    float zRotation = Mathf.Lerp(startRotation, endRotation, (elapsedTime/duration));
                    candy.transform.rotation = Quaternion.Euler(0,0,zRotation);
                    elapsedTime += Time.deltaTime;
                    //If the method were void and you didn't yield, all the calculations would be executed in a single frame.
                    yield return null;
                }
                candy.transform.rotation = Quaternion.Euler(0, 0, endRotation);
            }
        }
    }
    
}
