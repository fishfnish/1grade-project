using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundManager : MonoBehaviour
{
    public bool audioPlayed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void changeSoundClip(AudioClip audioClip, AudioSource audioSource)
    {
        if (Time.timeScale <= 0) return;
        audioPlayed = false;
        if (audioSource.clip != audioClip)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            audioPlayed = false;
        }
        if (!audioPlayed)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
            audioPlayed = true;
        }
        if (!audioSource.isPlaying)
        {
            audioPlayed = false;
        }
    }
}
