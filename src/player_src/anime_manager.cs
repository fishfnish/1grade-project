using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class anime_manager : MonoBehaviour
{
    // Start is called before the first frame update
    player pc;
    Animator animator;
    skills skills;
    stats.stat player_stat;
    float move_speed;
    void Start()
    {
        pc = GetComponent<player>();
        
        animator = GetComponent<Animator>();
        skills = GetComponent<skills>();
    }

    // Update is called once per frame
    void Update()
    {
        player_stat = pc.player_stat;
        // transform.position = pc.transform.position;
        // transform.rotation = pc.transform.rotation;
        if(player_stat.move_situation < 0){
            animator.SetBool("move",true);
            if(player_stat.move_situation==-3)animator.SetFloat("run",move_speed);
            else if(player_stat.move_situation==-2)animator.SetFloat("run",move_speed);
        }
        else animator.SetBool("move", false);
        if(player_stat.move_situation==-2){
            if(move_speed<1)move_speed+=Time.deltaTime;
        }
        else{if(move_speed>0)move_speed-=Time.deltaTime;}
        
    }
}
