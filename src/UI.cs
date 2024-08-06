using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UI : MonoBehaviour
{


    ///////////////////// 텍스트 출력
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
    

}
