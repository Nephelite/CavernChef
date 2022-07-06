using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitApplication : MonoBehaviour
{
    public static bool gameStarted = true;

    public void EndApp()
    {
        Application.Quit();
    }
}
