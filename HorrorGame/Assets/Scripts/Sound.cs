using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public AudioClip clip;
    [HideInInspector]
    public AudioSource source;
    public bool canLoop;
    [Range(0, 1)]
    public float volume;
    public string name;
}
