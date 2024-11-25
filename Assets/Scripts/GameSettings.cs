using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName= "GridSettings", menuName = "ScriptableObjects/GridSettings")]
public class GameSettings : ScriptableObject
{
    public float tileSize = 1;
    public float candyScaleFactor = 0.9f;
    public int tilesNumberI = 4;
    public int tilesNumberJ = 4;
    public int candyTypesCount;
    public List<CandyType> candyTypes = new List<CandyType>();
    public List<GameObject> candies = new List<GameObject>();

    [Header("Variables for animations:")]
    public float deltaMovementThreshold;
    public int candiesToMatch;
    public float dropSpeed;
    public float rotationDuration;
    public int numberOfRotations;

    [Header("Number of points for Match3,4,5,+.")] 
    public int pointsFor3;
    public int pointsFor4;
    public int pointsFor5;
    public int pointsFor6OrHigher;

    [Header("Number of candies spawn per level:")]
    public int CandyTypesLevel1;
    public int CandyTypesLevel2;
    public int CandyTypesLevel3;
    public int CandyTypesLevel4;
    public int CandyTypesLevel5;
    public int CandyTypesLevel6;

    [Header("Number of point to reach higher level:")]
    public int pointsToLevel2;
    public int pointsToLevel3;
    public int pointsToLevel4;
    public int pointsToLevel5;
    public int pointsToLevel6;
    
}
