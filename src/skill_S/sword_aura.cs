using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class sword_aura : skills_manager
{
    // Start is called before the first frame update
    
    public skill skill_op;
    public int skill_index;

    private Rigidbody rigid;
    void Start()
    {
        Destroy(this, skill_op.life_time);
        if(skill_op.skills_obj != null){
            Destroy(gameObject, skill_op.life_time);
            gameObject.transform.localScale = skill_op.skills_size;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
