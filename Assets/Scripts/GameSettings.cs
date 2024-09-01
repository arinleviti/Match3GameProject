using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName= "GridSettings", menuName = "ScriptableObjects/GridSettings")]
public class GameSettings : ScriptableObject
{
    public int tileSize = 1;
    public int tilesNumberHor = 4;
    public int tilesNumberVert = 4;
    public List<CandyType> candyTypes = new List<CandyType>();
    public List<GameObject> candies = new List<GameObject>();
    public float candySizeX = 0.9f;
    public float candySizeY = 0.9f;
}
