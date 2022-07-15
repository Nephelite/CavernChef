using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSnow : SelectButton
{
    void Start()
    {
        cost = (Resources.Load("SnowTRT") as GameObject).GetComponent<SnowTRT>().Cost();
    }

    public void select()
    {
        if (GlobalVariables.repelPoints >= cost) 
        {
            GlobalVariables.selectedTrt = (GameObject)Resources.Load("SnowTRT");
            GlobalVariables.isDefensiveTRT = false;
            GlobalVariables.isOffensiveTRT = true;
        }
        // else text shows not enough RP to be implemented
    }
}
