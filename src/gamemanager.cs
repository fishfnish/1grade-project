using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class gamemanager : MonoBehaviour
{
    // 게임 퍼즈
    public static bool GameIsPaused;
    public GameObject ESCpanel;
    public GameObject VolumePanel;
    public GameObject ScreenSizePanel;
    public AudioSource BGM;
    public AudioSource SFX;

    // Start is called before the first frame update
    public player pc;
    public skills skills;
    // static public skills_manager skills_Manager;

    public GameObject player_c;

    static public float pi = 3.141592f;
    public TextMeshProUGUI game_over_txt;

    public static bool GameOver = false;
    public float time;
    void Start()
    {
        GameIsPaused = false;
        ESCpanel.SetActive(false);
        VolumePanel.SetActive(false);
        ScreenSizePanel.SetActive(false);
        player_c = GameObject.FindGameObjectWithTag("Player");

        pc = player_c.GetComponent<player>();
        skills = player_c.GetComponent<skills>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameIsPaused)
        {
            Time.timeScale = time;
        }
        // Debug.Log(pns.now_hp);
        // Debug.Log(pc);
    }
    public void EscMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!GameIsPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }
    public void Pause()
    {
        Debug.Log("Pause");
        ESCpanel.SetActive(true);
        BGM.Pause();
        SFX.Pause();
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    public void Resume()
    {
        Debug.Log("Resume");
        ESCpanel.SetActive(false);
        BGM.UnPause();
        SFX.UnPause();
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    public void Volume()
    {
        VolumePanel.SetActive(true);
    }
    public void ScreenSize()
    {
        ScreenSizePanel.SetActive(true);
    }
    public void Return()
    {
        if (VolumePanel.activeSelf) VolumePanel.SetActive(false);
        else if (ScreenSizePanel.activeSelf) ScreenSizePanel.SetActive(false);

    }
    public void QuitGame()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }
}
