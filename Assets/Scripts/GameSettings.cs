using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName= "GridSettings", menuName = "ScriptableObjects/GridSettings")]
public class GameSettings : ScriptableObject
{
    public int tileSize = 1;
    public float candyScaleFactor = 0.9f;
    public int tilesNumberI = 4;
    public int tilesNumberJ = 4;
    public int candyTypesCount;
    public List<CandyType> candyTypes = new List<CandyType>();
    public List<GameObject> candies = new List<GameObject>();
    public float deltaMovementThreshold;
    public int candiesToMatch;
    public float dropSpeed;
    //public float candySizeX = 0.9f;
    //public float candySizeY = 0.9f;
}
