using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectStallPudding : SelectButton
{
    private float cd;
    void Start()
    {
        StallTRT stalltrt = (Resources.Load("PuddingTRT") as GameObject).GetComponent<StallTRT>();
        cost = stalltrt.Cost();
        cd = stalltrt.TBetPlacements();
    }

    public void select()
    {
        if (GlobalVariables.repelPoints >= cost && (StallTRT.lastPlacedTime + cd < Time.time || StallTRT.firstPlacement))
        {
            GlobalVariables.selectedTrt = (GameObject)Resources.Load("PuddingTRT");
            GlobalVariables.isDefensiveTRT = true;
            GlobalVariables.isOffensiveTRT = false;
            StallTRT.firstPlacement = false;
        }
        else if (GlobalVariables.repelPoints < cost)
        {
            displayErrorMessage("Not enough RP");
        }
        else if (StallTRT.lastPlacedTime + cd >= Time.time && !StallTRT.firstPlacement)
        {
            displayErrorMessage("Still on cooldown");
        }
        else
        {
            displayErrorMessage("Unknown error");
        }
    }
}
