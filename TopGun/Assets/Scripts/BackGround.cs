using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    // Start is called before the first frame update
    public int speed;
    public int startIdx;
    public int endIdx;
    public Transform[] backSprites;
    float viewHeight;

    private void Awake()
    {
        viewHeight = Camera.main.orthographicSize * 2;
    }
    void Update()
    {

        BackGroundMove();
        Scrolling();
    }

    void BackGroundMove()
    {
        Vector3 curPos = transform.position;
        Vector3 nextPos = Vector3.down * speed * Time.deltaTime;
        transform.position = curPos + nextPos;
    }

    void Scrolling()
    {
        if (backSprites[endIdx].transform.position.y < viewHeight * (-1))
        {
            //Sprite resue
            Vector3 frontSpritePos = backSprites[startIdx].localPosition;
            Vector3 backSpritePos = backSprites[endIdx].localPosition;
            backSprites[endIdx].transform.localPosition = frontSpritePos + Vector3.up * viewHeight;

            //Index update
            int temp = startIdx;
            startIdx = endIdx;
            temp += 1;
            endIdx = (temp > backSprites.Length - 1) ? temp = 0 : temp;
        }
    }
}
