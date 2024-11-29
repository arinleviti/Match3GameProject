using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    [SerializeField] private int posInArrayI;
    [SerializeField] private int posInArrayJ;
    public float PosX { get; set; }
    public float PosY {  get; set; }
    public int PosInArrayI { get { return posInArrayI; } set { posInArrayI = value; } }
    public int PosInArrayJ { get { return posInArrayJ; } set { posInArrayJ = value; } }
    private GameObject[,] candyGrid;
     
}
