using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ArrayGizmo : MonoBehaviour
{
    [SerializeField]
    private GridManagerViewer gridManager; // Reference to your GridManager
    [SerializeField]
    private GameSettings gameSettings;




    private void OnDrawGizmos()
    {
        if (gridManager.CandiesArray != null)
        {
            // Get the position of the ArrayGizmo GameObject
            Vector3 basePosition = transform.position;

            // Calculate the offsets based on the grid dimensions
            float offsetX = gameSettings.tilesNumberJ * 0.5f; // Adjust the multiplier as needed
            float offsetY = gameSettings.tilesNumberI * 0.5f; // Adjust the multiplier as needed

            // Loop through the array to draw Gizmos
            for (int i = 0; i < gridManager.CandiesArray.GetLength(0); i++)
            {
                for (int j = 0; j < gridManager.CandiesArray.GetLength(1); j++)
                {
                    if (gridManager.CandiesArray[i, j] != null)
                    {
                        CandyViewer candy = gridManager.CandiesArray[i, j].GetComponent<CandyViewer>();
                        if (candy != null)
                        {

                            Gizmos.color = GetColorFromCandyType(gridManager.CandiesArray[i, j].GetComponent<CandyViewer>().CandyType);

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

    private void DisplayCandyProperties(Vector3 position, CandyViewer candy)
    {
        // You can adjust the offset as needed
        Vector3 labelPosition = position + new Vector3(0, 0.5f, 0); // Adjust Y offset for visibility

#if UNITY_EDITOR
        // Display the candy properties using Handles
        Handles.Label(labelPosition,
            $"Type: {candy.CandyType}\n" +
            $"PosI: {candy.CandyModel.PosInArrayI}\n" +
            $"PosJ: {candy.CandyModel.PosInArrayJ}\n" +
            $"PosX: {candy.PosX}\n" +
            $"PosY: {candy.PosY}");
#endif
    }




    private Color GetColorFromCandyType(CandyType candyType)
    {
        switch (candyType)
        {
            case CandyType.Pumpkin:
                return new Color(1.0f, 0.6f, 0.0f); // Orange
            case CandyType.Bat:
                return new Color(0.2f, 0.2f, 0.2f); // Dark Gray
            case CandyType.BlackCandy:
                return Color.black; // Black
            case CandyType.Coffin:
                return new Color(0.4f, 0.2f, 0.1f); // Brown
            case CandyType.FrankenDead:
                return new Color(0.5f, 0.9f, 0.5f); // Pale Green
            case CandyType.Frankenstein:
                return new Color(0.3f, 0.7f, 0.3f); // Green
            case CandyType.Hat:
                return new Color(0.5f, 0.0f, 0.5f); // Purple
            case CandyType.Mummy:
                return new Color(0.9f, 0.9f, 0.9f); // Off White
            case CandyType.Potion:
                return new Color(0.6f, 0.2f, 0.8f); // Magenta
            case CandyType.Vampire:
                return new Color(0.8f, 0.0f, 0.0f); // Dark Red
            case CandyType.WhiteCandy:
                return Color.white; // Pure White
            case CandyType.Witch:
                return new Color(0.0f, 0.5f, 0.0f); // Dark Green
            default:
                return Color.gray; // Fallback color
        }
    }
}


