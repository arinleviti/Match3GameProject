using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreMovementChecksModel
{
    private GameSettings _gameSettings;
    private PreMovementChecksViewer _preMovementChecksViewer;
    public PreMovementChecksModel(GameSettings gameSettings, PreMovementChecksViewer preMovementChecksViewer)
    {
        _gameSettings = gameSettings;
        _preMovementChecksViewer = preMovementChecksViewer;
    }
    public bool CheckRowAndColumn(GameObject candy, GameObject[,] candyArray, bool isHorizontal, out List<GameObject> matches)
    {
        CandyViewer currentCandy = _preMovementChecksViewer.GetCandyComponent(candy);
        int currentI = currentCandy.CandyModel.PosInArrayI;
        int currentJ = currentCandy.CandyModel.PosInArrayJ;

        matches = new List<GameObject>() { candy };

        int matchingCandiesCount = 1;
        int[] offsets = new[] { -1, 1 };

        foreach (int offset in offsets)
        {
            int step = offset;


            while (true)
            {
                int nextI = isHorizontal ? currentI : currentI + step;
                int nextJ = isHorizontal ? currentJ + step : currentJ;

                // Check bounds
                if (nextI < 0 || nextI >= _gameSettings.tilesNumberI || nextJ < 0 || nextJ >= _gameSettings.tilesNumberJ)
                    break;

                GameObject neighborCandyGO = candyArray[nextI, nextJ];
                CandyViewer neighborCandy = _preMovementChecksViewer.GetCandyComponent(neighborCandyGO);

                if (neighborCandy != null && neighborCandy.CandyType == currentCandy.CandyType)
                {
                    matchingCandiesCount++;
                    matches.Add(neighborCandyGO);
                    step += offset; // Continue to the next candy in the same direction
                }
                else
                {
                    break; // Stop if no match
                }
            }
        }

        return matchingCandiesCount >= _gameSettings.candiesToMatch; // Return true if there are at least 3 matching candies
    }

}
