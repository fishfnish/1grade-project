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
    public GameObject VolumePanel;
    public GameObject ScreenSizePanel;
    public AudioSource BGM;
    public AudioSource SFX;
    // 볼륨
    public AudioMixer GameMixer;
    public Slider MasterAudioSlider;
    public Slider BGMAudioSlider;
    public Slider SFXAudioSlider;
    public float[] sound = new float[3]; // 0. Master 1.BGM 2. SFX
    ///////////////////// 텍스트 출력
    // 해상도
     public TMP_Dropdown resolutionDropdown;  // 해상도 옵션을 표시할 Dropdown UI
    private Resolution[] resolutions;  // 사용 가능한 해상도 목록
    // Start is called before the first frame update
    void Start()
    {
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
    public void AudioControl()
    {
        sound[0] = MasterAudioSlider.value;
        sound[1] = BGMAudioSlider.value;
        sound[2] = SFXAudioSlider.value;

        if (sound[0] == -40f) GameMixer.SetFloat("Master", -80);
        else GameMixer.SetFloat("Master", sound[0]);

        if (sound[1] == -40f) GameMixer.SetFloat("BGM", -80);
        else GameMixer.SetFloat("BGM", sound[1]);

        if (sound[2] == -40f) GameMixer.SetFloat("SFX", -80);
        else GameMixer.SetFloat("SFX", sound[2]);
    }
    public void ToggleAudioVolume()
    {
        AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
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
