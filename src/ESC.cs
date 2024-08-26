using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using TMPro;


public class ESC : MonoBehaviour
{
    // 게임 퍼즈
    public static bool GameIsPaused;
    public GameObject ESCpanel;
    public GameObject PauseMenu;
    public GameObject VolumePanel;
    public GameObject ScreenSizePanel;
    public AudioSource BGM;
    public AudioSource SFX;
    // 볼륨
    public AudioMixer GameMixer;
    public Slider[] AudioSlider; // 0. MasterAudioSlider 1. BGMAudioSlider 2. SFXAudioSlider
    public float[] sound = new float[3]; // 0. Master 1.BGM 2. SFX
    ///////////////////// 텍스트 출력
    // 해상도
    public TMP_Dropdown resolutionDropdown;  // 해상도 옵션을 표시할 Dropdown UI
    private Resolution[] resolutions;  // 사용 가능한 해상도 목록
    // Start is called before the first frame update
    void Start()
    {
        ESCpanel = GameObject.Find("ESCPanel");
        PauseMenu = GameObject.Find("PauseMenu");
        VolumePanel = GameObject.Find("VolumePanel");
        ScreenSizePanel = GameObject.Find("ScreenSizePanel");
        AudioSlider = GetComponentsInChildren<Slider>();
        resolutionDropdown = GetComponentInChildren<TMP_Dropdown>();
        GameIsPaused = false;
        ESCpanel.SetActive(false);
        VolumePanel.SetActive(false);
        ScreenSizePanel.SetActive(false);
        ResolutionOptionAdd();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void EscMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (GameIsPaused) Resume();
            else Pause();
        }
    }
    public void AudioControl()
    {
        string[] audioTypes = { "Master", "BGM", "SFX" };

        for (int i = 0; i < AudioSlider.Length; i++)
        {
            sound[i] = AudioSlider[i].value;

            if (sound[i] == -40f) GameMixer.SetFloat(audioTypes[i], -80);
            else GameMixer.SetFloat(audioTypes[i], sound[i]);
        }
    }
    public void ToggleAudioVolume()
    {
        float currentVolume;
        GameMixer.GetFloat("Master", out currentVolume);
        GameMixer.SetFloat("Master", currentVolume == -80 ? 0 : -80);
    }
    public void ResolutionOptionAdd()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        int currentResolutionIndex = 0;
        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height + " " + resolutions[i].refreshRate + "Hz";
            options.Add(option);

            // 현재 해상도와 일치하는 옵션을 찾음
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void Pause()
    {
        SetActivePanel(PauseMenu);
        ESCpanel.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Debug.Log("게임 일시정지");
    }
    public void Resume()
    {
        SetActivePanel(null);
        ESCpanel.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Debug.Log("게임 재개");
    }
    private void SetActivePanel(GameObject activePanel)
    {
        PauseMenu.SetActive(activePanel == PauseMenu);
        VolumePanel.SetActive(activePanel == VolumePanel);
        ScreenSizePanel.SetActive(activePanel == ScreenSizePanel);
    }
    public void Volume() => SetActivePanel(VolumePanel);
    public void ScreenSize() => SetActivePanel(ScreenSizePanel);
    public void Return() => SetActivePanel(PauseMenu);

    public void QuitGame()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }
}
