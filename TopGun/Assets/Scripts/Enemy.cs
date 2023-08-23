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

    public GameObject player;
    public GameObject bulletObjA;
    public GameObject bulletObjB;
    public GameObject ItemBoom;
    public GameObject ItemCoin;
    public GameObject ItemPower;
    public ObjectManager objManager;
    public float maxShotDelay;
    public float curShotDelay;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        oriHealth = health;
       
    }

    void Update()
    {
       
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
        spriteRenderer.sprite = sprites[1];
        Invoke("ToOriSprite", 0.1f);

        if (health <= 0)
        {
            Player_move playerLogic = player.GetComponent<Player_move>();
            playerLogic.score += Enemy_score;

            bool isdrop = (Random.Range(0, 2) == 0) ? false : true;

            if (isdrop)
            {
                int dropType = Random.Range(0, 10);

                if (dropType < 5) {
                    GameObject ItemCoin=objManager.MakeObj("itemCoin");
                    ItemCoin.transform.position = transform.position;
                   
                }
                   
                else if (dropType < 7)
                {
                    GameObject ItemPower = objManager.MakeObj("itemPower");
                    ItemPower.transform.position = transform.position;
                   
                }
                   
                else if (dropType < 9)
                {
                    GameObject ItemBoom = objManager.MakeObj("itemBoom");
                    ItemBoom.transform.position = transform.position;
                   
                }
                    
            }

            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity; // set to the default rotation value(0)
        }

    }

    private void OnEnable()
    {

        /*switch(enemyName){
            case "L":
                break;
            case "M":
                break;
            case "S":
                break;
        }*/
        health = oriHealth;
    }

    void ToOriSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderBullet")
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
