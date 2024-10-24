using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ArrayGizmo : MonoBehaviour
{
    [SerializeField]
    private GridManager gridManager; // Reference to your GridManager
    [SerializeField]
    private GameSettings gameSettings;




    private void OnDrawGizmos()
    {
        if (gridManager.candiesArray != null)
        {
            // Get the position of the ArrayGizmo GameObject
            Vector3 basePosition = transform.position;

            // Calculate the offsets based on the grid dimensions
            float offsetX = gameSettings.tilesNumberJ * 0.5f; // Adjust the multiplier as needed
            float offsetY = gameSettings.tilesNumberI * 0.5f; // Adjust the multiplier as needed

            // Loop through the array to draw Gizmos
            for (int i = 0; i < gridManager.candiesArray.GetLength(0); i++)
            {
                for (int j = 0; j < gridManager.candiesArray.GetLength(1); j++)
                {
                    if (gridManager.candiesArray[i, j] != null)
                    {
                        Candy candy = gridManager.candiesArray[i, j].GetComponent<Candy>();
                        if (candy != null)
                        {

                            Gizmos.color = GetColorFromCandyType(gridManager.candiesArray[i, j].GetComponent<Candy>().CandyType);

                            // Offset the position based on the grid and draw the cube
                            Vector3 cubePosition = basePosition + new Vector3(j + offsetX, i + offsetY, 0);
                            Gizmos.DrawCube(cubePosition, Vector3.one * 0.5f); // Adjust size
                            DisplayCandyProperties(cubePosition, candy);
                        }
                    }
                }
            }
        }
    }

    private void DisplayCandyProperties(Vector3 position, Candy candy)
    {
        // You can adjust the offset as needed
        Vector3 labelPosition = position + new Vector3(0, 0.5f, 0); // Adjust Y offset for visibility

#if UNITY_EDITOR
        // Display the candy properties using Handles
        Handles.Label(labelPosition,
            $"Type: {candy.CandyType}\n" +
            $"PosI: {candy.PosInArrayI}\n" +
            $"PosJ: {candy.PosInArrayJ}\n" +
            $"PosX: {candy.PosX}\n" +
            $"PosY: {candy.PosY}");
#endif
    }


    

    private Color GetColorFromCandyType(CandyType candyType)
    {
        switch (candyType)
        {
            case CandyType.Blue:
                return Color.blue;
            case CandyType.Green:
                return Color.green;
            case CandyType.Yellow:
                return Color.yellow;
            case CandyType.Red:
                return Color.red;
            default:
                return Color.white; // Fallback color
        }
    }
}


