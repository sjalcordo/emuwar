using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //public AudioSource bulletSFX; 
    public bool isLaser = false;
    // Start is called before the first frame update
    void Start()
    {
        Object.Destroy(gameObject, 0.1f);
        if(!isLaser)
            GameObject.Find("GunSFX").GetComponent<AudioSource>().Play();
        else
            GameObject.Find("emu laser").GetComponent<AudioSource>().Play();
        //bulletSFX.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    IEnumerator Sound()
    {
        
        yield return new WaitForSeconds(1f);
        Destroy
    }*/
}
