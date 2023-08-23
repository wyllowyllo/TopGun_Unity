using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ObjectManager : MonoBehaviour
{

    public GameObject enemyLPrefabs;
    public GameObject enemyMPrefabs;
    public GameObject enemySPrefabs;
    public GameObject itemCoinPrefabs;
    public GameObject itemBoomPrefabs;
    public GameObject itemPowerPrefabs;
    public GameObject bulletPlayerAPrefabs;
    public GameObject bulletPlayerBPrefabs;
    public GameObject bulletEnemyAPrefabs;
    public GameObject bulletEnemyBPrefabs;

    GameObject[] enemyL;
    GameObject[] enemyM;
    GameObject[] enemyS;

    GameObject[] itemCoin;
    GameObject[] itemBoom;
    GameObject[] itemPower;

    GameObject[] bulletPlayerA;
    GameObject[] bulletPlayerB;
    GameObject[] bulletEnemyA;
    GameObject[] bulletEnemyB;

    GameObject[] targetPool;
    private void Awake()
    {
        enemyL = new GameObject[10];
        enemyM = new GameObject[10];
        enemyS = new GameObject[20];

        itemCoin = new GameObject[20];
        itemBoom = new GameObject[10];
        itemPower = new GameObject[10];

        bulletPlayerA = new GameObject[100];
        bulletPlayerB = new GameObject[100];
        bulletEnemyA = new GameObject[100];
        bulletEnemyB = new GameObject[100];

        Generate();
    }

    private void Generate()
    {
        //Enemy
        for (int i = 0; i < enemyL.Length; i++)
        {
            enemyL[i] = Instantiate(enemyLPrefabs);
            enemyL[i].SetActive(false);
        } 
            

        for (int i = 0; i < enemyM.Length; i++)
        {
            enemyM[i] = Instantiate(enemyMPrefabs);
            enemyM[i].SetActive(false);
        }
            

        for (int i = 0; i < enemyS.Length; i++)
        {
            enemyS[i] = Instantiate(enemySPrefabs);
            enemyS[i].SetActive(false);
        }
            

        //Item
        for (int i = 0; i < itemCoin.Length; i++)
        {
            itemCoin[i] = Instantiate(itemCoinPrefabs);
            itemCoin[i].SetActive(false);
        }
            

        for (int i = 0; i < itemBoom.Length; i++)
        {
            itemBoom[i] = Instantiate(itemPowerPrefabs);
            itemBoom[i].SetActive(false);
        }
            

        for (int i = 0; i < itemPower.Length; i++)
        {
            itemPower[i] = Instantiate(itemBoomPrefabs);
            itemPower[i].SetActive(false);
        }
           

        //bullet
        for (int i = 0; i < bulletPlayerA.Length; i++)
        {
            bulletPlayerA[i] = Instantiate(bulletPlayerAPrefabs);
            bulletPlayerA[i].SetActive(false);
        }
           

        for (int i = 0; i < bulletPlayerB.Length; i++)
        {
            bulletPlayerB[i] = Instantiate(bulletPlayerBPrefabs);
            bulletPlayerB[i].SetActive(false);
        }
          

        for (int i = 0; i < bulletEnemyA.Length; i++)
        {
            bulletEnemyA[i] = Instantiate(bulletEnemyAPrefabs);
            bulletEnemyA[i].SetActive(false);
        }
           

        for (int i = 0; i < bulletEnemyB.Length; i++)
        {
            bulletEnemyB[i] = Instantiate(bulletEnemyAPrefabs);
            bulletEnemyB[i].SetActive(false);
        }
            
    }

    public GameObject MakeObj(string type)
    {

        switch (type) {
            case "enemyL":
                targetPool = enemyL;
                break;
            case "enemyM":
                targetPool = enemyM;
                break;
            case "enemyS":
                targetPool = enemyS;
                break;
            case "itemCoin":
                targetPool = itemCoin;
                break;
            case "itemBoom":
                targetPool = itemBoom;
                break;
            case "itemPower":
                targetPool = itemPower;
                break;
            case "bulletPlayerA":
                targetPool = bulletPlayerA;
                break;
            case "bulletPlayerB":
                targetPool = bulletPlayerB;
                break;
            case "bulletEnemyA":
                targetPool = bulletEnemyA;
                break;
            case "bulletEnemyB":
                targetPool = bulletEnemyB;
                break;

        }

        for(int i=0;i<targetPool.Length;i++)
        {
            if (!targetPool[i].activeSelf)
            {
                targetPool[i].SetActive(true);
                return targetPool[i];
            }
        }
        
          return null;
    }

    public GameObject[] GetPool(string type)
    {
        switch (type)
        {
            case "enemyL":
                targetPool = enemyL;
                break;
            case "enemyM":
                targetPool = enemyM;
                break;
            case "enemyS":
                targetPool = enemyS;
                break;
            case "itemCoin":
                targetPool = itemCoin;
                break;
            case "itemBoom":
                targetPool = itemBoom;
                break;
            case "itemPower":
                targetPool = itemPower;
                break;
            case "bulletPlayerA":
                targetPool = bulletPlayerA;
                break;
            case "bulletPlayerB":
                targetPool = bulletPlayerB;
                break;
            case "bulletEnemyA":
                targetPool = bulletEnemyA;
                break;
            case "bulletEnemyB":
                targetPool = bulletEnemyB;
                break;

        }
        return targetPool;
    }
}
