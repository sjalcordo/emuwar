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
        if (Input.GetButtonDown("Submit") || Input.GetButtonDown("Jump"))
        {
            MenuButtons(0);
        }
        else if (Input.GetButtonDown("Cancel"))
        {
            MenuButtons(1);
        }
    }

    public void MenuButtons(int i)
    {
        switch (i)
        {
            case 0: //start
                //add sfx
                SceneManager.LoadScene(1);
                break;
            case 1: // quit
                Application.Quit();
                break;
        }
    }

}
