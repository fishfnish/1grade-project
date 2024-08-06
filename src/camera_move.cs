using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_move : MonoBehaviour
{
    // Start is called before the first frame updatepublic GameObject Target;               // 카메라가 따라다닐 타겟
    public GameObject Target;   

    public float[] offset = new float[3]{0.0f,0.0f,0.0f};
    public float[] agl_offset = new float[3]{0.0f,0.0f,0.0f};
    public bool track;

    public float CameraSpeed = 10.0f;       // 카메라의 속도
    Vector3 TargetPos;                      // 타겟의 위치


    void Awake(){
        transform.position = new Vector3(Target.transform.position.x + offset[0],
            Target.transform.position.y + offset[1],
            Target.transform.position.z + offset[2]);
    }

    void LateUpdate()
    {
        // 타겟의 x, y, z 좌표에 카메라의 좌표를 더하여 카메라의 위치를 결정
        TargetPos = new Vector3(
            Target.transform.position.x + offset[0],
            Target.transform.position.y + offset[1],
            Target.transform.position.z + offset[2]
            );

        // 카메라의 움직임을 부드럽게 하는 함수(Lerp)
        transform.position = Vector3.Slerp(transform.position, TargetPos, CameraSpeed * Time.deltaTime);
        if(track)transform.LookAt(Target.transform);
        else transform.rotation = Quaternion.Euler(agl_offset[0], agl_offset[1], agl_offset[2]);
    }
}
