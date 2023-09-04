using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public float speed;
    public int health;
    public int Enemy_score;
    int oriHealth;
    public Sprite[] sprites;
    SpriteRenderer spriteRenderer;
    Animator anime;

    public GameObject player;
    public GameObject bulletObjA;
    public GameObject bulletObjB;
    public GameObject ItemBoom;
    public GameObject ItemCoin;
    public GameObject ItemPower;
    public ObjectManager objManager;
    public GameManager gameManager;
    public float maxShotDelay;
    public float curShotDelay;

    public int patternIndex;
    public int curPatternCount;
    public int[] maxPatternCount;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        oriHealth = health;
       
        if(enemyName=="B")
            anime= GetComponent<Animator>();
    }

    void Update()
    {
        if (enemyName == "B")
            return;
        Fire();
        Reload();
    }
    void Fire()
    {
        if (curShotDelay < maxShotDelay) //it doesn't work until Reloading is completed
            return;


        if (enemyName == "S")
        {
            GameObject bullet = objManager.MakeObj("bulletEnemyA");
            bullet.transform.position = transform.position;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector3 dirVec = player.transform.position - transform.position;
            rigid.AddForce(dirVec.normalized * 5, ForceMode2D.Impulse);
        }
        else if (enemyName == "L")
        {
            GameObject bulletR = objManager.MakeObj("bulletEnemyB");
            GameObject bulletL = objManager.MakeObj("bulletEnemyB");
            bulletR.transform.position = transform.position + Vector3.right * 03;
            bulletL.transform.position = transform.position + Vector3.left * 03;
            Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
            Vector3 dirVecR = player.transform.position - transform.position;
            Vector3 dirVecL = player.transform.position - transform.position;
            rigidR.AddForce(dirVecR.normalized * 4, ForceMode2D.Impulse);
            rigidL.AddForce(dirVecL.normalized * 4, ForceMode2D.Impulse);

        }

        curShotDelay = 0;
        
    }
    private void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    public void OnHit(int dmg)
    {
        //exception handling in double_itemdrop
        if (health <= 0)
            return;

        health -= dmg;

        if (enemyName == "B")
        {
            anime.SetTrigger("OnHit");
        }
        else
        {
            spriteRenderer.sprite = sprites[1];
            Invoke("ToOriSprite", 0.1f);
        }
       

        if (health <= 0)
        {
            gameManager.Call_Explosion(transform.position, enemyName);
            Player_move playerLogic = player.GetComponent<Player_move>();
            playerLogic.score += Enemy_score;

            
                int dropType = enemyName=="B"?0:Random.Range(0, 10);

            

                if (dropType>=3&&dropType < 6) {
                    GameObject ItemCoin=objManager.MakeObj("itemCoin");
                    ItemCoin.transform.position = transform.position;
                   
                }
                   
                else if (dropType <8)
                {
                    GameObject ItemPower = objManager.MakeObj("itemPower");
                    ItemPower.transform.position = transform.position;
                   
                }
                   
                else if (dropType < 10)
                {
                    GameObject ItemBoom = objManager.MakeObj("itemBoom");
                    ItemBoom.transform.position = transform.position;
                   
                }
            

            CancelInvoke();
            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity; // set to the default rotation value(0)
        }

    }

    private void OnEnable()
    {

        switch(enemyName){
            case "B":
                Invoke("Stop", 2);
               
                break;
            case "L":
              
                break;
            case "M":
               
                break;
            case "S":
                
                break;
        }
        health = oriHealth;
    }

    void Stop()
    {
        //Enable함수로 인한 Stop중복실행 방지
        if (!gameObject.activeSelf)
            return;
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;

        Invoke("Think", 2);
    }
    void Think()
    {
        patternIndex = (patternIndex == 3) ? 0 : patternIndex + 1;
        curPatternCount = 0;

        switch (patternIndex)
        {
            case 0:
                FireForward();
                break;
            case 1:
                FireShot();
                break;
            case 2:
                FireArc();
                break;
            case 3:
                FireAround();
                break;
        }

    }

    private void FireAround()
    {
        if (health <= 0)
            return;
        int roundNumA = 50; // the number of bullets
        int roundNumB = 40; // the number of bullets
        int roundNum = (curPatternCount%2==0)?roundNumA:roundNumB; // the number of bullets

        for (int i = 0; i < roundNum; i++)
        {
            GameObject bullet = objManager.MakeObj("bulletBossB");
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / roundNum*1.5f),
                Mathf.Sin(Mathf.PI * 2 * i / roundNum * 1.5f));
            rigid.AddForce(dirVec.normalized * 2, ForceMode2D.Impulse);


            //rotate a sprite of bullets
            Vector3 rotateVec = Vector3.forward * 360 * i / roundNum * 1.5f + Vector3.forward * 90;
            rigid.transform.Rotate(rotateVec);
        }

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireAround", 0.7f);
        else
            Invoke("Think", 3);
    }

    private void FireArc()
    {
        if (health <= 0)
            return;
        GameObject bullet = objManager.MakeObj("bulletEnemyA");
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.identity;
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

        Vector2 dirVec = new Vector2(Mathf.Sin(Mathf.PI*10*curPatternCount / maxPatternCount[patternIndex]),-1);
        rigid.AddForce(dirVec.normalized * 5, ForceMode2D.Impulse);

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireArc", 0.15f);
        else
            Invoke("Think", 3);
    }

    private void FireShot()
    {
        if (health <= 0)
            return;
        for (int i = 0; i < 5; i++)
        {
            GameObject bullet = objManager.MakeObj("bulletEnemyB");
            bullet.transform.position = transform.position;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector2 dirVec = player.transform.position - transform.position;
            Vector2 ranVec = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0f, 2f));
            dirVec += ranVec;
            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        }
        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireShot", 3.5f);
        else
            Invoke("Think", 3);
    }

    private void FireForward()
    {
        if (health <= 0)
            return;
        //Create bullet objs
        GameObject bulletL = objManager.MakeObj("bulletBossA");
        GameObject bulletR = objManager.MakeObj("bulletBossA");
        GameObject bulletLL = objManager.MakeObj("bulletBossA");
        GameObject bulletRR = objManager.MakeObj("bulletBossA");
        bulletL.transform.position = transform.position + Vector3.left * 0.3f;
        bulletR.transform.position = transform.position + Vector3.right * 0.3f;
        bulletLL.transform.position = transform.position + Vector3.left * 0.45f;
        bulletRR.transform.position = transform.position + Vector3.right * 0.45f;
        Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
        rigidL.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidR.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidLL.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidRR.AddForce(Vector2.down * 8, ForceMode2D.Impulse);

        //Pattern Counting
        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("FireForward", 2);
        else
            Invoke("Think", 3);
    }

    void ToOriSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderBullet"&&enemyName!="B")
        {
            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
        }
           

        else if (collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.dmg);
            collision.gameObject.SetActive(false);
            
        }

       
    }
}
