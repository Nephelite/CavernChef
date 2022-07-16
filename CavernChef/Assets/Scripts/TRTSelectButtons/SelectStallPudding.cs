using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectStallPudding : SelectButton
{
    public static bool checkReady;

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

    void Update()
    {
        if (checkReady && Time.time > cd + StallTRT.lastPlacedTime)
        {
            checkReady = false;
            displayDeployMessage();
        }
    }
}
