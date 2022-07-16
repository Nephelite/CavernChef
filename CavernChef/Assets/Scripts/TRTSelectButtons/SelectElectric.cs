using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectElectric : SelectButton
{
    public static bool checkReady;

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

    void Update()
    {
        if (checkReady && Time.time > cd + ElectricTRT.lastPlacedTime)
        {
            checkReady = false;
            displayDeployMessage();
        }
    }
}
