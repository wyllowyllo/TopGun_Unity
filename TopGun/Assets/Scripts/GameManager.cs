using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    string[] enemyObjs;
    public Transform[] spawnPoints;
    public Text Score_text;
    public Image[] Health_image;
    public Image[] Boom_image;
    public GameObject Gameover_set;
    public GameObject player;
    public ObjectManager objManager;
    public Animator startanime;
    public Animator endanime;
    public Animator fadeanime;
    public Transform player_spawnpos;

    public int Stage;

    public float curSpawnDelay;
    public float nextSpawnDelay;

    public List<Spawn> spawnList;
    public int spawnIndex;
    public bool spawnEnd;


    private void Awake()
    {
        enemyObjs = new string[] { "enemyS", "enemyM", "enemyL" , "boss" };
        spawnList = new List<Spawn>();
        StartStage();
    }


    public void StartStage()
    {
        //#.Stage UI load
        startanime.SetTrigger("On");
        startanime.GetComponent<Text>().text = "Stage " + Stage;
       
        //#. Read a spawn enemy file
        ReadSpawnFile();

        //#.Fade in
        fadeanime.SetTrigger("In");
    }
    public void EndStage()
    {
        //#.Clear UI load
        endanime.SetTrigger("On");
        endanime.GetComponent<Text>().text = "Clear!";
       
       

        //#.Fade out
        fadeanime.SetTrigger("Out");
        //#.Player repos
        transform.position = player_spawnpos.position;

        //#.Stage increment
        Stage++;

        //#.Next Stage

        if (Stage < 3)
            Invoke("StartStage", 5);
        else
            Invoke("GameOver",6);

      
    }
    void ReadSpawnFile()
    {
        //Initialization
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        //Read a respawn file
        //Load a file from the directory 'Resources'
        TextAsset textFile = Resources.Load("Stage "+Stage) as TextAsset; // if the 'Stage 0' file is not a 'TextAsset' type, return null;
        StringReader stringReader = new StringReader(textFile.text);

      

        while (stringReader != null)
        {
            string line = stringReader.ReadLine();
            Debug.Log(line);
            if (line == null)
                break;

            Spawn file = new Spawn();
            file.delay = float.Parse(line.Split(',')[0]);
            file.type = line.Split(',')[1];
            file.point = int.Parse(line.Split(',')[2]);

            spawnList.Add(file);
        }

        //Close the file
        stringReader.Close();

        //apply the first date of delay  
        nextSpawnDelay = spawnList[0].delay;
    }
    private void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if (curSpawnDelay >= nextSpawnDelay && !spawnEnd)
        {
            SpawnEnemy();      
            curSpawnDelay = 0;
        }

        Player_move playerLogic = player.GetComponent<Player_move>();
        Score_text.text = string.Format("{0:n0}", playerLogic.score);

       

    }

    private void SpawnEnemy()
    {
        int enemyType=0;
        switch (spawnList[spawnIndex].type)
        {
            case "B":
                enemyType = 3;
                break;
            case "L":
                enemyType = 2;
                break;
            case "M":
                enemyType = 1;
                break;
            case "S":
                enemyType = 0;
                break;
           
        }

        int spawnPos = spawnList[spawnIndex].point;
           
        //GameObject enemy=Instantiate(enemyObjs[enemyType], spawnPoints[spawnPos].position, spawnPoints[spawnPos].rotation);
       
        GameObject enemy = objManager.MakeObj(enemyObjs[enemyType]);
        enemy.transform.position = spawnPoints[spawnPos].position;

        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player; //pass the player's info 
        enemyLogic.objManager = objManager;//pass the Objectmanager's info 
        enemyLogic.gameManager = this; //pass the Gamemanager's info

        //set the velocity of each positon
        //right
        if (spawnPos == 5 || spawnPos == 6)
        {
            rigid.velocity = new Vector2(enemyLogic.speed * -1, -1);
            rigid.transform.Rotate(Vector3.back * 90);
        }

        //left
        else if (spawnPos == 7 || spawnPos == 8)
        {
            rigid.velocity = new Vector2(enemyLogic.speed, -1);
            rigid.transform.Rotate(Vector3.forward * 90);
        }

        //top
        else
        {
            rigid.velocity = new Vector2(0, -1)*enemyLogic.speed;
        }

        spawnIndex++;
        if (spawnIndex == spawnList.Count)
        {
            spawnEnd = true;
            return;
        }

        nextSpawnDelay = spawnList[spawnIndex].delay;

    }

    public void RespawnPlayer()
    {
        Invoke("RespawnPlayerExe", 2.0f);
    }
    void RespawnPlayerExe()
    {
        player.transform.position = new Vector3(0, -3.5f, 0);
        player.SetActive(true);

        Player_move playerLogic = player.GetComponent<Player_move>();
        playerLogic.isHit = false;

      
    }



    public void UpdateLifeImage(int life)
    {

        for (int i = 0; i < 3; i++)
            Health_image[i].color = new Color(1, 1, 1, 0);

        for (int i = 0; i < life; i++)
        {
            Health_image[i].color = new Color(1, 1, 1, 1);
        }
       
    }

    public void UpdateBoomImage(int boom)
    {

        for (int i = 0; i < 3; i++)
            Boom_image[i].color = new Color(1, 1, 1, 0);

        for (int i = 0; i < boom; i++)
        {
            Boom_image[i].color = new Color(1, 1, 1, 1);
        }

    }

    public void GameOver()
    {
        Gameover_set.SetActive(true);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
       
    }

    public void Call_Explosion(Vector3 pos,string type)
    {
        GameObject explosion = objManager.MakeObj("explosion");
        Explosion explosionLogic = explosion.GetComponent<Explosion>();
        explosionLogic.transform.position = pos;
        explosionLogic.OnExplosion(type);
        
    }
}
