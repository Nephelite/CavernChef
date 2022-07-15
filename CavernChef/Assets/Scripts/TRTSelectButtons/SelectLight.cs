using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLight : SelectButton
{
    private float cd;
    void Start()
    {
        LightTRT lighttrt = (Resources.Load("LightTRT") as GameObject).GetComponent<LightTRT>();
        cost = lighttrt.Cost();
        cd = lighttrt.TBetPlacements();
    }

    public void select()
    {
        if (GlobalVariables.repelPoints >= cost && (LightTRT.lastPlacedTime + cd < Time.time || LightTRT.firstPlacement))
        {
            GlobalVariables.selectedTrt = (GameObject)Resources.Load("LightTRT");
            GlobalVariables.isDefensiveTRT = false;
            GlobalVariables.isOffensiveTRT = true;
            LightTRT.firstPlacement = false;
        }
        else if (GlobalVariables.repelPoints < cost)
        {
            displayErrorMessage("Not enough RP");
        }
        else if (LightTRT.lastPlacedTime + cd >= Time.time && !LightTRT.firstPlacement)
        {
            displayErrorMessage("Still on cooldown");
        }
        else
        {
            displayErrorMessage("Unknown error");
        }
    }
}
