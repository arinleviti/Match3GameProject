using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManagerModel
{
    public GameSettings _gameSettings;
    private ScoreManagerViewer _scoreManagerViewer;
    public int numberOfCandies;

    public ScoreManagerModel(GameSettings gameSettings, ScoreManagerViewer scoreManagerViewer)
    {
        _gameSettings = gameSettings;
        _scoreManagerViewer = scoreManagerViewer;
    }
    
    public (int, int) CandiesForCurrentLevel(int currentScore, int currentLevel)
    {
        if (currentScore > 0)
        {
            if (currentScore >= _gameSettings.pointsToLevel6)
            {
                currentLevel = 6;
                return (currentLevel, _gameSettings.CandyTypesLevel5);
            }
            if (currentScore >= _gameSettings.pointsToLevel5 && currentScore < _gameSettings.pointsToLevel6)
            {
                currentLevel = 5;
                return (currentLevel, _gameSettings.CandyTypesLevel5);
            }
            if (currentScore >= _gameSettings.pointsToLevel4 && currentScore < _gameSettings.pointsToLevel5)
            {
                currentLevel = 4;
                return (currentLevel, _gameSettings.CandyTypesLevel4);
            }
            if (currentScore >= _gameSettings.pointsToLevel3 && currentScore < _gameSettings.pointsToLevel4)
            {
                currentLevel = 3;
                return (currentLevel, _gameSettings.CandyTypesLevel3);
            }
            if (currentScore >= _gameSettings.pointsToLevel2 && currentScore < _gameSettings.pointsToLevel3)
            {
                currentLevel = 2;
                return (currentLevel, _gameSettings.CandyTypesLevel2);
            }

        }
        return (currentLevel, _gameSettings.CandyTypesLevel1);
    }
    public int CalculatePoints(List<GameObject> listOfMatches)
    {
        if (listOfMatches.Count < 3)
            throw new InvalidOperationException("List must contain at least 3 elements to calculate points.");

        switch (listOfMatches.Count)

        {
            case 3: return _gameSettings.pointsFor3;
            case 4: return _gameSettings.pointsFor4;
            case 5: return _gameSettings.pointsFor5;
            case 6: return _gameSettings.pointsFor6OrHigher;
            default: return listOfMatches.Count >= 6 ? _gameSettings.pointsFor6OrHigher : 0;
        }
    }
}
