using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectWater : MonoBehaviour
{
    public int cost;
    public void select()
    {
        if (GlobalVariables.repelPoints >= cost) 
        {
            GlobalVariables.selectedTrt = (GameObject)Resources.Load("WaterTRT");
        }
        // else text shows not enough RP to be implemented
    }
}
