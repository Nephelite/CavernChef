using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectEcon : SelectButton
{
    private float cd;
    void Start()
    {
        EconTRT econtrt = (Resources.Load("EconTRT") as GameObject).GetComponent<EconTRT>();
        cost = econtrt.Cost();
        cd = econtrt.TBetPlacements();
    }

    public void select()
    {
        if (GlobalVariables.repelPoints >= cost && (EconTRT.lastPlacedTime + cd < Time.time || EconTRT.firstPlacement))
        {
            GlobalVariables.selectedTrt = (GameObject)Resources.Load("EconTRT");
            GlobalVariables.isDefensiveTRT = false;
            GlobalVariables.isOffensiveTRT = true;
            EconTRT.firstPlacement = false;
        }
        else if (GlobalVariables.repelPoints < cost)
        {
            displayErrorMessage("Not enough RP");
        }
        else if (EconTRT.lastPlacedTime + cd >= Time.time && !EconTRT.firstPlacement)
        {
            displayErrorMessage("Still on cooldown");
        }
        else
        {
            displayErrorMessage("Unknown error");
        }
    }
}
