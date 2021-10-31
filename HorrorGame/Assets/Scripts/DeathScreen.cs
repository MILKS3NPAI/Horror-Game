using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class DeathScreen : MonoBehaviour
{
    private float timeOfDeath;
    private GameObject deathMenu;
    private VideoPlayer videoPlayer;
    private void Start()
    {
        timeOfDeath = Time.time;
        deathMenu = transform.GetChild(2).gameObject;
        videoPlayer = transform.GetChild(0).gameObject.GetComponent<VideoPlayer>();
        videoPlayer.SetDirectAudioVolume(0, PauseMenu.volume/2);
        for (int i = 0; i < AudioManager.AM.sounds.Length; i++)
        {
            AudioManager.StopSound(AudioManager.AM.sounds[i].name);
        }
    }
    private void Update()
    {
        if (!videoPlayer.isPlaying && Time.time - 5 > timeOfDeath)
        {
            deathMenu.SetActive(true);
        }
    }
}
