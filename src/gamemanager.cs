using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class gamemanager : MonoBehaviour
{
    

    // Start is called before the first frame update
    public player pc;
    public skills skills;
    // static public skills_manager skills_Manager;

    public GameObject player_c;

    static public float pi = 3.141592f;
    public TextMeshProUGUI game_over_txt;

    public static bool GameOver = false;
    public bool audioPlayed;
    public float time;
    void Start()
    {
        
        player_c = GameObject.FindGameObjectWithTag("Player");

        pc = player_c.GetComponent<player>();
        skills = player_c.GetComponent<skills>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ESC.GameIsPaused == false)
        {
            Time.timeScale = time;
        }
        // Debug.Log(pns.now_hp);
        // Debug.Log(pc);
    }
    public void changeSoundClip(AudioClip audioClip, AudioSource audioSource, bool repeat)
    {
        if (Time.timeScale > 0)
        {
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
            else if (!audioSource.isPlaying)
            {
                if (repeat)
                {
                    audioPlayed = false;
                }
            }
        }
    }
    
}
