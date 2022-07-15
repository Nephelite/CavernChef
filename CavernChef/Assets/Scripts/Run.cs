using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Run
{
    public bool[] unlocks;
    public int scene;
    public int next;
    public int[] last3FoodChoicesID, upgradeCountList, upgradesEcon, upgradesFire, upgradesWater, upgradesSnow, upgradesLight, upgradesElectric, upgradesEarth, upgradesBlockage, upgradesPudding;
    public int numZones;

    public Run(bool[] unlocks, int scene, int next, int[] choices, int numZones, 
                int[] upgradeCountList, int[] upgradesEcon, int[] upgradesFire, int[] upgradesWater, 
                int[] upgradesSnow, int[] upgradesLight, int[] upgradesElectric, int[] upgradesEarth, 
                int[] upgradesBlockage, int[] upgradesPudding)
    {
        this.unlocks = unlocks;
        this.scene = scene;
        this.next = next;
        this.last3FoodChoicesID = choices;
        this.numZones = numZones;
        this.upgradeCountList = upgradeCountList;
        this.upgradesEcon = upgradesEcon;
        this.upgradesFire = upgradesFire;
        this.upgradesWater = upgradesWater;
        this.upgradesSnow = upgradesSnow;
        this.upgradesLight = upgradesLight;
        this.upgradesElectric = upgradesElectric;
        this.upgradesEarth = upgradesEarth;
        this.upgradesBlockage = upgradesBlockage;
        this.upgradesPudding = upgradesPudding;
    }
}
