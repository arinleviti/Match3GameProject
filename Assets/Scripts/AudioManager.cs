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
    private bool isMusicPlaying = true;
    private List<AudioSource> _activeAudioSources = new List<AudioSource>();

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

                // Create a new GameObject to hold the AudioSource
                GameObject audioObject = new GameObject("BackgroundMusicSource");
                AudioSource source = audioObject.AddComponent<AudioSource>();
                source.clip = clip;
                source.volume = gameSettings.volume;
                source.loop = false; // Ensure the clip itself does not loop
                source.Play();

                // Track the AudioSource for cleanup later
                _activeAudioSources.Add(source);

                // Destroy the GameObject after the clip has finished playing
                Destroy(audioObject, clip.length);

                // Wait for the clip to finish before playing the next one
                yield return new WaitForSeconds(clip.length);
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

        // Stop and clean up all active sources
        foreach (var source in _activeAudioSources)
        {
            if (source != null)
            {
                source.Stop();
                Destroy(source.gameObject); // Clean up the AudioSource object created by PlayClipAtPoint
            }
        }
        _activeAudioSources.Clear();
    }

    public void ToggleMusic()
    {
        if (isMusicPlaying)
        {
            StopBackgroundMusic();
        }
        else
        {
            PlayBackgroundMusic("Background Music");
        }

        isMusicPlaying = !isMusicPlaying;
    }
}
