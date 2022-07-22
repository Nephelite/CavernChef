using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitApplication : MonoBehaviour
{
    public static bool gameStarted = true;

    void Start()
    {
        GlobalVariables.settings = SaveSystem.LoadAnySetting();
        AudioManager manager = FindObjectOfType<AudioManager>();
        manager.UpdateNewMusicVolume();
        manager.UpdateNewSFXVolume();
    }

    public void EndApp()
    {
        Application.Quit();
    }
}
