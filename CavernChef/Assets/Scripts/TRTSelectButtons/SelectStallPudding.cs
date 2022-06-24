using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectStallPudding : SelectButton
{
    void Start()
    {
        cost = (Resources.Load("PuddingTRT") as GameObject).GetComponent<StallTRT>().cost;
    }

    public void select()
    {
        if (GlobalVariables.repelPoints >= cost)
        {
            GlobalVariables.selectedTrt = (GameObject)Resources.Load("PuddingTRT");
            GlobalVariables.isDefensiveTRT = true;
            GlobalVariables.isOffensiveTRT = false;
        }
        // else text shows not enough RP to be implemented
    }
}
