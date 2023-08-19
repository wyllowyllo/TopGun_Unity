using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public float speed;
    public int health;
    public int Enemy_score;
    public Sprite[] sprites;
    SpriteRenderer spriteRenderer;

    public GameObject player;
    public GameObject bulletObjA;
    public GameObject bulletObjB;
    public float maxShotDelay;
    public float curShotDelay;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
       
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
            GameObject bullet = Instantiate(bulletObjA, transform.position, transform.rotation); //Obj ����
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector3 dirVec = player.transform.position - transform.position;
            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        }
        else if (enemyName == "L")
        {
            GameObject bulletR = Instantiate(bulletObjB, transform.position+Vector3.right*0.3f, transform.rotation); 
            GameObject bulletL = Instantiate(bulletObjB, transform.position+Vector3.left*0.3f, transform.rotation); 
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

    void OnHit(int dmg)
    {
        health -= dmg;
        spriteRenderer.sprite = sprites[1];
        Invoke("ToOriSprite", 0.1f);

        if (health <= 0)
        {
            Player_move playerLogic = player.GetComponent<Player_move>();
            playerLogic.score += Enemy_score;
            Destroy(gameObject);
        }

    }

    void ToOriSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderBullet")
            Destroy(gameObject);

        else if (collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.dmg);
            Destroy(collision.gameObject);
        }

       
    }
}
