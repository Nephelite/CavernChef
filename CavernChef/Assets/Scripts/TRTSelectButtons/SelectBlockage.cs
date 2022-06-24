using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBlockage : SelectButton
{
    void Start()
    {
        cost = (Resources.Load("BlockageTRT") as GameObject).GetComponent<BlockageTRT>().cost;
    }

    public void select()
    {
        if (GlobalVariables.repelPoints >= cost)
        {
            GlobalVariables.selectedTrt = (GameObject)Resources.Load("BlockageTRT");
            GlobalVariables.isDefensiveTRT = true;
            GlobalVariables.isOffensiveTRT = false;
        }
        // else text shows not enough RP to be implemented
    }
}
