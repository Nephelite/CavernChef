using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AlmanacOpen : MonoBehaviour
{
    public void open()
    {
        SceneManager.LoadScene(10);
    }
}
