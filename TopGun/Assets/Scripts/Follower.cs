using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public float maxShotDelay;
    public float curShotDelay;
    public ObjectManager objManager;

    public Vector3 followPos;
    public int followDelay;
    public Transform parent;
    public Queue<Vector3> parentPos;


    private void Awake()
    {
        parentPos = new Queue<Vector3>();
    }
    void Update()
    {
        Watch();
        Follow();
        Fire();
        Reload();
      
    }

    private void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    void Watch()
    {

        // when a player stop the moving, followers also stop at their postison right away
        if(!parentPos.Contains(parent.position))
            parentPos.Enqueue(parent.position);

        if(parentPos.Count>followDelay)
            followPos = parentPos.Dequeue();

        else if(parentPos.Count<followDelay) // 첫 생성 시 위치 설정
            followPos = parent.position;

    }
    void Follow()
    {
        transform.position = followPos;
    }
    void Fire()
    {
        if (curShotDelay < maxShotDelay) //it doesn't work until Reloading is completed
            return;


                    GameObject bullet = objManager.MakeObj("bulletFollower");
                    bullet.transform.position = transform.position;

                    Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                    rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                    


            curShotDelay = 0;
        
    }
}
