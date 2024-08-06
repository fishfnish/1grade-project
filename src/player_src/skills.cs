using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;

// using System.Numerics;

using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class skills : MonoBehaviour
{
    public stats.stat player_stat;
    public skills_manager sk_manager;
    public int attect_combo = 3;
    public bool is_delay;
    bool is_attect;
    public float co_time;
    public float attect_delay;
    Animator animator;
    player pc;
    public List<string> skill_ID = new List<string>();


    
    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponent<player>();
        
        animator = GetComponent<Animator>();
        co_time = attect_delay;
        
    }
    void Update()
    {
        player_stat = pc.player_stat;
    }

    
    public void on_attack(InputAction.CallbackContext context){
        if (context.performed && !player_stat.is_skill) {
            StartCoroutine(attect_ck());
            if (context.interaction is PressInteraction) {
            co_time = (sk_manager.skill_dict[skill_ID[attect_combo]].life_time+sk_manager.skill_dict[skill_ID[attect_combo]].after_delay) * 1.40f;
                if(!is_attect && attect_combo>0) attect_combo = 0;
                if (attect_combo <= 0) {
                    // animator.SetBool("isAttacking", true);
                    animator.SetBool("s_at", true);
                } else {
                    // Debug.Log("n_at");
                    animator.SetBool("s_at", false);
                    animator.SetBool("attect", true);
                    animator.SetTrigger("next_at");
                }
                StartCoroutine(use_Skills_delay(skill_ID[3]));
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
            if(t==0)StartCoroutine(dash_life(sk_manager.skill_dict[skill_ID[4]].life_time));
        }
    }
    public void use_skiil(InputAction.CallbackContext context){
        
    }

    
    public IEnumerator use_Skills_delay(string ID){
        pc.player_stat.is_skill = true;
        Debug.Log("ID : " + ID);
        if(sk_manager.skill_dict[ID].before_delay > 0){
            for(float i = sk_manager.skill_dict[ID].before_delay; i>0 ;i-=Time.deltaTime)
                yield return new WaitForSeconds(Time.deltaTime);
        } 
        sk_manager.use_skill(ID,gameObject);
        if(sk_manager.skill_dict[ID].life_time > 0 && (sk_manager.skill_dict[ID].skill_type == 0 || sk_manager.skill_dict[ID].skill_type == 2)){
            for(float i = sk_manager.skill_dict[ID].life_time; i>0 ;i-=Time.deltaTime)
                yield return new WaitForSeconds(Time.deltaTime);
        }
        if(sk_manager.skill_dict[ID].after_delay > 0){
            for(float i = sk_manager.skill_dict[ID].after_delay; i>0 ;i-=Time.deltaTime)
                yield return new WaitForSeconds(Time.deltaTime);
        }
        pc.player_stat.is_skill = false;
    }
    public IEnumerator dash_life(float time){
        animator.SetBool("dash",true);
        player_stat.is_dash = true;
        for(float i = time; i>0 ;i-=Time.deltaTime)
                yield return new WaitForSeconds(Time.deltaTime);
        player_stat.is_dash = false;
        animator.SetBool("dash",false);

    }
    public IEnumerator attect_ck(){
        // animator.SetBool("s_at",true);
        is_attect = true;
        for(; co_time>0 ;co_time-=Time.deltaTime){
            yield return new WaitForSeconds(Time.deltaTime);
        }
        // 공격 종료 처리
        is_attect = false;
        animator.SetBool("s_at", is_attect);
        animator.SetBool("attect", is_attect);
    }
}
