using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface ICandyModel
{
    int PosInArrayI { get; set; }
    int PosInArrayJ { get; set; }
}
public class CandyModel : ICandyModel
{
    private CandyType candyType;
    private int posInArrayI;
    private int posInArrayJ;
    public CandyType CandyType { get =>  candyType; set => candyType = value; }
    public int PosInArrayI { get => posInArrayI; set => posInArrayI = value; }
    public int PosInArrayJ { get => posInArrayJ; set => posInArrayJ = value; }

    public CandyModel(CandyType candyType)
    {
        CandyType = candyType;      
    }

    public void ResetProperties()
    {
        PosInArrayI = -1;
        PosInArrayJ = -1;
    }

    public void SetArrayPosition(GameObject candyGO, GameObject[,] candiesArray, int i, int j)
    {
        PosInArrayI = i;
        PosInArrayJ = j;
        candiesArray[i, j] = candyGO;
    }
}
