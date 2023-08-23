using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string Item_type;
    Rigidbody2D rigid;
    private void OnEnable()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        rigid.velocity = Vector3.down * 1.5f;
    }
}
