using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public TMP_Text musicVal, SFXVal, displayToggle;
    public Slider musicSlider, sfxSlider;
    public Settings oldSettings;

    void Start()
    {
        oldSettings = SaveSystem.LoadSettings();
        GlobalVariables.settings = oldSettings;
        musicSlider.value = oldSettings.musicVol / 2f;
        musicVal.text = ((int)(oldSettings.musicVol * 50f)).ToString() + "%";
        sfxSlider.value = oldSettings.SFXVol / 2f;
        SFXVal.text = ((int)(oldSettings.SFXVol * 50f)).ToString() + "%";
        displayToggle.text = oldSettings.lightingToggle ? "(Active)" : "(Not Active)";
    }

    public void reset()
    {
        GlobalVariables.settings = new Settings(1f, 1f, true);
        oldSettings = GlobalVariables.settings;
        musicSlider.value = oldSettings.musicVol / 2f;
        musicVal.text = ((int)(oldSettings.musicVol * 50f)).ToString() + "%";
        sfxSlider.value = oldSettings.SFXVol / 2f;
        SFXVal.text = ((int)(oldSettings.SFXVol * 50f)).ToString() + "%";
        displayToggle.text = oldSettings.lightingToggle ? "(Active)" : "(Not Active)";
    }

    public void changeMusicVal(float value)
    {
        musicVal.text = ((int) (value * 100f)).ToString() + "%";
        Settings newSettings = new Settings(value * 2f, oldSettings.SFXVol, oldSettings.lightingToggle);
        GlobalVariables.settings = newSettings;
        oldSettings = newSettings;
        FindObjectOfType<AudioManager>().UpdateNewMusicVolume();
    }

    public void changeSFXVal(float value)
    {
        SFXVal.text = ((int) (value * 100f)).ToString() + "%";
        Settings newSettings = new Settings(oldSettings.musicVol, value * 2f, oldSettings.lightingToggle);
        oldSettings = newSettings;
        GlobalVariables.settings = newSettings;
    }

    public void changeLightingToggle()
    {
        Settings newSettings = new Settings(oldSettings.musicVol, oldSettings.SFXVol, !oldSettings.lightingToggle);
        displayToggle.text = newSettings.lightingToggle ? "(Active)" : "(Not Active)";
        GlobalVariables.settings = newSettings;
        oldSettings = newSettings;
    }
}
