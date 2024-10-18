using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICandyViewer
{
    
    float SizeX { get; set; }
    float SizeY { get; set; }
    float PosX { get; set; }
    float PosY { get; set; }
        
}

// ICandy implementation
public class CandyViewer : MonoBehaviour, ICandyViewer
{
    [TextArea]
    public string WARNING = "'j' corresponds to the row index, equivalent to the X-coordinate. 'i' corresponds to the column index, equivalent to the Y-coordinate";

    [SerializeField] private CandyType candyType;
    [SerializeField] private float posX;
    [SerializeField] private float posY;

    public float PosX { get { return posX; } set { posX = value; } }
    public float PosY { get { return posY; } set { posY = value; } }
    public CandyType CandyType { get { return candyType; } set { candyType = value; } }
    public CandyModel CandyModel { get { return candyModel; } set { candyModel = value; } }
    public float SizeX { get; set; }
    public float SizeY { get; set; }
    private CandyModel candyModel;
    private void Awake()
    {
        CandyModel = new CandyModel(CandyType);
    }

   
    public void SetPhysicalPosition(Vector3 position)
    {
        PosX = position.x;
        PosY = position.y;
        transform.position = new Vector3(position.x, position.y, -1);
    }

    public void ResetProperties()
    {
        candyModel.ResetProperties();
    }

    public void SetArrayPosition(GameObject candyGO, GameObject[,] candiesArray, int i, int j)
    {
        candyModel.SetArrayPosition(candyGO, candiesArray, i, j);
    }
}
public enum CandyType { Blue = 0, Green = 1, Yellow =2, Red =3};