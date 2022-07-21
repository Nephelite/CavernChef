using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GlobalVariables.settings = SaveSystem.LoadSettings();
    }

    public void select()
    {
        SceneManager.LoadScene(17);
    }

    public void goBack()
    {
        SaveSystem.SaveCurrentSettings(GlobalVariables.settings);
        SceneManager.LoadScene(2);
    }

    public void reset()
    {
        GlobalVariables.settings = new Settings(1f, 1f, true);
        AudioManager manager = FindObjectOfType<AudioManager>();
        manager.UpdateNewMusicVolume();
        manager.UpdateNewSFXVolume();
        SaveSystem.SaveCurrentSettings(GlobalVariables.settings);
    }
}
