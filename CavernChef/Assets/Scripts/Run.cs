using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Run
{
    public bool[] unlocks;
    public int scene;
    public int next;
    public int[] last3FoodChoicesID;
    public Run(bool[] unlocks, int scene, int next, int[] choices)
    {
        this.unlocks = unlocks;
        this.scene = scene;
        this.next = next;
        this.last3FoodChoicesID = choices;
    }
}
