using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MenuButtons(int i)
    {
        switch (i)
        {
            case 0: //start
                //add sfx
                SceneManager.LoadScene(0);
                break;
            case 1: // quit
                Application.Quit();
                break;
        }
    }
}
