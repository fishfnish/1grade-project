using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class MagicMonster : MonoBehaviour
{
    public stats.stat stat;
    public monster monster;
    public skills_manager sk_manager;

    public List<string> skill_ID = new List<string>();

    void Start()
    {
        monster = GetComponent<monster>();
        sk_manager = monster.sk_manager;

        monster.monster_now_stat = stat;
        monster.maxhp = stat.hp;
    }

    void Update()
    {
        if (monster.range >= monster.distance )
        {
            StartCoroutine(CastSpell(skill_ID[0])); // 003
            if (monster.damaged)
            {
                StartCoroutine(TelePort(skill_ID[1])); // 005
            }
        }
    }

    IEnumerator CastSpell(string ID)
    {
        if (sk_manager.skill_dict[ID].before_delay > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].before_delay));
        }
        
        Debug.Log("instantiate");
        sk_manager.use_skill(ID, gameObject);

        if (sk_manager.skill_dict[ID].after_delay > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].after_delay));
        }
        if (sk_manager.skill_dict[ID].cool_time > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].cool_time));
        }
    }
    IEnumerator TelePort(string ID)
    {
        if (sk_manager.skill_dict[ID].before_delay > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].before_delay));
        }
        
        Debug.Log("teleport");
        sk_manager.use_skill(ID, gameObject);

        if (sk_manager.skill_dict[ID].after_delay > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].after_delay));
        }
        if (sk_manager.skill_dict[ID].cool_time > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].cool_time));
        }
    }
}