using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMainMenu : MonoBehaviour
{
    public void action()
    {
        SceneManager.LoadScene(0);
        GlobalVariables.SaveFileID = 0;
    }
}
