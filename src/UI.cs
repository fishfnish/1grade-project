using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class UI : MonoBehaviour
{
    // 볼륨
    public AudioMixer masterMixer;
    public Slider MasterAudioSlider;
    public Slider BGMAudioSlider;
    public Slider SFXAudioSlider;
    public float[] sound = new float[3]; // 0. Master 1.BGM 2. SFX
    ///////////////////// 텍스트 출력
    // 해상도
     public TMP_Dropdown resolutionDropdown;  // 해상도 옵션을 표시할 Dropdown UI
    private Resolution[] resolutions;  // 사용 가능한 해상도 목록

    public TextMeshProUGUI weapon_txt;
    public TextMeshProUGUI bullet_txt;
    ///////////////////// 

    /////////////////////체력,스테미나 바
    public Slider hp_bar;
    public Slider stamina_bar;
    ///////////////////// 

    public GameObject[] bullet_cnt_img = new GameObject[6];
    public GameObject bullet_img;
    // public GameObject player;
    public player pc;
    int bullcnt;

    public TextMeshProUGUI timer;
    int m, ss;
    static public float s;

    public float max_scoal_x;
    public float max_scoal_y;
    // Start is called before the first frame update
    void Awake()
    {
        ResolutionOptionAdd();

        // pns = GetComponent<player_now_state>();
        // pc = GetComponent<player>();

        max_scoal_x = Screen.width;
        max_scoal_y = Screen.height;
        // Debug.Log(max_scoal_x + " " + max_scoal_y);
        // for(int i=0; i<pc.bullet_cnt; i++){
        //     bullet_cnt_img[i]  = Instantiate(bullet_img, new Vector2((max_scoal_x-100)-(60*i),60), Quaternion.identity,GameObject.Find("Canvas").transform);
        // }
        s = m = 0;
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        // weapon_txt.text = pns.weapon[pns.now_weapon];
        // bullet_txt.text = " "+ pns.bullet_cnt;

        hp_bar.value = pc.player_stat.hp / pc.max_HP;

        s += Time.deltaTime;
        ss = (int)s;
        if (ss >= 60) { m++; ss = 0; s = 0; }
        timer.text = " " + m + " : " + ss;
    }
    public void AudioControl()
    {
        sound[0] = MasterAudioSlider.value;
        sound[1] = BGMAudioSlider.value;
        sound[2] = SFXAudioSlider.value;

        if (sound[0] == -40f) masterMixer.SetFloat("Master", -80);
        else masterMixer.SetFloat("Master", sound[0]);

        if (sound[1] == -40f) masterMixer.SetFloat("BGM", -80);
        else masterMixer.SetFloat("BGM", sound[1]);

        if (sound[2] == -40f) masterMixer.SetFloat("SFX", -80);
        else masterMixer.SetFloat("SFX", sound[2]);
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
}
