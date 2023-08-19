using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public GameObject[] enemyObjs;
    public Transform[] spawnPoints;
    //public Text Score_text;
    //public Image[] Health_image;
    //public GameObject Gameover_set;
    public GameObject player;

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

        Player_move playerLogic = player.GetComponent<Player_move>();
        //Score_text.text = string.Format("{0:n0}", playerLogic.score); 

    }

    private void SpawnEnemy()
    {
        int ranEnemy= UnityEngine.Random.Range(0, 3);
        int ranPos = UnityEngine.Random.Range(0, 9);

        GameObject enemy=Instantiate(enemyObjs[ranEnemy], spawnPoints[ranPos].position, spawnPoints[ranPos].rotation);

        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player; //pass the player's info 

        //set the velocity of each positon
        //right
        if (ranPos == 5 || ranPos == 6)
        {
            rigid.velocity = new Vector2(enemyLogic.speed * -1, -1);
            rigid.transform.Rotate(Vector3.back * 45);
        }

        //left
        else if (ranPos == 7 || ranPos == 8)
        {
            rigid.velocity = new Vector2(enemyLogic.speed, -1);
            rigid.transform.Rotate(Vector3.forward * 45);
        }

        //top
        else
        {
            rigid.velocity = new Vector2(0, -1)*enemyLogic.speed;
        }

    }

    public void RespawnPlayer()
    {
        Invoke("RespawnPlayerExe", 2.0f);
    }
    void RespawnPlayerExe()
    {
        player.transform.position = new Vector3(0, -3.5f, 0);
        player.SetActive(true);
    }


}
