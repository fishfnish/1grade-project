using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class MagicMonster : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip bulletInstantiateClip;// 마법 생성 음향
    public AudioClip bulletFireClip;// 마법 발사 음향
    public monster monster;
    public Animator anim;
    public skills_manager sk_manager;
    public float bulletRange;
    public bool can;

    public List<string> skill_ID = new List<string>();

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        monster = GetComponent<monster>();
        anim = GetComponent<Animator>();
        sk_manager = monster.sk_manager;
        monster.monster_now_stat.is_skill = true;
        can = true;
    }

    void Update()
    {
        if (monster.damaged)
        {
            StartCoroutine(TelePort(skill_ID[1])); // 005
        }
        else if (can)
        {
            anim.SetBool("Is_run", true);
            if (bulletRange >= monster.distance)
            {                
                StartCoroutine(CastSpell(skill_ID[0])); // 003
            }
        }

    }

    private IEnumerator CastSpell(string ID)
    {
        can = false;
        monster.monster_now_stat.speed = 0;
        
        // monster.monster_now_stat.is_skill = true;
        // monster.changeSoundClip(bulletInstantiateClip, audioSource);     
        anim.SetBool("Is_run", false);
        anim.SetBool("Is_casting",true);
        if (sk_manager.skill_dict[ID].before_delay > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].before_delay));
        }

        Debug.Log("instantiate");
        sk_manager.use_skill(ID, gameObject);
        // audioSource.PlayOneShot(bulletFireClip);
        //monster.changeSoundClip(bulletFireClip, audioSource);
        if (sk_manager.skill_dict[ID].life_time > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].life_time));
        }
        if (sk_manager.skill_dict[ID].after_delay > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].after_delay));
        }

        // monster.monster_now_stat.is_skill = false;
        if (sk_manager.skill_dict[ID].cool_time > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].cool_time));
        }
        monster.monster_now_stat.speed = monster.originalSpeed;

        anim.SetBool("Is_casting", false);
        can = true;
    }
    private IEnumerator TelePort(string ID)
    {
        if (sk_manager.skill_dict[ID].before_delay > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].before_delay));
        }

        // monster.monster_now_stat.is_skill = true;
        Debug.Log("teleport");
        sk_manager.use_skill(ID, gameObject);

        if (sk_manager.skill_dict[ID].life_time > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].life_time));
        }
        if (sk_manager.skill_dict[ID].after_delay > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].after_delay));
        }
        // monster.monster_now_stat.is_skill = false;
        if (sk_manager.skill_dict[ID].cool_time > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].cool_time));
        }
    }
}