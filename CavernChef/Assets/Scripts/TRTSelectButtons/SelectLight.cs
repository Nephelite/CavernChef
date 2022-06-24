using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLight : SelectButton
{
    void Start()
    {
        cost = (Resources.Load("LightTRT") as GameObject).GetComponent<LightTRT>().cost;
    }

    public void select()
    {
        if (GlobalVariables.repelPoints >= cost) 
        {
            GlobalVariables.selectedTrt = (GameObject)Resources.Load("LightTRT");
            GlobalVariables.isDefensiveTRT = false;
            GlobalVariables.isOffensiveTRT = true;
        }
        // else text shows not enough RP to be implemented
    }
}
