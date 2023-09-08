using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    Animator anime;
    AudioSource audiosource;
    public AudioClip[] audioClips;
    enum soundeffect { S,M,L,B};
    private void Awake()
    {
        anime = GetComponent<Animator>();
        audiosource = GetComponent<AudioSource>();
    }

    public void OnExplosion(string target)
    {
        anime.SetTrigger("IsExplosion");
       
        switch (target)
        {
            case "S":
                audiosource.clip = audioClips[(int)soundeffect.S];
                audiosource.Play();
                transform.localScale = Vector3.one * 0.7f;
                break;
            case "M":             
            case "P":
                audiosource.clip = audioClips[(int)soundeffect.M];
                audiosource.Play();
                transform.localScale = Vector3.one * 1f;
                break; 
            case "L":
                audiosource.clip = audioClips[(int)soundeffect.L];
                audiosource.Play();
                transform.localScale = Vector3.one * 2f;
                break;
            case "B":
                audiosource.clip = audioClips[(int)soundeffect.B];
                audiosource.Play();
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
