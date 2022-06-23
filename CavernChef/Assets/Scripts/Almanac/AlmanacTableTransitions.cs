using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AlmanacTableTransitions : MonoBehaviour
{
    public void open(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
