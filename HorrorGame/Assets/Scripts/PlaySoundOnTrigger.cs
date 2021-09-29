using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlaySoundOnTrigger : MonoBehaviour
{
    AudioSource s;
    // Start is called before the first frame update
    void Start()
    {
        s = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        while (s.isPlaying == false)
        {
            s.Play();
        }
    }
}
