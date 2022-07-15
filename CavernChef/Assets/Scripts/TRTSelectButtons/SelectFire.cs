using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectFire : SelectButton
{
    void Start()
    {
        cost = (Resources.Load("FireTRT") as GameObject).GetComponent<FireTRT>().Cost();
    }

    public void select()
    {
        if (GlobalVariables.repelPoints >= cost) 
        {
            GlobalVariables.selectedTrt = (GameObject)Resources.Load("FireTRT");
            GlobalVariables.isDefensiveTRT = false;
            GlobalVariables.isOffensiveTRT = true;
        }
        // else text shows not enough RP to be implemented
    }
}
