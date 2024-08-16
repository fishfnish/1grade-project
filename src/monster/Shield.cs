using System.Collections;
using System.Collections.Generic;
// using System.Numerics;
using JetBrains.Annotations;
using TMPro;
// using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class Shield : MonoBehaviour
{
    public AudioClip attackClip;
    public AudioClip parryClip;
    public AudioClip dashClip;
    // public AudioClip dashingClip;
    public AudioSource audioSource;
    public monster monster;
    public player player;
    public dash dash;
    public skills_manager sk_manager;

    public bool[] can = new bool[2]; // 0. 방패치기 1. 돌진

    public bool isStun;
    public bool isKnockBack;
    public float shieldStun; // 패링 스턴
    public float damagePlus; // 데미지 배율
    public float damagePlusTime; // 데미지 배율 적용시간
    public float originalSpeed; // 오리지널 스피드
    public float originalStun; // 오리지널 스턴
    public float attackRange; // 방패치기 공격거리
    public float dashRange; // 대쉬 공격거리
    public Vector3 PTP; // player.transform.position
    public float knockBackDistance = 10f;
    public float knockBackSpeed = 10f;


    public List<string> skill_ID = new List<string>();

    // Start is called before the first frame update

    void Start()
    {
        isKnockBack = false;
        isStun = false;

        can[0] = true;
        can[1] = true;

        audioSource = GetComponent<AudioSource>();
        monster = GetComponent<monster>();
        player = monster.player;
        sk_manager = monster.sk_manager;

        originalStun = monster.stun;
        originalSpeed = monster.monster_now_stat.speed;
        monster.monster_now_stat.is_dash = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (monster != null)
        {
            if (isKnockBack)
            {
                Vector3 targetPosition = PTP + monster.monster_now_stat.move_D * knockBackDistance;
                Debug.Log("dd" + targetPosition);

                player.transform.position = Vector3.MoveTowards(player.transform.position, targetPosition, knockBackSpeed * Time.deltaTime);
                if (player.transform.position == targetPosition)
                {
                    isKnockBack = false;
                }
            }
            if (monster.monster_now_stat.is_dash && monster.isSword)
            {
                StartCoroutine(parryStun());
                monster.isSword = false;
            }
            else
            {
                if (can[1])
                {
                    if (dashRange >= monster.distance)
                    {
                        StartCoroutine(PerformDash(skill_ID[1]));
                    }
                }
                if (can[0])
                {
                    if (attackRange >= monster.distance)
                    {
                        StartCoroutine(PerformAttack(skill_ID[0]));
                    }
                }
            }
        }
    }
    private IEnumerator PerformAttack(string ID) // 방패치기
    {
        if (monster.monster_now_stat.is_dash)
        {
            yield break;
        }
        can[0] = false;

        monster.monster_now_stat.speed = 0;
        if (sk_manager.skill_dict[ID].before_delay > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].before_delay));
        }

        monster.monster_now_stat.is_skill = true;
        monster.changeSoundClip(attackClip, audioSource, false);
        sk_manager.use_skill(ID, gameObject);
        Debug.Log("attackStart");

        if (sk_manager.skill_dict[ID].life_time > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].life_time));
        }
        if (sk_manager.skill_dict[ID].after_delay > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].after_delay));
        }
        Debug.Log("attackEnd");
        monster.monster_now_stat.is_skill = false;
        monster.monster_now_stat.speed = originalSpeed;

        if (sk_manager.skill_dict[ID].cool_time > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].cool_time));
        }
        can[0] = true;
    }
    private IEnumerator PerformDash(string ID) // 대쉬 
    {
        can[1] = false;

        monster.monster_now_stat.speed = 0;
        Debug.Log("charging");

        monster.monster_now_stat.is_dash = true;
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

        // monster.changeSoundClip(dashingClip, audioSource,);
        if (sk_manager.skill_dict[ID].life_time > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].life_time));
        }
        if (sk_manager.skill_dict[ID].after_delay > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].after_delay));
        }

        monster.monster_now_stat.is_dash = false;
        monster.monster_now_stat.speed = originalSpeed;
        Debug.Log("dashEnd");

        if (sk_manager.skill_dict[ID].cool_time > 0)
        {
            yield return StartCoroutine(monster.WaitForDelay(sk_manager.skill_dict[ID].cool_time));
        }
        can[1] = true;
    }
    private IEnumerator parryStun() // 패링 스턴
    {
        monster.changeSoundClip(parryClip, audioSource, false);

        Destroy(dash);

        monster.monster_now_stat.is_dash = false;
        Debug.Log("dashEnd2");

        monster.stun = shieldStun;
        player.player_stat.demege = player.player_stat.demege * damagePlus;
        yield return new WaitForSeconds(damagePlusTime);

        monster.stun = originalStun;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (monster.monster_now_stat.is_dash)
        {
            Destroy(dash);
            if (other.gameObject.tag == "Player")
            {
                isKnockBack = true;
                PTP = other.transform.position;
                monster.changeSoundClip(dashClip, audioSource, false);
            }
        }
    }
}