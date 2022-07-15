using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectElectric : SelectButton
{
    private float cd;
    void Start()
    {
        ElectricTRT electrictrt = (Resources.Load("ElectricTRT") as GameObject).GetComponent<ElectricTRT>();
        cost = electrictrt.Cost();
        cd = electrictrt.TBetPlacements();
    }

    public void select()
    {
        if (GlobalVariables.repelPoints >= cost && (ElectricTRT.lastPlacedTime + cd < Time.time || ElectricTRT.firstPlacement))
        {
            GlobalVariables.selectedTrt = (GameObject)Resources.Load("ElectricTRT");
            GlobalVariables.isDefensiveTRT = false;
            GlobalVariables.isOffensiveTRT = true;
            ElectricTRT.firstPlacement = false;
        }
        else if (GlobalVariables.repelPoints < cost)
        {
            displayErrorMessage("Not enough RP");
        }
        else if (ElectricTRT.lastPlacedTime + cd >= Time.time && !ElectricTRT.firstPlacement)
        {
            displayErrorMessage("Still on cooldown");
        }
        else
        {
            displayErrorMessage("Unknown error");
        }
    }
}
