using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_move : MonoBehaviour
{
    // Start is called before the first frame updatepublic GameObject Target;               // 카메라가 따라다닐 타겟
    public GameObject Target;
    public LayerMask obstructionLayer;  // 장애물이 될 레이어

    private Renderer[] previousObstructions;  // 이전 프레임의 장애물들
    private Material[] originalMaterials;  // 원래의 재질

    public float[] offset = new float[3] { 0.0f, 0.0f, 0.0f };
    public float[] agl_offset = new float[3] { 0.0f, 0.0f, 0.0f };
    public bool track;

    public float CameraSpeed = 10.0f;       // 카메라의 속도
    Vector3 TargetPos;                      // 타겟의 위치


    void Awake()
    {
        transform.position = new Vector3(Target.transform.position.x + offset[0],
            Target.transform.position.y + offset[1],
            Target.transform.position.z + offset[2]);
    }

    void LateUpdate()
    {
        HandleObstructions();
        // 타겟의 x, y, z 좌표에 카메라의 좌표를 더하여 카메라의 위치를 결정
        TargetPos = new Vector3(
            Target.transform.position.x + offset[0],
            Target.transform.position.y + offset[1],
            Target.transform.position.z + offset[2]
            );

        // 카메라의 움직임을 부드럽게 하는 함수(Lerp)
        transform.position = Vector3.Slerp(transform.position, TargetPos, CameraSpeed * Time.deltaTime);
        if (track) transform.LookAt(Target.transform);
        else transform.rotation = Quaternion.Euler(agl_offset[0], agl_offset[1], agl_offset[2]);
    }
    void HandleObstructions()
    {
        // 카메라와 플레이어 사이의 방향 계산
        Vector3 directionToPlayer = Target.transform.position - transform.position;

        // 레이캐스트로 카메라와 플레이어 사이의 장애물 감지
        Ray ray = new Ray(transform.position, directionToPlayer);
        RaycastHit[] hits = Physics.RaycastAll(ray, directionToPlayer.magnitude, obstructionLayer);

        // 이전에 투명하게 만든 장애물들을 원상태로 복원
        if (previousObstructions != null)
        {
            for (int i = 0; i < previousObstructions.Length; i++)
            {
                if (previousObstructions[i] != null)
                {
                    previousObstructions[i].material = originalMaterials[i];
                }
            }
        }

        // 새롭게 감지된 장애물 처리
        if (hits.Length > 0)
        {
            previousObstructions = new Renderer[hits.Length];
            originalMaterials = new Material[hits.Length];

            for (int i = 0; i < hits.Length; i++)
            {
                Renderer renderer = hits[i].transform.GetComponent<Renderer>();
                if (renderer != null)
                {
                    previousObstructions[i] = renderer;
                    originalMaterials[i] = renderer.material;

                    // 오브젝트를 투명하게 만들기 위해 임시 재질 설정
                    Material transparentMaterial = new Material(renderer.material);
                    transparentMaterial.color = new Color(
                        renderer.material.color.r,
                        renderer.material.color.g,
                        renderer.material.color.b,
                        0.3f);  // 투명도 설정

                    renderer.material = transparentMaterial;
                }
            }
        }
    }

}
