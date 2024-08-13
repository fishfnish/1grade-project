using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class MagicMonster : MonoBehaviour
{
    public monster monster;
    public skills_manager sk_manager;
    public AudioSource audioSource;
    public AudioClip bulletInstantiateClip;// 마법 생성 음향
    public AudioClip bulletFireClip;// 마법 발사 음향
    public float bulletRange;

    public List<string> skill_ID = new List<string>();

    void Start()
    {
        monster = GetComponent<monster>();
        audioSource = GetComponent<AudioSource>();
        sk_manager = monster.sk_manager;
    }

    void Update()
    {
        if (monster.damaged)
        {
            TelePort(skill_ID[1]); // 005
        }
        else if (bulletRange >= monster.distance)
        {

            CastSpell(skill_ID[0]); // 003
        }
    }

    void CastSpell(string ID)
    {
        monster.monster_now_stat.is_skill = true;
        // StartCoroutine(monster.changeSoundClip(bulletInstantiateClip, audioSource, false));
        if (sk_manager.skill_dict[ID].before_delay > 0)
        {
            monster.WaitForDelay(sk_manager.skill_dict[ID].before_delay);
        }

        Debug.Log("instantiate");
        sk_manager.use_skill(ID, gameObject);
        // audioSource.clip = bulletFireClip;
        // monster.changeSoundClip(bulletFireClip, audioSource, false);

        if (sk_manager.skill_dict[ID].life_time > 0)
        {
            monster.WaitForDelay(sk_manager.skill_dict[ID].life_time);
        }
        if (sk_manager.skill_dict[ID].after_delay > 0)
        {
            monster.WaitForDelay(sk_manager.skill_dict[ID].after_delay);
        }

        Debug.Log("after : " + Time.deltaTime);
        monster.monster_now_stat.is_skill = false;
        if (sk_manager.skill_dict[ID].cool_time > 0)
        {
            monster.WaitForDelay(sk_manager.skill_dict[ID].cool_time);
        }
    }
    void TelePort(string ID)
    {
        if (sk_manager.skill_dict[ID].before_delay > 0)
        {
            monster.WaitForDelay(sk_manager.skill_dict[ID].before_delay);
        }

        monster.monster_now_stat.is_skill = true;
        Debug.Log("teleport");
        sk_manager.use_skill(ID, gameObject);

        if (sk_manager.skill_dict[ID].after_delay > 0)
        {
            monster.WaitForDelay(sk_manager.skill_dict[ID].after_delay);
        }
        if (sk_manager.skill_dict[ID].life_time > 0)
        {
            monster.WaitForDelay(sk_manager.skill_dict[ID].life_time);
        }
        monster.monster_now_stat.is_skill = false;
        if (sk_manager.skill_dict[ID].cool_time > 0)
        {
            monster.WaitForDelay(sk_manager.skill_dict[ID].cool_time);
        }
    }
}