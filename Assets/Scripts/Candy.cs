using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICandy : ICandyPoolNotifier
{
    public CandyType CandyType { get;  }
    float SizeX { get; }
    float SizeY { get; }
    float PosX { get; set; }
    float PosY { get; set; }
    int PosInArrayI { get; set; }
    int PosInArrayJ { get; set; }
    //GameObject Initialize(CandyType candyType);
    //Handles what happens when a player interacts with the candy. 
    
}

// ICandy implementation
public class Candy : MonoBehaviour, ICandy, ICandyPoolNotifier
{
    [TextArea]
    public string WARNING = "'j' corresponds to the row index, equivalent to the X-coordinate. 'i' corresponds to the column index, equivalent to the Y-coordinate";

    [SerializeField] private CandyType candyType;
    [SerializeField] private int posInArrayI;
    [SerializeField] private int posInArrayJ;
    public CandyType CandyType => candyType;
    public float SizeX { get; set; }
    public float SizeY { get; set; }
    public float PosX { get ; set ; }
    public float PosY { get ; set ; }
    public int PosInArrayI { get { return posInArrayI; } set { posInArrayI = value; } }
    public int PosInArrayJ { get { return posInArrayJ; } set { posInArrayJ = value; } }

    private GameSettings gameSettings;


    public void OnEnqueuedToPool()
    {
        // Handle what happens when the candy is returned to the pool
        
    }
    public void OnCreatedOrDequeuedFromPool(bool created)
    {
        // Handle what happens when the candy is created or dequeued from the pool
        if (created)
        {

        }
        else
        {

        }
    }
}
public enum CandyType { Blue, Green, Yellow, Red};