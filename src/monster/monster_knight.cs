using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class monster_knight : MonoBehaviour
{
    //public stats.stat monster_stat;
    public monster ms;
    public skills_manager sk_manager;
    public Animator anim;
    public float attack_range;
    public List<string> skill_ID = new List<string>();
    public bool can;

    void Start()
    {
        can = true;
        ms = GetComponent<monster>();
        anim = GetComponent<Animator>();
        ms.target = GameObject.FindGameObjectWithTag("Player");
        if (sk_manager == null)
        {
            GameObject game_manager = GameObject.Find("gamemanager");
            sk_manager = game_manager.GetComponent<skills_manager>();
        }
    }


    void Update()
    {
        //ms.monster_now_stat = monster_stat;

        if (can)
        {
            anim.SetBool("Is_run",true);

            if (attack_range >= ms.distance)
            {
                StartCoroutine(knight_attack(skill_ID[0]));
            }
        }
        //else if (attack_range >= ms.distance)
        //{
        //    anim.SetBool("Is_attack", true);
        //}
    }
    IEnumerator knight_attack(string ID)
    {
        can = false;
        ms.monster_now_stat.speed = 0;
        anim.SetBool("Is_run", false);
        anim.SetBool("Is_attack", true);
        if (sk_manager.skill_dict[ID].before_delay > 0)
        {
            yield return StartCoroutine(ms.WaitForDelay(sk_manager.skill_dict[ID].before_delay));
        }

        //sk_manager.use_skill(ID, gameObject);

        if (sk_manager.skill_dict[ID].after_delay > 0)
        {
            yield return StartCoroutine(ms.WaitForDelay(sk_manager.skill_dict[ID].after_delay));
        }
  

        if (sk_manager.skill_dict[ID].cool_time > 0)
        {
            yield return StartCoroutine(ms.WaitForDelay(sk_manager.skill_dict[ID].cool_time));
        }
        ms.monster_now_stat.speed = ms.originalSpeed;
        anim.SetBool("Is_attack", false);
        can = true;
    }

    public void use_skiils(string index)
    {
        sk_manager.use_skill(index, gameObject);
    }
    IEnumerator Slash(float delay)
    {


        //yield return new WaitForSeconds(ms.pre_attack_delay);



        //yield return new WaitForSeconds(ms.post_attack_delay);
        yield return 0;

    }

}
