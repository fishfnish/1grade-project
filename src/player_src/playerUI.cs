using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerUI : MonoBehaviour
{
    public GameObject pc;
    public player player;
    public Slider pcSlider;
    // Start is called before the first frame update
    void Start()
    {
        pc = GameObject.FindGameObjectWithTag("Player");
        player = pc.GetComponent<player>();
        pcSlider = GetComponentInChildren<Slider>();
    }

    // Update is called once per frame
    void Update()
    {

        pcSlider.value = player.player_stat.hp / player.max_HP;
    }
}
