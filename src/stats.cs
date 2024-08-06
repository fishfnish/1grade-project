using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class stats : MonoBehaviour
{
    // Start is called before the first frame update
    [Serializable] 
    public struct stat{
        public float demege;
        public float speed;
        public float rot_speed;
        public float hp;
        public bool is_skill;
        public bool is_dash;
        public bool is_sturn;
        public bool attect_situation;
        public int move_situation;
        public Vector3 move_D;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
