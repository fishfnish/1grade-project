using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Unity.VisualScripting;
// using System.Diagnostics;

public class bullet : MonoBehaviour
{   
    public skills_manager.skill skill_op;
    public monster monster;
    public Vector3 moveD;
    bool HitCheck = true;
    RaycastHit hit;
    void Start()
    {   
        monster = skill_op.sp_pos.GetComponent<monster>();
        moveD = monster.monster_now_stat.move_D;
    }
    // Update is called once per frame
    void Update()
    {
        // Debug.Log("mon :" + monster.monster_now_stat.move_D);
        // Debug.Log("moveD :" + moveD);
        Destroy(gameObject,skill_op.life_time);

        if (HitCheck)
        {
            Vector3 input = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(input);
            Physics.Raycast(ray, out hit);
            Debug.Log("hit " + hit.point);
            HitCheck = false;
        }
        if (skill_op.target_ting == null) 
        {
            gameObject.transform.LookAt(hit.point);
            gameObject.transform.position = Vector3.MoveTowards(transform.position, hit.point, skill_op.speed * Time.deltaTime);
            // Debug.Log(hit.point);
        }
        else if(skill_op.target_ting != null) // 몬스터 전용 불렛 
        {
            transform.Translate(moveD * skill_op.speed * Time.deltaTime);
            // transform.position = Vector3.MoveTowards(transform.position, target + new Vector3(0f,1f,0f), skill_op.speed * Time.deltaTime);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != skill_op.sp_pos)
        {   
            Destroy(gameObject);
        }
    }
}
