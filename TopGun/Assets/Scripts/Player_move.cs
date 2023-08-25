using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_move : MonoBehaviour
{
    Animator anime;
    public float speed;
    public int power;
    public int max_power;
    public int boom_num;
    public int max_boom_num;
    public bool isboom_time;
    public int life;
    public int score;
    public bool isHit; //중복피격 방지
    public bool top_collision;
    public bool bottom_collision;
    public bool left_collsion;
    public bool right_collsion;


    public float maxShotDelay;
    public float curShotDelay;

    public GameManager gameManager;
    public ObjectManager objManager;
    public GameObject bulletObjA;
    public GameObject bulletObjB;
    public GameObject boom_effect;
    public GameObject[] followers;
    private void Awake()
    {
        anime = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
        Reload();
        Boom();
    }

    private void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if ((h == 1 && right_collsion) || (h == -1 && left_collsion))
            h = 0;

        float v = Input.GetAxisRaw("Vertical");
        if ((v == 1 && top_collision) || (v == -1 && bottom_collision))
            v = 0;

        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime; //물리적 이동이 아닌, transform을 통한 이동 시 deltatime 사용하기


        transform.position = curPos + nextPos;

        if (Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal"))
        {
            anime.SetInteger("Input", (int)h);
        }
    }
    void Fire()
    {
        if (curShotDelay < maxShotDelay) //it doesn't work until Reloading is completed
            return;

        if (Input.GetButton("Fire1"))
        {

            switch (power)
            {
                case 1:
                    GameObject bullet = objManager.MakeObj("bulletPlayerA");
                    bullet.transform.position = transform.position;
                       
                    Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                    rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                    break;

                case 2:
                    GameObject bulletL = objManager.MakeObj("bulletPlayerA");
                    GameObject bulletR = objManager.MakeObj("bulletPlayerA");
                    bulletL.transform.position = transform.position + Vector3.left * 0.1f;
                    bulletR.transform.position = transform.position + Vector3.right * 0.1f;                
                    Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                    Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                    rigidL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                    rigidR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                    break;

                default:
                    GameObject bulletLL = objManager.MakeObj("bulletPlayerA");            
                    GameObject bulletRR = objManager.MakeObj("bulletPlayerA");                 
                    GameObject bulletCC = objManager.MakeObj("bulletPlayerB");
                    bulletLL.transform.position = transform.position + Vector3.left * 0.35f;
                    bulletRR.transform.position = transform.position + Vector3.right * 0.35f;
                    bulletCC.transform.position = transform.position;
                    Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
                    Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
                    Rigidbody2D rigidCC = bulletCC.GetComponent<Rigidbody2D>();
                    rigidLL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                    rigidRR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                    rigidCC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                    break;
            }


            curShotDelay = 0;
        }
    }

    void Boom()
    {
        if (Input.GetButtonDown("Fire2") && !isboom_time && boom_num >= 1)
        {
            boom_num--;
            isboom_time = true;
            gameManager.UpdateBoomImage(boom_num);
            //make the effect visible
            boom_effect.SetActive(true);

            //kill enemies
            GameObject[] enemiesS = objManager.GetPool("enemyS");
            GameObject[] enemiesM = objManager.GetPool("enemyM");
            GameObject[] enemiesL = objManager.GetPool("enemyL");
            

            //GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < enemiesS.Length; i++)
            {
                if (enemiesS[i].activeSelf)
                {
                    Enemy enemyLogic = enemiesS[i].GetComponent<Enemy>();
                    enemyLogic.OnHit(1000);
                }
               
            }
            for (int i = 0; i < enemiesM.Length; i++)
            {
                if (enemiesM[i].activeSelf)
                {
                    Enemy enemyLogic = enemiesM[i].GetComponent<Enemy>();
                    enemyLogic.OnHit(1000);
                }

            }
            for (int i = 0; i < enemiesL.Length; i++)
            {
                if (enemiesL[i].activeSelf)
                {
                    Enemy enemyLogic = enemiesL[i].GetComponent<Enemy>();
                    enemyLogic.OnHit(1000);
                }

            }

            //remove bullets
            GameObject[] bulletEnemyA = objManager.GetPool("bulletEnemyA");
            GameObject[] bulletEnemyB = objManager.GetPool("bulletEnemyB");
            //GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
            for (int i = 0; i < bulletEnemyA.Length; i++)
            {
                if(bulletEnemyA[i].activeSelf)
                    bulletEnemyA[i].SetActive(false);
            }
              
            for (int i = 0; i < bulletEnemyB.Length; i++)
            {
                if (bulletEnemyB[i].activeSelf)
                    bulletEnemyB[i].SetActive(false);
            }
               

            Invoke("Off_BoomEffect", 3.0f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            
            switch (collision.gameObject.name)
            {
                case "Top":
                    top_collision = true;
                    break;

                case "Bottom":
                    bottom_collision = true;
                    break;

                case "Left":
                    left_collsion = true;
                    break;

                case "Right":
                    right_collsion = true;
                    break;
            }
        }

        else if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet")
        {

            if (isHit)
                return;

            life--;

            if (life <= 0)
            {
                gameManager.GameOver();
            }
            else
            {
                gameManager.RespawnPlayer();
            }

            gameManager.UpdateLifeImage(life);
            gameObject.SetActive(false);

        }

        else if(collision.gameObject.tag == "Item")
        {
            Item item = collision.gameObject.GetComponent<Item>();

            switch (item.Item_type)
            {
                case "Power":
                    power++;
                    AddFollower();
                  
                    break;

                case "Coin":
                    score += 1000;
                    break;

                case "Boom":
                    if (boom_num < max_boom_num)
                    {
                        boom_num++;
                        gameManager.UpdateBoomImage(boom_num);
                    }
                    else
                        score += 500;
                  
                    break;
            }
            collision.gameObject.SetActive(false);
           
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    top_collision = false;
                    break;

                case "Bottom":
                    bottom_collision = false;
                    break;

                case "Left":
                    left_collsion = false;
                    break;

                case "Right":
                    right_collsion = false;
                    break;
            }
        }

       
    }

    void Off_BoomEffect()
    {
        boom_effect.SetActive(false);
        isboom_time = false;
    }

    void AddFollower()
    {
        if (power == 4)
            followers[0].SetActive(true);
        else if (power == 5)
            followers[1].SetActive(true);
        else if (power == 6)
            followers[2].SetActive(true);
    }
}
