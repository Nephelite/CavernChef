using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectWater : SelectButton
{
    void Start()
    {
        cost = (Resources.Load("WaterTRT") as GameObject).GetComponent<WaterTRT>().cost;
    }

    public void select()
    {
        if (GlobalVariables.repelPoints >= cost) 
        {
            GlobalVariables.selectedTrt = (GameObject)Resources.Load("WaterTRT");
            GlobalVariables.isDefensiveTRT = false;
            GlobalVariables.isOffensiveTRT = true;
        }
        // else text shows not enough RP to be implemented
    }
}
