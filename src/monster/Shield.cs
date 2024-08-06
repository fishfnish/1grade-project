using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
// using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public stats.stat stat;
    public monster monster;
    public player player;
    public dash dash;
    public skills_manager sk_manager;
    private skills_manager.skill skill_op;

    public bool[] can = new bool[2]; // 0. 방패치기 1. 돌진
    public float[] original = new float[2]; // 오리지널 0. 데미지 1. 스턴시간

    public bool isStun;
    public float shieldStun; // 패링 스턴
    public float damagePlus; // 데미지 배율
    public float damagePlusTime; // 데미지 배율 적용시간

    public List<string> skill_ID = new List<string>();

    // Start is called before the first frame update

    void Start()
    {
        isStun = false;
        stat.is_dash = false;

        can[0] = true;
        can[1] = true;

        monster = GetComponent<monster>();
        player = monster.player;
        sk_manager = monster.sk_manager;
        
        original[0] = player.player_stat.demege;
        original[1] = monster.stun;

        monster.monster_now_stat = stat;
        monster.maxhp = stat.hp;
    }
    // Update is called once per frame
    void Update()
    {
        if (monster != null && monster.range >= monster.distance)
        {
            if (stat.is_dash && monster.isSword)
            {
                StartCoroutine(parryStun());
                monster.isSword = false;
            }
            else if (can[1])
            {
                StartCoroutine(PerformDash(skill_ID[1]));
            }
        }
    }
    private IEnumerator PerformAttack(string ID) // 방패치기
    {
        if (stat.is_dash)
        {
            yield break;
        }
        can[0] = false;

        monster.monster_now_stat.speed = 0;
        if (sk_manager.skill_dict[ID].before_delay > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].before_delay));
        }
        Debug.Log("attackStart");

        sk_manager.use_skill(ID, gameObject);

        if (sk_manager.skill_dict[ID].after_delay > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].after_delay));
        }
        Debug.Log("attackEnd");
        if (sk_manager.skill_dict[ID].cool_time > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].cool_time));
        }

        monster.monster_now_stat.speed = stat.speed;

        can[0] = true;
    }
    private IEnumerator PerformDash(string ID) // 대쉬 
    {
        can[1] = false;

        monster.monster_now_stat.speed = 0;

        Debug.Log("charging");
        stat.is_dash = true;
        if (sk_manager.skill_dict[ID].before_delay > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].before_delay));
        }

        Debug.Log("dashStart");
        sk_manager.use_skill(ID, gameObject);
        dash = GetComponent<dash>();

        if (monster.damaged)
        {
            yield break;
        }

       if (sk_manager.skill_dict[ID].life_time > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].life_time));
        }
        Debug.Log("dashEnd");
        stat.is_dash = false;
        if (sk_manager.skill_dict[ID].after_delay > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].after_delay));
        }

        monster.monster_now_stat.speed = stat.speed;
        if (sk_manager.skill_dict[ID].cool_time > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].cool_time));
        }
        can[1] = true;
    }
    private IEnumerator parryStun() // 패링 스턴
    {
        Destroy(dash);

        stat.is_dash = false;
        Debug.Log("dashEnd2");
        
        monster.stun = shieldStun;
        player.player_stat.demege = player.player_stat.demege * damagePlus;
        yield return new WaitForSeconds(damagePlusTime);

        player.player_stat.demege = original[0];
        monster.stun = original[1];
    }
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(dash);
            if (can[0])
            {
                StartCoroutine(PerformAttack(skill_ID[0]));
            }
        }
    }
}
