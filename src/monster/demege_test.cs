using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class demege_test : MonoBehaviour
{
    public bool hit = false;
    public stats.stat mon_stat;
    public skills_manager sk_manager;
    // public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        // target = FindObjectOfType("Player").gameObject.GetComponent<skills_manager>();
    }

    // Update is called once per frame
    void Update()
    {
        // mon_stat.move_D = transform.position - target.transform.position;

        // Debug.Log("moveD : "+mon_stat.move_D);
        if(mon_stat.hp < 50){
            // sk_manager.use_skill(7,gameObject);
        }
    }
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "sword"){
            sword_aura test = other.GetComponent<sword_aura>();
            if(test!=null){
                Debug.Log("damage : " + test.skill_op.demege);
                mon_stat.hp -= test.skill_op.demege;
            }
        }
    }
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Player"){
            dash test = other.gameObject.GetComponent<dash>();
            if(test!=null && test.skill_op.demege > 0){
                Debug.Log("damage : " + test.skill_op.demege);
                // mon_stat.hp -= test.skill_op.demege;
            }
        }
    }
}
