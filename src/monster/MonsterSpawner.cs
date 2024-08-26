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
        public GameObject monsterInstance;
        public GameObject spawnCircle; // 마법진
        public GameObject monsterObj;
        public monster monster;
        public Vector3 spawnPosition;
        public float spawn_delay; // 스폰 간격
        public float originalSpeed;
        // public int spawnCount;
        // public bool isSpawn;
    };
    [Serializable]
    public struct waves
    {
        public List<spawn_monster> spawn;
        // public float delay; // 다음 웨이브 까지의 시간
    }
    public List<waves> wave = new List<waves>();

    private Transform[] childTransforms;
    private int spawnCircleSequence; // 소환 서클오브젝트 순서 맞추기
    private int waveIndex;
    // public int nextWave; // 다음 웨이브 까지 남은 몬스터
    public int monstersRemainingInWave;  // 현재 웨이브에 남아 있는 몬스터 수
    public float monsterYSize; // 몬스터 크기 맞추기용
    public float spawnCircleLifeTime; // 마법진 라이프 타임

    void Start()
    {
        monstersRemainingInWave = 0;
        spawnCircleSequence = 0;
        waveIndex = 0;
    }
    void Update()
    {
        if (waveIndex >= wave.Count && monstersRemainingInWave <= 0)
        {
            StartCoroutine(spawnDestroy());
            return;
        }
        if (waveIndex < wave.Count)
        {
            // if (wave[waveIndex].delay > 0)
            // {
            //     StartCoroutine(spawnDelay(wave[waveIndex].delay));
            // }
            spawnStart(waveIndex);
            waveIndex++;
        }
        for (int i = 0; i < wave.Count; i++)
        {
            for (int j = 0; j < wave[i].spawn.Count; j++)
            {
                if (wave[i].spawn[j].monsterInstance == null)
                {
                    monstersRemainingInWave--;
                }
            }
        }
    }
    void spawnStart(int waveCount)
    {
        // numberOfMonsters = 0;
        childTransforms = GetComponentsInChildren<Transform>();
        spawnCircleSequence = childTransforms.Length - 1;
        monstersRemainingInWave = wave[waveCount].spawn.Count;

        for (int i = 0; i < wave[waveCount].spawn.Count; i++)
        {
            spawn_monster tempSpawn = wave[waveCount].spawn[i];
            if (i < spawnCircleSequence)
            {
                tempSpawn.spawnCircle = childTransforms[i + 1].gameObject;
            }
            wave[waveCount].spawn[i] = tempSpawn;
            StartCoroutine(SpawnMonsters(waveCount, i));
        }
    }
    private IEnumerator SpawnMonsters(int index, int index2)
    {
        spawn_monster tempSpawn = wave[index].spawn[index2];

        tempSpawn.monsterInstance = Instantiate
            (tempSpawn.monsterObj,
            tempSpawn.spawnCircle.transform.position,
            Quaternion.identity);

        tempSpawn.monster = tempSpawn.monsterInstance.GetComponent<monster>();
        tempSpawn.originalSpeed = tempSpawn.monster.monster_now_stat.speed;

        tempSpawn.monster.monster_now_stat.speed = 0;
        wave[index].spawn[index2] = tempSpawn;

        tempSpawn.monsterInstance.transform.position = new Vector3 // 몬스터 땅속에 스폰
        (tempSpawn.monsterInstance.transform.position.x,
        tempSpawn.spawnCircle.transform.position.y - tempSpawn.monsterInstance.transform.position.y + monsterYSize,
        tempSpawn.monsterInstance.transform.position.z);

        Vector3 targetPosition = tempSpawn.spawnCircle.transform.position + new Vector3(0f, monsterYSize, 0f);

        float elapsedTime = 0f;
        while (elapsedTime < spawnCircleLifeTime) // 몬스터 띄우기
        {
            tempSpawn.monsterInstance.transform.position = Vector3.Lerp(
                tempSpawn.monsterInstance.transform.position,
                targetPosition,
                elapsedTime / spawnCircleLifeTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        tempSpawn.spawnCircle.SetActive(false);
        // tempSpawn.monsterInstance.transform.position = targetPosition;

        tempSpawn.monster.monster_now_stat.speed = tempSpawn.originalSpeed;
    }
    private IEnumerator spawnDestroy()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
    private void destroyCheck(GameObject instance)
    {
        if (!instance.activeSelf)
        {
            monstersRemainingInWave--;
        }
    }
    // private IEnumerator spawnDelay(float delay)
    // {
    //     yield return new WaitForSeconds(delay);

    //     spawnStart(waveIndex);
    //     waveIndex++;
    // }
}
