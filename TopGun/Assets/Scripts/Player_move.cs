using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_move : MonoBehaviour
{
    Animator anime;
    public float speed;

    public bool top_collision;
    public bool bottom_collision;
    public bool left_collsion;
    public bool right_collsion;


    private void Awake()
    {
        anime = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if ((h == 1 && right_collsion) || (h == -1 && left_collsion))
            h = 0;

        float v = Input.GetAxisRaw("Vertical");
        if ((v == 1 && top_collision) ||( v == -1 && bottom_collision))
            v = 0;

        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime; //물리적 이동이 아닌, transform을 통한 이동 시 deltatime 사용하기


        transform.position = curPos + nextPos;

        if (Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal"))
        {
            anime.SetInteger("Input", (int)h);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            Debug.Log("sdfsdfsd");
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
}
