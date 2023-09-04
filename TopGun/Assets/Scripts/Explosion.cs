using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    Animator anime;
    private void Awake()
    {
        anime = GetComponent<Animator>();
    }

    public void OnExplosion(string target)
    {
        anime.SetTrigger("IsExplosion");

        switch (target)
        {
            case "S":
                transform.localScale = Vector3.one * 0.7f;
                break;
            case "M":             
            case "P":
                transform.localScale = Vector3.one * 1f;
                break; 
            case "L":
                transform.localScale = Vector3.one * 2f;
                break;
            case "N":
                transform.localScale = Vector3.one * 3f;
                break;
        }

        Invoke("Disabled", 2.0f);

    }

    void Disabled()
    {
        gameObject.SetActive(false);
    }

   
}
