using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour 
{
    public SoundEffect[] effects;
    private Dictionary<string, SoundEffect> _effectDictionary;
    private AudioListener _listener;
    private Coroutine _backgroundMusicCoroutine;
    [SerializeField] private GameSettings gameSettings;


    private void Awake()
    {
        _effectDictionary = new Dictionary<string, SoundEffect>();
        foreach (var effect in effects)
        {
            _effectDictionary[effect.name] = effect;
        }
        _listener = FindObjectOfType<AudioListener>();
    }

    public void PlayEffect(string effectName)
    {
        if (_listener == null)
        {
            _listener = FindObjectOfType<AudioListener>();
        }
        PlayEffect(effectName, _listener.transform.position);
    }

    public IEnumerator PlayEffect(string effectName, Vector3 position)
    {
        if(_effectDictionary.ContainsKey(effectName) == false)
        {
            Debug.LogWarningFormat("Effect {0} is not registered.", effectName);
            yield return null;
        }
        var clip = _effectDictionary[effectName].GetRandomClip();

        if (clip == null)
        {
            Debug.LogWarningFormat("Effect {0} has no clips to play.", effectName);
            yield return null;
        }
        AudioSource.PlayClipAtPoint(clip, position);
    }
    public void PlayAllEffects(string effectName)
    {
        if (_listener == null)
        {
            _listener = FindObjectOfType<AudioListener>();
        }
        SoundEffect soundsCollection = _effectDictionary[effectName];
        
        foreach(var sound in soundsCollection.clips)
        {
            AudioSource.PlayClipAtPoint(sound, _listener.transform.position);
        }
       
    }
    public void PlayAllEffectsWrapper(string effectName)
    {
        StartCoroutine(PlayAllEffectsCoroutine(effectName));
    }
    public IEnumerator PlayAllEffectsCoroutine(string effectName)
    {
        if (_listener == null)
        {
            _listener = FindObjectOfType<AudioListener>();
        }

        SoundEffect soundsCollection = _effectDictionary[effectName];
        foreach (var clip in soundsCollection.clips)
        {
            StartCoroutine(PlayClipCoroutine(clip, _listener.transform.position));
        }
        yield return null;
    }
    private IEnumerator PlayClipCoroutine(AudioClip clip, Vector3 position)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, position);
        }
        else
        {
            Debug.LogWarning("Null clip provided to PlayClipCoroutine.");
        }
        yield return null; // This coroutine ends immediately after playing the clip.
    }
    public void PlayBackgroundMusic(string effectName)
    {
        if (_backgroundMusicCoroutine != null)
        {
            StopCoroutine(_backgroundMusicCoroutine);
        }

        if (!_effectDictionary.ContainsKey(effectName))
        {
            Debug.LogWarningFormat("Effect {0} is not registered.", effectName);
            return;
        }

        var soundEffect = _effectDictionary[effectName];

        _backgroundMusicCoroutine = StartCoroutine(PlayClipsInLoop(soundEffect));
    }

    private IEnumerator PlayClipsInLoop(SoundEffect soundEffect)
    {
        if (soundEffect.clips == null || soundEffect.clips.Length == 0)
        {
            Debug.LogWarning("No clips available to loop.");
            yield break;
        }

        while (true) // Infinite loop for continuous playback
        {
            foreach (var clip in soundEffect.clips)
            {
                if (clip == null) continue;

                if (_listener == null)
                {
                    _listener = FindObjectOfType<AudioListener>();
                }
                AudioSource.PlayClipAtPoint(clip, _listener.transform.position, gameSettings.volume); // Play clips at 50% volume
                yield return new WaitForSeconds(clip.length); // Wait for the clip to finish
            }
        }
    }

    public void StopBackgroundMusic()
    {
        if (_backgroundMusicCoroutine != null)
        {
            StopCoroutine(_backgroundMusicCoroutine);
            _backgroundMusicCoroutine = null;
        }
    }
}
