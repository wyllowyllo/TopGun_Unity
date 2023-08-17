using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameManager : MonoBehaviour
{
    public GameObject[] enemyObjs;
    public Transform[] spawnPoints;

    public float curSpawnDelay;
    public float maxSpawnDelay;

    private void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if (curSpawnDelay >= maxSpawnDelay)
        {
            SpawnEnemy();
            maxSpawnDelay = UnityEngine.Random.Range(0.5f,3f);
            curSpawnDelay = 0;
        }

    }

    private void SpawnEnemy()
    {
        int ranEnemy= UnityEngine.Random.Range(0, 3);
        int ranPos = UnityEngine.Random.Range(0, 5);

        Instantiate(enemyObjs[ranEnemy], spawnPoints[ranPos].position, spawnPoints[ranPos].rotation);
    }
}
