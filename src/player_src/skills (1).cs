using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.Tracing;

// using System.Numerics;

using Unity.VisualScripting;
using UnityEditor;
// using UnityEditor.Rendering;
// using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class skills : MonoBehaviour
{
    public soundManager soundManager;
    public AudioSource audioSource;
    public AudioClip[] swordClip = new AudioClip[3];
    public AudioClip dashClip;
    public AudioClip bulletClip;
    public stats.stat player_stat;
    public skills_manager sk_manager;
    public skills_manager.skill sk_op;
    public int attect_combo = 0;
    public bool is_delay;
    public bool is_attect;
    public float co_time = 0;
    public float attect_delay;
    Animator animator;
    player pc;
    public List<string> skill_ID = new List<string>();

    bool test1=false;
    public Vector3 sp_pos2;
    
    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponent<player>();
        audioSource = GetComponent<AudioSource>();
        
        animator = GetComponent<Animator>();
        co_time = 0;
        
    }
    void Update()
    {
        player_stat = pc.player_stat;
        if(co_time > 0) co_time -= Time.deltaTime;
        else if(co_time < 0){
            is_attect = false;
            attect_combo = 0;
            sk_manager.cooldownTimers[skill_ID[attect_combo]] = 0;
            animator.SetBool("s_at", false);
            animator.SetBool("attect", is_attect);
        }
    }

    
    public void on_attack(InputAction.CallbackContext context){
        if (context.performed && !player_stat.is_skill && sk_manager.cooldownTimers[skill_ID[attect_combo]] <= 0) {
            if (context.interaction is PressInteraction) {
                is_attect = true;
                if (attect_combo <= 0) {
                    StartCoroutine(dash_at(sk_manager.skill_dict[skill_ID[attect_combo]].life_time * 0.1f));
                    animator.SetBool("s_at", true);
                }
                if(co_time > 0) {
                    // Debug.Log("test");
                    animator.SetBool("s_at", false);
                    animator.SetBool("attect", true);
                    animator.SetTrigger("next_at");
                }
                StartCoroutine(dash_at(0.4f));
                StartCoroutine(use_Skills_delay(skill_ID[attect_combo]));
                co_time = (sk_manager.skill_dict[skill_ID[attect_combo]].before_delay+sk_manager.skill_dict[skill_ID[attect_combo]].after_delay+sk_manager.skill_dict[skill_ID[attect_combo]].life_time)*attect_delay;
                attect_combo = attect_combo >= 2 ? 0 : attect_combo + 1;
            }
        }
    }
    public void on_m_at(InputAction.CallbackContext context){
        if(context.performed){
            animator.SetTrigger("m_at_");
            StartCoroutine(use_Skills_delay(skill_ID[3]));
        }
    }
    public void use_dsah(InputAction.CallbackContext context){
        if(context.performed){
            int t = sk_manager.use_skill(skill_ID[4],gameObject);
            // Debug.Log(t);
            if(t==0)StartCoroutine(dash_life(sk_manager.skill_dict[skill_ID[4]].life_time));
        }
    }
    public void use_skiil(InputAction.CallbackContext context){
        
    }
    public IEnumerator use_Skills_delay(string ID){
        pc.player_stat.is_skill = true;
        if(sk_manager.skill_dict[ID].before_delay > 0){
            for(float i = sk_manager.skill_dict[ID].before_delay; i>0 ;i-=0.1f)
                yield return new WaitForSeconds(0.1f);
        } 
        sk_manager.use_skill(ID,gameObject);
        if (sk_manager.skill_dict[ID].skill_type == 0) 
        {
            if(attect_combo == 1) soundManager.changeSoundClip(swordClip[1], audioSource);
            if(attect_combo == 2) soundManager.changeSoundClip(swordClip[2], audioSource);
        }
        else if (sk_manager.skill_dict[ID].skill_type == 1) soundManager.changeSoundClip(dashClip, audioSource);
        Debug.Log("ID : " + ID);
        if(sk_manager.skill_dict[ID].life_time > 0 && (sk_manager.skill_dict[ID].skill_type == 0 || sk_manager.skill_dict[ID].skill_type == 2)){
            for(float i = sk_manager.skill_dict[ID].life_time; i>0 ;i-=0.1f)
                yield return new WaitForSeconds(0.1f);
        }
        pc.player_stat.is_skill = false;
        if(sk_manager.skill_dict[ID].after_delay > 0){
            for(float i = sk_manager.skill_dict[ID].after_delay; i>0 ;i-=0.1f)
                yield return new WaitForSeconds(0.1f);
        }
    }
    public IEnumerator dash_life(float time){
        animator.SetBool("dash",true);  
        player_stat.is_dash = true;
        soundManager.changeSoundClip(dashClip, audioSource);
        for(float i = time; i>0 ;i-=0.1f)
                yield return new WaitForSeconds(0.1f);
        player_stat.is_dash = false;
        animator.SetBool("dash",false);
    }
    public IEnumerator dash_at(float time){
        animator.SetBool("dash_at",true);  
        soundManager.changeSoundClip(swordClip[0], audioSource);
        for(float i = time; i>0 ;i-=0.1f)
                yield return new WaitForSeconds(0.1f);
        animator.SetBool("dash_at",false);
    }
    public IEnumerator attect_ck(){
        // animator.SetBool("s_at",true);
        for(; co_time>0 ;co_time-=0.1f){
            yield return new WaitForSeconds(0.1f);
        }
    }
    // public IEnumerator dash_ef(float time){
    //     for(float i = time; i>0 ;i-=0.1f)
    //             yield return new WaitForSeconds(0.1f);
                
    //     sk_op = sk_manager.skill_dict[skill_ID[0]];
    //     Quaternion t_rot = Quaternion.Euler(new Vector3(
    //         0f,
    //         sk_op.sp_pos.transform.rotation.eulerAngles.y + sk_op.sp_rot ,
    //         0
    //     ));
    //     Vector3 t_pos = new Vector3(
    //         sk_op.sp_pos.transform.position.x+(sk_op.sp_pos2.x * MathF.Cos((t_rot.eulerAngles.y-90)*(Mathf.PI / 180))),
    //         sk_op.sp_pos.transform.position.y + sk_op.sp_pos2.y,
    //         sk_op.sp_pos.transform.position.z+(sk_op.sp_pos2.z * Mathf.Sin((t_rot.eulerAngles.y+90)*(Mathf.PI / 180)))
    //     );
    //     GameObject sk_ef = Instantiate(sk_op.skills_obj,t_pos,t_rot);
    //     skills_ef sk_ef2 = sk_ef.AddComponent<skills_ef>();
    //     sk_ef2.sk_op = sk_op;
        
    // }
}