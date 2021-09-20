using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    private void Awake()
    {
        foreach (Sound s in sounds)
        {
            

            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
        }
    }

    private void Start()
    {
        PlaySound("Music1");
        PlaySound("Music2");
    }

    public void PlaySound(string name)
    {
        // Find the sound with the input name
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
}
