using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveFile
{
    public bool[] seenTRTs, seenFoods, seenEnemies;
    public SaveFile(bool[] seenTRTs, bool[] seenFoods, bool[] seenEnemies)
    {
        this.seenTRTs = seenTRTs;
        this.seenFoods = seenFoods;
        this.seenEnemies = seenEnemies;
    }
}