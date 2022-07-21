using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Settings
{
    public float SFXVol, musicVol;
    public bool lightingToggle;

    public Settings(float musicVol, float SFXVol, bool lightingToggle)
    {
        this.SFXVol = SFXVol;
        this.musicVol = musicVol;
        this.lightingToggle = lightingToggle;
    }
}
