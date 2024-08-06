using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class dash : MonoBehaviour
{
    public Vector3 TeleportAreaCenter;
    public Vector3 TeleportAreaSize;
    public Transform pc;
    public skills_manager.skill skill_op;
    public stats.stat stat;
    public int dash_type;   
    public LayerMask obstacleLayer;
    private monster monster;
    private player player;
    public bool is_boder;
    // Start is called before the first frame update
    void Start()
    {
        if(dash_type == 0) Destroy(this,skill_op.life_time);
        if(dash_type == 1) Destroy(this,skill_op.life_time);
        if(gameObject.tag == "Player"){
            player = gameObject.GetComponent<player>();
            stat = player.player_stat;
            if(stat.is_skill){
                stat.move_D = (player.get_mouse_pos(true)-transform.position).normalized;
            }
        }
        if (gameObject.tag == "monster"){
            monster = gameObject.GetComponent<monster>();
            stat = monster.monster_now_stat;
        }
    }

    // Update is called once per frame
    private void FixedUpdate() {
        StopToWall();
    }
    void Update()
    {
        if(dash_type == 0) dsah();
        else if(dash_type == 1) Teleport();
    }
    public void dsah(){
        if(is_boder)Destroy(this);
        if(!is_boder)transform.Translate(stat.move_D * skill_op.speed * Time.deltaTime, Space.World);
    }
    void StopToWall()
    {
        List<Vector3> ray_pos = new List<Vector3>();
        ray_pos.Add(transform.position + Vector3.up * 0.1f);
        ray_pos.Add(transform.position + Vector3.up * 1.5f);
        ray_pos.Add(transform.position + Vector3.up * 3f);
        foreach(Vector3 pos in ray_pos) {
            Debug.DrawRay(pos, transform.forward * 5, Color.green);
            if(Physics.Raycast(pos,transform.forward,out RaycastHit hit, 1.5f)){
                if(hit.collider.CompareTag("Wall")) {
                    is_boder = true;
                }
            }
            else is_boder = false;
        }
    }
    public void Teleport()
    {
        Vector3 newPosition = GetTeleportPosition();
        if (newPosition != Vector3.zero)
        {
            transform.position = newPosition;
        }
    }

    private Vector3 GetTeleportPosition()
    {
        float currentDistanceToPc = Vector3.Distance(transform.position, pc.position);
        Debug.Log(currentDistanceToPc);

        for (int i = 0; i < 10; i++)
        {
            Vector3 randomPosition = GetRandomPositionInArea();
            if (IsValidTeleportPosition(randomPosition) && Vector3.Distance(randomPosition, pc.position) >= currentDistanceToPc)
            {
                return randomPosition;
            }
        }
        return Vector3.zero;
    }

    private Vector3 GetRandomPositionInArea()
    {
        float x = Random.Range(-TeleportAreaSize.x / 2, TeleportAreaSize.x / 2);
        float y = Random.Range(-TeleportAreaSize.y / 2, TeleportAreaSize.y / 2);
        float z = Random.Range(-TeleportAreaSize.z / 2, TeleportAreaSize.z / 2);

        return TeleportAreaCenter + new Vector3(x, y, z);
    }

    private bool IsValidTeleportPosition(Vector3 position)
    {
        if (Physics.CheckSphere(position, 1f, obstacleLayer))
        {
            return false;
        }

        return true;
    }
}
