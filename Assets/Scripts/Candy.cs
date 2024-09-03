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
    //GameObject Initialize(CandyType candyType);
    //Handles what happens when a player interacts with the candy. 
    void HandleInteraction();
}

// ICandy implementation
public class Candy : MonoBehaviour, ICandy
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
    //public GameObject Initialize(CandyType candyType)
    //{
    //    //CandyType = candyType;
    //    return gameObject;
    //}
    public void HandleInteraction()
    {

    }
    
    void Start()
    {
        //gameSettings = Resources.Load<GameSettings>("ScriptableObjects/GridSettings");
        //if (gameSettings != null)
        //{
        //    SizeX = gameSettings.candySizeX;
        //    SizeY = gameSettings.candySizeY;
        //}
        //else
        //{
        //    Debug.LogError("GameSettings asset not found!");
        //}
        //Vector3 scale = new Vector3 (SizeX, SizeY);
        //transform.localScale = scale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
public enum CandyType { Blue, Green, Yellow, Red};