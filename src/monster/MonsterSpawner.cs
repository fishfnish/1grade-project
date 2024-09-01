using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using UnityEngine.AI;
using Unity.Collections;
// using System.Numerics;

public class MonsterSpawner : MonoBehaviour
{
    [Serializable]
    public struct spawn_monster
    {
        public GameObject monsterObj;
        public GameObject spawnCircle; // 마법진
        public Rigidbody rigidbody;
        public monster monster;
        public Vector3 spawnPosition;
        public float originalSpeed;
        // public float originalDistance;
    };

    [Serializable]
    public struct waves
    {
        public List<spawn_monster> spawn;
    }

    public List<waves> wave = new List<waves>();

    private Transform[] childTransforms;
    private int spawnCircleSequence; // 소환 서클 오브젝트 순서
    private int waveIndex; // 현재 웨이브 인덱스
    private int monstersRemainingInWave;  // 현재 웨이브에 남아 있는 몬스터 수
    public float monsterDownSize; // 몬스터 땅에 묻기 사이즈
    public float monsterUpSize; // 몬스터 땅에 띄우기 사이즈
    public float spawnCircleLifeTime; // 마법진 라이프 타임
    public float sizeControl;

    void Start()
    {
        monstersRemainingInWave = 0;
        waveIndex = 0;
        spawnCircleSequence = 0;
        childTransforms = GetComponentsInChildren<Transform>();
        SpecifyCircles();
    }
    void Update()
    {
        if (monstersRemainingInWave <= 0)
        {
            if (waveIndex < wave.Count)
            {
                spawnWave();
                waveIndex++;
                return;
            }
            if (waveIndex >= wave.Count)
            {
                StartCoroutine(spawnDestroy());
            }
        }
        else
        {
            for (int i = 0; i < wave.Count; i++)
            {
                for (int j = 0; j < wave[i].spawn.Count; j++)
                {
                    if (wave[i].spawn[j].monsterObj.activeSelf == false)
                    {
                        monstersRemainingInWave--;
                        return;
                    }
                }
            }
        }
    }
    private void spawnWave()
    {
        monstersRemainingInWave = wave[waveIndex].spawn.Count;
        // Debug.Log(waveIndex);
        for (int i = 0; i < wave[waveIndex].spawn.Count; i++)
        {
            StartCoroutine(SpawnMonsters(waveIndex, i));
        }
    }

    private IEnumerator SpawnMonsters(int waveIndex, int monsterIndex)
    {
        spawn_monster tempSpawn = wave[waveIndex].spawn[monsterIndex];
        tempSpawn.spawnCircle.SetActive(true);
        Vector3 randomPosition = radomSize(tempSpawn.spawnCircle.transform.position, tempSpawn.spawnCircle.transform.localScale);

        tempSpawn.monsterObj = Instantiate(
            tempSpawn.monsterObj,
            randomPosition,
            Quaternion.identity);

        tempSpawn.monster = tempSpawn.monsterObj.GetComponent<monster>();
        tempSpawn.rigidbody = tempSpawn.monster.GetComponent<Rigidbody>();
        tempSpawn.rigidbody.useGravity = false;
        tempSpawn.originalSpeed = tempSpawn.monster.monster_now_stat.speed;
        // tempSpawn.originalDistance = tempSpawn.monster.distance;
        tempSpawn.monster.monster_now_stat.speed = 0; // 스폰 시 일시적으로 속도 0
        // tempSpawn.monster.distance = 0;

        wave[waveIndex].spawn[monsterIndex] = tempSpawn;

        // // 몬스터를 땅 속에 스폰
        tempSpawn.monsterObj.transform.position = new Vector3(
            tempSpawn.monsterObj.transform.position.x,
            tempSpawn.spawnCircle.transform.position.y - tempSpawn.monsterObj.transform.position.y + monsterDownSize,
            tempSpawn.monsterObj.transform.position.z);

        // 몬스터를 천천히 위로 올림
        Vector3 targetPosition = randomPosition + new Vector3(0f, monsterUpSize, 0f);
        float elapsedTime = 0f;

        while (elapsedTime < spawnCircleLifeTime)
        {
            tempSpawn.monsterObj.transform.position = Vector3.Lerp(
                tempSpawn.monsterObj.transform.position,
                targetPosition,
                elapsedTime / spawnCircleLifeTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return null;
        Destroy(tempSpawn.spawnCircle);
        tempSpawn.monster.monster_now_stat.speed = tempSpawn.originalSpeed; // 원래 속도로 복원
        tempSpawn.rigidbody.useGravity = true;
        // tempSpawn.monster.distance = tempSpawn.originalDistance;
    }
    private IEnumerator spawnDestroy()
    {
        yield return new WaitForSeconds(spawnCircleLifeTime + 1f);
        Destroy(gameObject);
    }
    private void SpecifyCircles() // 마법진 지정
    {
        for (int i = 0; i < wave.Count; i++)
        {
            for (int j = 0; j < wave[i].spawn.Count; j++)
            {
                spawn_monster tempSpawn = wave[i].spawn[j];
                if (spawnCircleSequence < childTransforms.Length - 1)
                {
                    tempSpawn.spawnCircle = childTransforms[spawnCircleSequence + 1].gameObject;
                }
                tempSpawn.spawnCircle.SetActive(false);
                wave[i].spawn[j] = tempSpawn;
            }
            spawnCircleSequence++;
        }
    }
    private Vector3 radomSize(Vector3 size, Vector3 scale)
    {
        float x = UnityEngine.Random.Range(size.x - (scale.x - sizeControl)/2, size.x + (scale.x - sizeControl)/2);
        float y = size.y;
        float z = UnityEngine.Random.Range(size.z - (scale.z - sizeControl)/2, size.z + (scale.z - sizeControl)/2);
        // Debug.Log(size);
        // Debug.Log(x);
        // Debug.Log(z);
        size = new Vector3(x, y, z);
        return size;
    }
}
