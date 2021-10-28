using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FloorSound : MonoBehaviour
{
    [SerializeField] string soundName = "Step1";
    [SerializeField] float soundVolume = .5f;
    [SerializeField] AudioClip newSound;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            AudioManager.SetSound(soundName, newSound);
            AudioManager.SetVolume("Step1", soundVolume);
            AudioManager.PlaySound("Step1");
        }
    }
}
