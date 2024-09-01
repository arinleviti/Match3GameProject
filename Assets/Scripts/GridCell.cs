using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public int x;
    public int y;
    private GameObject[,] candyGrid;
    
    public bool HasCandy(GameObject[,] candies)
    {
        return candies[x, y] != null;
    }

}
