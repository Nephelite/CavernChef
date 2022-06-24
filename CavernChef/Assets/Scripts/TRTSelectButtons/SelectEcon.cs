using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectEcon : SelectButton
{
    void Start()
    {
        cost = (Resources.Load("EconTRT") as GameObject).GetComponent<EconTRT>().cost;
    }

    public void select()
    {
        if (GlobalVariables.repelPoints >= cost)
        {
            GlobalVariables.selectedTrt = (GameObject)Resources.Load("EconTRT");
            GlobalVariables.isDefensiveTRT = false;
            GlobalVariables.isOffensiveTRT = true;
        }
        // else text shows not enough RP to be implemented
    }
}
