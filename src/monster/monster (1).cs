using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using static skills_manager;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices.WindowsRuntime;

public class monster : MonoBehaviour
{
    public AudioClip walkClip;
    public AudioSource audioSource;
    public stats.stat monster_now_stat;
    public skills_manager sk_manager;
    public GameObject target;
    public player player;

    public float maxHp;
    public float range = 0;
    public float distance = 0;
    public float pitch;
    public float stun;
    public float monsterDamaged; // 데미지 받기
    public float originalSpeed;

    private bool die = false;
    public bool damaged = false;
    public bool damaged2 = false; // 텍스트 띄우기용 
    public bool isSword;
    public bool audioPlayed;

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

    void Awake()
    {
        audioPlayed = false;
        maxHp = monster_now_stat.hp;
        originalSpeed = monster_now_stat.speed; // 스피드 저장

        audioSource = GetComponent<AudioSource>();
        target = GameObject.FindGameObjectWithTag("Player");
        cam = Camera.main.transform;
        damageText = GetComponentInChildren<TextMeshProUGUI>();
        MonHpSlider = GetComponentInChildren<Slider>();
        MonCan = GetComponentInChildren<Canvas>();
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
        MonHpSlider.value = monster_now_stat.hp / maxHp;
        damageText.text = $"{monsterDamaged}";

        if (damaged2)
        {
            DisplayDamageText();
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
        if (monster_now_stat.is_skill == false && monster_now_stat.is_dash == false)
        {
            changeSoundClip(walkClip, audioSource);
            audioSource.pitch = pitch;
        }
        else 
        {
            audioSource.pitch = 1f;
        }
        Quaternion targetrotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetrotation, monster_now_stat.rot_speed * Time.deltaTime);

        transform.position += direction * monster_now_stat.speed * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bullet") || other.CompareTag("sword"))
        {
            if (other.CompareTag("sword"))
            {
                isSword = true;
            }
            HandleDamage();
        }
    }
    private void HandleDamage()
    {
        damaged2 = true;
        damageText.fontSize = startFontSize;
        elapsedTime = 0;
        damaged = true;

        monster_now_stat.hp -= monsterDamaged;
        Debug.Log($"받은 데미지: {monsterDamaged}");
        if (gameObject.tag != "boss" && isSword)
        {
            StartCoroutine(attackStun());
            StartCoroutine(RedEffect());
        }
    }
    void DisplayDamageText() // 데미지 띄우기
    {

        if (elapsedTime > shrinkDuration)
        {
            elapsedTime = 0;
            damageText.fontSize = 0;
            damaged2 = false;
        }
        else
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / shrinkDuration;
            damageText.fontSize = Mathf.Lerp(startFontSize, endFontSize, t);
        }
    }
    private IEnumerator attackStun() // 공격 스턴
    {
        monster_now_stat.speed = 0;
        yield return new WaitForSeconds(stun);
        damaged = false;
        monster_now_stat.speed = originalSpeed;
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
            for (float i = delay; i > 0; i -= 0.1f)
            {
                // Debug.Log("delta : " + i);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
    public void changeSoundClip(AudioClip audioClip, AudioSource audioSource)
    {
        // 오디오가 아직 재생되지 않았을 경우
        if (!audioPlayed)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
            audioPlayed = true;  // 오디오가 재생되었음을 표시
        }
        // 오디오가 재생이 끝났을 경우
        else if (!audioSource.isPlaying)
        {
            // 오디오 재생 상태를 초기화
            audioPlayed = false;
        }
    }
}