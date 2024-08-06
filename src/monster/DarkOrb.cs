using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkOrb : MonoBehaviour
{
    public float speed = 10f;

    public Vector3 target;

    public void Initialize(Vector3 targetPosition)
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<Vector3>();
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            HitTarget();
        }
    }

    void HitTarget()
    {
        Destroy(gameObject);
    }
}