using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using static skills_manager;
using UnityEngine.UI;
using TMPro;

public class monster : MonoBehaviour
{
    // test
    public stats.stat monster_now_stat;
    public skills_manager sk_manager;
    public GameObject target;
    public player player;
    public float maxhp;
    public float range = 0;
    public float distance = 0;
    public float stun;
    public float originalSpeed;

    private bool die = false;
    public bool damaged = false;
    public bool isSword;

    public Transform cam;
    private float elapsedTime;

    // 몬스터 데미지 텍스트
    public TextMeshProUGUI damageText; // 데미지를 표시할 Text 컴포넌트
    public float shrinkDuration; // 텍스트가 줄어드는 시간
    public int startFontSize; // 시작 폰트 크기
    public int endFontSize;
    // 몬스터 체력 UI
    public Slider MonHpSlider;
    public Canvas MonCan;

    public float testDamage; // 테스트 용

    void Awake()
    {
        originalSpeed = monster_now_stat.speed; // 스피드 저장
        target = GameObject.FindGameObjectWithTag("Player");
        cam = Camera.main.transform;

        damageText.fontSize = 0; // 초기 폰트 크기 설정

        if (sk_manager == null)
        {
            GameObject game_manager = GameObject.Find("gamemanager");
            sk_manager = game_manager.GetComponent<skills_manager>();
        }
        player = target.GetComponent<player>();
    }

    void Update()
    {
        MonHpSlider.value = monster_now_stat.hp / maxhp;
        damageText.text = $"{player.player_stat.demege}";


        if (damaged)
        {
            damageDisplay();
        }

        // 카메라 따라가기
        MonCan.transform.LookAt(cam);
        // damageText.transform.LookAt(cam);
        // transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up

        // 죽음
        if (monster_now_stat.hp <= 0)
        {
            Destroy(gameObject);
            die = true;
        }
        Vector3 direction = target.transform.position - transform.position;
        direction.y = 0;
        distance = direction.magnitude;
        direction = Vector3.Normalize(direction);
        monster_now_stat.move_D = direction;
        // Debug.Log("Direction: " + direction + " | Distance: " + distance);
        if (range >= distance)
        {

            Quaternion targetrotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetrotation, monster_now_stat.rot_speed * Time.deltaTime);

            transform.position += direction * monster_now_stat.speed * Time.deltaTime;
            // Debug.Log(monster_now_stat.move_D);            
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bullet") || other.CompareTag("sword"))
        {
            HandleDamage();
            if (other.CompareTag("sword"))
            {
                isSword = true;
            }
        }
    }
    private void HandleDamage()
    {
        damaged = true;
        monster_now_stat.hp -= player.player_stat.demege;
        Debug.Log($"받은 데미지: {player.player_stat.demege}");
        if (gameObject.tag != "boss")
        {
            StartCoroutine(attackStun());
        }
        StartCoroutine(RedEffect());
    }
    void damageDisplay() // 데미지 띄우기
    {
        if (elapsedTime > shrinkDuration)
        {
            elapsedTime = 0;
            damageText.fontSize = 0;
        }
        else
        {
            elapsedTime += Time.deltaTime;
            Debug.Log(elapsedTime);
            float t = elapsedTime / shrinkDuration;
            damageText.fontSize = Mathf.Lerp(startFontSize, endFontSize, t);
        }
    }
    private IEnumerator attackStun() // 공격 스턴
    {
        monster_now_stat.speed = 0;
        yield return new WaitForSeconds(stun);
        monster_now_stat.speed = originalSpeed;
        damaged = false;
    }
    public IEnumerator RedEffect()
    {
        Renderer renderer = GetComponent<Renderer>();

        Color originalColor = renderer.material.color;
        renderer.material.color = Color.red;

        yield return new WaitForSeconds(stun);

        renderer.material.color = originalColor;
    }
    public IEnumerator WaitForDelay(float delay)
    {
        if (delay > 0)
        {
            // Debug.Log("time : " + Time.deltaTime);
            for (float i = delay; i > 0; i -= Time.deltaTime)
                yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}