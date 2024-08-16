using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using System;



public class player : MonoBehaviour
{
    [Header("player now_stat")]
    public soundManager soundManager;
    public AudioSource audioSource;
    public AudioClip walkClip;
    public stats.stat player_stat;
    public bool mouse_ck = false;
    Rigidbody rig;
    Quaternion rotation;
    quaternion move_buffer;
    Vector2 input;
    public bool is_boder;
    public bool audioPlayed = false;
    public float max_HP = 100;
    private float speed2;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rig = GetComponent<Rigidbody>();
        player_stat.hp = max_HP;
        speed2 = player_stat.speed;
        player_stat.is_sturn = false;
    }

    private void FixedUpdate()
    {
        StopToWall();
    }

    void Update()
    {
        // Debug.Log(Time.timeScale);
        bool cal = (player_stat.move_D != Vector3.zero);

        if (!player_stat.is_skill) mouse_ck = false;
        if ((cal || player_stat.is_skill) && !player_stat.is_sturn)
        {
            if (!player_stat.is_skill)
            {
                if (player_stat.speed > speed2) player_stat.move_situation = -2;
                else player_stat.move_situation = -3;
            }
            if (player_stat.is_skill)
            {
                move_rot();
            }
            else
            {
                player_stat.move_D = new Vector3(input.x, 0.0f, input.y);
                rotation = Quaternion.LookRotation(player_stat.move_D);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, player_stat.rot_speed * Time.deltaTime);
                if (!is_boder) transform.Translate(Vector3.forward * player_stat.speed * Time.deltaTime);
                // move_rot();
            }
        }
        else if (player_stat.is_sturn)
        {

        }
        else
        {
            player_stat.move_situation = 0;

        }
        if (!player_stat.is_skill && !player_stat.is_dash && player_stat.move_D != Vector3.zero)
        {
            // audioSource.time = 5f;
            soundManager.changeSoundClip(walkClip, audioSource);
        }
        else if (!player_stat.is_skill && !player_stat.is_dash && player_stat.move_D == Vector3.zero)
        {
            audioSource.clip = null;
        }
    }
    void StopToWall()
    {
        List<Vector3> ray_pos = new List<Vector3>();
        ray_pos.Add(transform.position + Vector3.up * 0.1f);
        ray_pos.Add(transform.position + Vector3.up * 1.5f);
        ray_pos.Add(transform.position + Vector3.up * 3f);
        foreach (Vector3 pos in ray_pos)
        {
            Debug.DrawRay(pos, transform.forward * 5, Color.green);
            if (Physics.Raycast(pos, transform.forward, out RaycastHit hit, 1.5f))
            {
                if (hit.collider.CompareTag("Wall")) is_boder = true;

            }
            else is_boder = false;
        }
    }
    public void move_rot()
    {
        // player_stat.move_D = new Vector3(input.x, 0.0f, input.y);
        // if(!mouse_ck)rotation = Quaternion.LookRotation(get_mouse_pos() - transform.position);
        if (!mouse_ck) transform.LookAt(get_mouse_pos(true));
        // StartCoroutine(rot_skills(rotation));
        // transform.Translate(Vector3.forward * player_stat.speed * Time.deltaTime);
    }
    public void on_Demege(float demege)
    {

    }

    public void Die()
    {
        gamemanager.GameOver = true;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
        // Debug.Log(input);]
        if (input != null && context.performed)
        {
            player_stat.move_D = new Vector3(input.x, 0.0f, input.y);
            rotation = Quaternion.LookRotation(player_stat.move_D);
        }
    }
    public void onRun(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            player_stat.speed *= 1.5f;
        }
        else
        {
            player_stat.speed = speed2;
        }
    }
    public Vector3 get_mouse_pos(bool target)
    {
        if (!player_stat.is_skill && !mouse_ck) mouse_ck = true;
        Vector3 input = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(input);
        // Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.blue, 5f);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 100f);
        Vector3 hit_pos = new Vector3(hit.point.x, target ? transform.position.y : hit.point.y, hit.point.z);
        return hit_pos;
        // transform.LookAt(hit_pos);
        // StartCoroutine(rot_skills(T_rot));
    }
    
}