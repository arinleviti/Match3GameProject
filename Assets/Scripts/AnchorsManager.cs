using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
// This class creates and manages the anchor points that are responsible for the scaling of the foreground image (Tombstone and Cemetery)
public class AnchorsManager : MonoBehaviour
{
    [SerializeField] private GameObject _topLeft;
    [SerializeField] private GameObject _topRight;
    [SerializeField] private GameObject _bottomLeft;
    [SerializeField] private GameObject _bottomRight;
    [SerializeField] private GameObject _innerFrame;
    private GameObject[,] _tilesGrid;
    private GameSettings _gridSettings;


    public void Initialize(GameObject[,] tilesGrid, GameSettings gameSettings)
    {
        _tilesGrid = tilesGrid;
        _gridSettings = gameSettings;
    }

    public void FindGridObjectsInCorners()
    {
        int nOfRows = _tilesGrid.GetLength(0);
        int nOfColumns = _tilesGrid.GetLength(1);

        _topLeft.transform.position = _tilesGrid[0, 0].transform.position;
        _topRight.transform.position = _tilesGrid[0, nOfColumns - 1].transform.position;
        _bottomLeft.transform.position = _tilesGrid[nOfRows - 1, 0].transform.position;
        _bottomRight.transform.position = _tilesGrid[nOfRows - 1, nOfColumns - 1].transform.position;
        
        GameObject[] arrayofGO = new GameObject[4];
        arrayofGO[0] = _topLeft;
        arrayofGO[1] = _topRight;
        arrayofGO[2] = _bottomLeft;
        arrayofGO[3] = _bottomRight;
        Vector2[] cornerPositions = CalculateCornersPosition(arrayofGO);

        for (int i = 0; i < arrayofGO.Length; i++)
        {
            arrayofGO[i].transform.position = new Vector3(cornerPositions[i].x, cornerPositions[i].y, -5);
        }
        AdjustImageToRectangle();
    }

    private Vector2[] CalculateCornersPosition(GameObject[] arrayOfGO)
    {
        Vector2[] cornerPositions = new Vector2[arrayOfGO.Length];
        float tileSize = _gridSettings.tileSize;
        for (int i = 0; i < arrayOfGO.Length; i++)
        {
            GameObject go = arrayOfGO[i];
            Vector2 centerPosition = go.transform.position;

            switch (i)
            {
                case 0: // Top-left
                    cornerPositions[i] = centerPosition + new Vector2(-tileSize / 2, tileSize / 2);
                    break;
                case 1: // Top-right
                    cornerPositions[i] = centerPosition + new Vector2(tileSize / 2, tileSize / 2);
                    break;
                case 2: // Bottom-left
                    cornerPositions[i] = centerPosition + new Vector2(-tileSize / 2, -tileSize / 2);
                    break;
                case 3: // Bottom-right
                    cornerPositions[i] = centerPosition + new Vector2(tileSize / 2, -tileSize / 2);
                    break;
            }
        }
        return cornerPositions;
    }
    private void AdjustImageToRectangle()
    {
        // Get the positions of the top-left and bottom-right grid corners
        Vector3 topLeftPosition = _topLeft.transform.position;
        Vector3 bottomRightPosition = _bottomRight.transform.position;

        // Calculate grid width and height based on the corner positions
        float gridWidth = Mathf.Abs(bottomRightPosition.x - topLeftPosition.x);
        float gridHeight = Mathf.Abs(topLeftPosition.y - bottomRightPosition.y);

        // Set the innerFrame position to the center
        Vector2 gridCenter = new Vector2(topLeftPosition.x + (gridWidth / 2), topLeftPosition.y - (gridHeight / 2));
        _innerFrame.transform.position = gridCenter;

        // Adjust the scale of the innerFrame to match the grid dimensions
        Vector3 newScale = _innerFrame.transform.localScale;
        newScale.x = gridWidth;  // Set the width
        newScale.y = gridHeight; // Set the height
        _innerFrame.transform.localScale = newScale;
    }


}
