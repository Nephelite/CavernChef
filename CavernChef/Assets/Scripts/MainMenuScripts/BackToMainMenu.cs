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
        RunManager.seenTRTs = new bool[32];
        RunManager.seenEnemies = new bool[32];
        RunManager.seenFoods = new bool[32];
        RunManager.accessibleButtonsSaveData = new bool[32];
    }
}
