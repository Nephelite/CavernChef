using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBlockage : SelectButton
{
    private float cd;
    void Start()
    {
        BlockageTRT blockagetrt = (Resources.Load("BlockageTRT") as GameObject).GetComponent<BlockageTRT>();
        cost = blockagetrt.Cost();
        cd = blockagetrt.TBetPlacements();
    }

    public void select()
    {
        if (GlobalVariables.repelPoints >= cost && (BlockageTRT.lastPlacedTime + cd < Time.time || BlockageTRT.firstPlacement))
        {
            GlobalVariables.selectedTrt = (GameObject)Resources.Load("BlockageTRT");
            GlobalVariables.isDefensiveTRT = true;
            GlobalVariables.isOffensiveTRT = false;
            BlockageTRT.firstPlacement = false;
        }
        else if (GlobalVariables.repelPoints < cost)
        {
            displayErrorMessage("Not enough RP");
        }
        else if (BlockageTRT.lastPlacedTime + cd >= Time.time && !BlockageTRT.firstPlacement)
        {
            displayErrorMessage("Still on cooldown");
        }
        else
        {
            displayErrorMessage("Unknown error");
        }
    }
}
