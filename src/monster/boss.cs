using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss : MonoBehaviour
{
    // test
    public stats.stat stat;
    public monster monster;
    public player player;
    public skills_manager sk_manager;
    public GameObject shield;
    public GameObject wave;
    // Start is called before the first frame update
    void Start()
    {
        monster = GetComponent<monster>();
        player = monster.player;
        sk_manager = monster.sk_manager;

        monster.monster_now_stat = stat;
        monster.maxhp = stat.hp;
    }

    // Update is called once per frame
    void Update()
    {
        if (monster != null && monster.range >= monster.distance)
        {
            
        }
    }
    private IEnumerator PerformBossAttack(string ID) // 휘두르기
    {
        monster.monster_now_stat.speed = 0;
        if (sk_manager.skill_dict[ID].before_delay > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].before_delay));
        }
        Debug.Log("BossAttackStart");

        sk_manager.use_skill(ID, gameObject);

        if (sk_manager.skill_dict[ID].after_delay > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].after_delay));
        }

        Debug.Log("BossAttackEnd");
        if (sk_manager.skill_dict[ID].cool_time > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].cool_time));
        }

        monster.monster_now_stat.speed = stat.speed;
    }
    private IEnumerator PerformSmashDown(string ID) // 내려찍기
    {
        monster.monster_now_stat.speed = 0;
        if (sk_manager.skill_dict[ID].before_delay > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].before_delay));
        }
        Debug.Log("SmashDownStart");

        sk_manager.use_skill(ID, gameObject);

        if (sk_manager.skill_dict[ID].after_delay > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].after_delay));
        }

        Debug.Log("SmashDownEnd");
        if (sk_manager.skill_dict[ID].cool_time > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].cool_time));
        }

        monster.monster_now_stat.speed = stat.speed;
    }
    private IEnumerator PerformChainAttack(string ID) // 사슬떨어지기
    {
        monster.monster_now_stat.speed = 0;
        if (sk_manager.skill_dict[ID].before_delay > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].before_delay));
        }
        Debug.Log("ChainAttackStart");

        sk_manager.use_skill(ID, gameObject);

        if (sk_manager.skill_dict[ID].after_delay > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].after_delay));
        }

        Debug.Log("ChainAttackEnd");
        if (sk_manager.skill_dict[ID].cool_time > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].cool_time));
        }

        monster.monster_now_stat.speed = stat.speed;
    }
    private IEnumerator PerformScytheAttack(string ID) // 낫 던지기
    {
        monster.monster_now_stat.speed = 0;
        if (sk_manager.skill_dict[ID].before_delay > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].before_delay));
        }
        Debug.Log("ScytheAttackStart");

        sk_manager.use_skill(ID, gameObject);

        if (sk_manager.skill_dict[ID].after_delay > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].after_delay));
        }

        Debug.Log("ScytheAttackEnd");
        if (sk_manager.skill_dict[ID].cool_time > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].cool_time));
        }

        monster.monster_now_stat.speed = stat.speed;
    }

}
