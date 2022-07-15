using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectWater : SelectButton
{
    private float cd;
    void Start()
    {
        WaterTRT watertrt = (Resources.Load("WaterTRT") as GameObject).GetComponent<WaterTRT>();
        cost = watertrt.Cost();
        cd = watertrt.TBetPlacements();
    }

    public void select()
    {
        if (GlobalVariables.repelPoints >= cost && (WaterTRT.lastPlacedTime + cd < Time.time || WaterTRT.firstPlacement))
        {
            GlobalVariables.selectedTrt = (GameObject)Resources.Load("WaterTRT");
            GlobalVariables.isDefensiveTRT = false;
            GlobalVariables.isOffensiveTRT = true;
            WaterTRT.firstPlacement = false;
        }
        else if (GlobalVariables.repelPoints < cost)
        {
            displayErrorMessage("Not enough RP");
        }
        else if (FireTRT.lastPlacedTime + cd >= Time.time && !WaterTRT.firstPlacement)
        {
            displayErrorMessage("Still on cooldown");
        }
        else
        {
            displayErrorMessage("Unknown error");
        }
    }
}
