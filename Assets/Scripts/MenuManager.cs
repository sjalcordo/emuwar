using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public AudioSource bkgdMusic;
    private static AudioSource _instance;
    public GameObject controls;
    public bool firstPress = false;
    public GameObject startButton;
    public AudioSource startSound;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("bgm").GetComponent<AudioSource>() != null
            && !GameObject.Find("bgm").GetComponent<AudioSource>().isPlaying)
        {
            GameObject.Find("bgm").GetComponent<AudioSource>().Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit") || Input.GetButtonDown("Jump"))
        {
            MenuButtons(0);
            startSound.Play();
        }
        else if (Input.GetButtonDown("Cancel"))
        {
            if(firstPress)
            {
                controls.SetActive(false);
                startButton.SetActive(false);
            }
            else
                MenuButtons(1);
        }
    }

    public void MenuButtons(int i)
    {
        switch (i)
        {
            case 0: //start
                
                controls.SetActive(true);
                startButton.SetActive(true);
                if (firstPress)
                {
                    StartCoroutine("StartSequence");
                }
                else
                {
                    firstPress = true;
                }
                break;
            case 1: // quit
                Destroy(GameObject.Find("bgm"));
                Application.Quit();
                break;
        }
    }

    IEnumerator StartSequence()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }

    // persisting bgm
    public static AudioSource Instance
    {
        // prevents accessing the raw variable
        get { return _instance; }
    }

    private void Awake()
    {
        // already initialized and isn't the current song
        if (_instance != null && _instance != this.bkgdMusic)
        {
            // delete any new instances
            Destroy(bkgdMusic);
            return;
        }
        // set instance to current object
        _instance = this.bkgdMusic;

        // persisting bkgd music
        DontDestroyOnLoad(bkgdMusic);
    }


}
