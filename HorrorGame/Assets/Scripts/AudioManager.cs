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
            s.source.loop = s.canLoop;
            s.source.clip = s.clip;
            s.source.volume = s.volume;
        }
    }

    private void Start()
    {
        PlaySound("Music1");
        PlaySound("Music2");
    }

    private void Update()
    {
        if (PlayerRoom().Equals("right"))
        {
            //Debug.Log("Player is in the right room.");
            MuteSound("Music2");
            UnmuteSound("Music1");
        }
        else if (PlayerRoom().Equals("left"))
        {
            //Debug.Log("Player is in the left room.");
            MuteSound("Music1");
            UnmuteSound("Music2");
        }
        else
        {
            Debug.Log("Player is not in a room.");
        }
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.Play();
        }
    }
    public void PauseSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.Pause();
        }
    }
    public void MuteSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.volume = 0;
        }
    }
    public void UnmuteSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.volume = s.volume;
        }
    }

    // Determines what room the player is in
    private string PlayerRoom()
    {
        GameObject player = FindObjectOfType<Player>().gameObject;
        if (player != null)
        {
            if (player.transform.position.x >= 0)
            {
                return "right";
            }
            else
            {
                return "left";
            }
        }
        else
        {
            return "";
        }
    }
}
