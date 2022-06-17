using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    void Start()
    {
        RunManager.testRun = false;
    }

    public void play()
    {
        SceneManager.LoadScene(1);
    }
}
