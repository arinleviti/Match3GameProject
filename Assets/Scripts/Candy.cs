using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICandy
{
    public CandyType CandyType { get;  }
    float SizeX { get; }
    float SizeY { get; }
    float PosX { get; set; }
    float PosY { get; set; }
    int PosInArrayI { get; set; }
    int PosInArrayJ { get; set; }
    
    
}

// ICandy implementation
public class Candy : MonoBehaviour, ICandy
{
    [TextArea]
    public string WARNING = "'j' corresponds to the row index, equivalent to the X-coordinate. 'i' corresponds to the column index, equivalent to the Y-coordinate";

    [SerializeField] private CandyType candyType;
    [SerializeField] private int posInArrayI;
    [SerializeField] private int posInArrayJ;
    public CandyType CandyType { get { return candyType; } set { candyType = value; } }
    public float SizeX { get; set; }
    public float SizeY { get; set; }
    public float PosX { get ; set ; }
    public float PosY { get ; set ; }
    public int PosInArrayI { get { return posInArrayI; } set { posInArrayI = (int)value; } }
    public int PosInArrayJ { get { return posInArrayJ; } set { posInArrayJ = (int)value; } }

    private GameSettings gameSettings;


    public void ResetProperties()
    {
        
        PosX = -20;
        PosY = -20;
        PosInArrayI = -1;
        PosInArrayJ = -1;      
    }

    public void SetArrayPosition(GameObject candyGO, GameObject[,] candiesArray, int i, int j)
    {
        PosInArrayI = i;
        PosInArrayJ = j;
        candiesArray[i, j] = candyGO;
    }
    public void SetPhysicalPosition(GameObject candyGO, Vector3 position)
    {
        PosX = position.x;
        PosY = position.y;
        candyGO.transform.position = new Vector3(position.x, position.y, -1);

    }
}
public enum CandyType { Blue = 0, Green = 1, Yellow =2, Red =3};