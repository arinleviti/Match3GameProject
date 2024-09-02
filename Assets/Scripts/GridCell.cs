using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public float PosX { get; set; }
    public float PosY {  get; set; }
    public int PosInArrayI { get; set; }
    public int PosInArrayJ { get; set; }
    private GameObject[,] candyGrid;
    
    //public bool HasCandy(GameObject[,] candies)
    //{
    //    return candies[PosX, PosY] != null;
    //}

}
