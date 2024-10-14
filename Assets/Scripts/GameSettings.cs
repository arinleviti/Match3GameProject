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
    public float rotationDuration;
    public int numberOfRotations;

    //Number of points ofr Match3,4,5,+.
    public int pointsFor3;
    public int pointsFor4;
    public int pointsFor5;
    public int pointsFor6OrHigher;

    //Number of candies spawn per level:
    public int candiesForLevel1; 
    public int candiesForLevel2;
    public int candiesForLevel3;
    public int candiesForLevel4;

    //Number of point to reach higher level:
    public int pointsToLevel2;
    public int pointsToLevel3;
    public int pointsToLevel4;
    public int pointsToLevel5;
    public int pointsToLevel6;
    
}
