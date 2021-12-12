using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager AM;
    public Sound[] sounds;
    private static bool notFirstLoad;

    private void Awake()
    {
        
        if (AM != null)
            GameObject.Destroy(AM);
        else
            AM = this;

        DontDestroyOnLoad(this);
        
    }
    private void Start()
    {
        foreach (Sound s in sounds)
        {
            if (PauseMenu.notFirstLoad)
            {
                s.volume = PauseMenu.volume;
            }
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.playOnAwake = false;
            s.source.loop = s.canLoop;
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            
        }
        PlaySound("Music1");
        if (!notFirstLoad)
        {
            notFirstLoad = true;
            PauseMenu.volume = 0.5f;
        }
        Debug.Log(PauseMenu.volume);
        AudioManager.SetVolume(PauseMenu.volume);
    }

    public static void PlaySound(string name)
    {
        Sound s = Array.Find(AM.sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.Play();
        }
    }
    public static void StopSound(string name)
    {
        Sound s = Array.Find(AM.sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.Stop();
        }
    }
    public static void MuteSound(string name)
    {
        Sound s = Array.Find(AM.sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.volume = 0;
        }
    }
    public static void UnmuteSound(string name)
    {
        Sound s = Array.Find(AM.sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.volume = s.volume;
        }
    }
    public static void SetVolume(float volume)
    {
        foreach (Sound s in AM.sounds)
        {
            s.source.volume = volume;
            s.volume = volume;
        }
    }

    public static void SetVolume(string name, float volume)
    {
        Sound s = Array.Find(AM.sounds, sound => sound.name == name);
        if (s != null && s.source != null)
        {
            //Debug.Log("SOURCE IS " + s.source);
            s.source.volume = volume;
            s.volume = volume;
        }
    }

    public static void SetSound(string nName, AudioClip newSound)
    {
        Sound s = Array.Find(AM.sounds, sound => sound.name == nName);
        if (s != null)
        {
            s.source.clip = newSound;
        }
    }
    public static Sound GetSound(string name)
    {
        return Array.Find(AM.sounds, sound => sound.name == name);
    }
}
