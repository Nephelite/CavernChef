using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AlmanacTableTransitions : MonoBehaviour
{
    public void open(int scene)
    {
        SceneManager.LoadScene(scene);
        if (scene == 2)
        {
            FindObjectOfType<AudioManager>().StopAllAudio();
            FindObjectOfType<AudioManager>().PlayMusic("MenuTheme");
            if (SceneManager.GetActiveScene().buildIndex == 10)
            {
                FindObjectOfType<AudioManager>().Play("CloseBook");
            }
        }
        else if (scene == 9)
        {
            FindObjectOfType<AudioManager>().StopAllAudio();
            FindObjectOfType<AudioManager>().PlayMusic("AlmanacTheme");
            FindObjectOfType<AudioManager>().Play("PickUpBook");
        }
        else if (scene == 10)
        {
            FindObjectOfType<AudioManager>().Play("TurnPage2");
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("TurnPage1");
        }
    }
}
