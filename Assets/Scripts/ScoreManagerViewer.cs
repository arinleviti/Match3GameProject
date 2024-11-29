using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScoreManagerViewer : MonoBehaviour, IScoreManagerViewer
{
    public int CurrentScore {  get; private set; }
    public event Action OnScoreChanged;
    public event Action<int> OnLevelUp;
    public GameSettings gameSettings;
    public int currentLevel = 1;
    public ScoreManagerModel scoreManagerModel;
    public static ScoreManagerViewer instance;
    public int updatedLevel;
    private AudioManager _audioManager;

    public static ScoreManagerViewer Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("ScoreManagerViewer");
                instance = go.AddComponent<ScoreManagerViewer>();
            }           
            return instance;
        }      
    }
    public void Initialize(GameSettings gameSettings, AudioManager audioManager)
    {
        instance.gameSettings = gameSettings;
        if (scoreManagerModel == null)
        {
            scoreManagerModel = new ScoreManagerModel(gameSettings, this);
        }
        _audioManager = audioManager;
    }
    
    public void AddPoints(List<GameObject> listOfMatches)
    {
        CurrentScore += scoreManagerModel.CalculatePoints(listOfMatches);
        OnScoreChanged?.Invoke();
        HasLevelChanged();
    }
    public void HasLevelChanged()
    {
        int oldLevel = currentLevel;
        var levelAndnumberOfCandies = scoreManagerModel.CandiesForCurrentLevel(CurrentScore, currentLevel);
        updatedLevel = levelAndnumberOfCandies.Item1;
        int numberOfCandies = levelAndnumberOfCandies.Item2;
        if (oldLevel < updatedLevel)
        {
            currentLevel = updatedLevel; // Update currentLevel to prevent repeated level-ups
            OnLevelUp?.Invoke(numberOfCandies);
            _audioManager.PlayAllEffects("Level Up");
        }
    }
    
    public void PlaySoundMatch3()
    {
        _audioManager.PlayAllEffectsWrapper("Match3 Sound");
    }
    public void PlaySoundMatch4()
    {
        _audioManager.PlayAllEffectsWrapper("Match4 Sound");
    }
    public void PlaySoundMatch5()
    {
        _audioManager.PlayAllEffectsWrapper("Match5 Sound");
    }
    public void PlaySoundMatch6()
    {
        _audioManager.PlayAllEffectsWrapper("Match6 Sound");
    }

}
public interface IScoreManagerViewer
{
    public void PlaySoundMatch3();
    public void PlaySoundMatch4();
    public void PlaySoundMatch5();
    public void PlaySoundMatch6();

}