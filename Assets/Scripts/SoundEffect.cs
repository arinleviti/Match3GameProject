using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class SoundEffect : ScriptableObject
{
    public AudioClip[] clips;

    public AudioClip GetRandomClip()
    {
        if (clips.Length == 0)
        {
            return null;
        }
        return clips[Random.Range(0, clips.Length)];  

    }

}
