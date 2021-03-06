using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectEarth : SelectButton
{
    public static bool checkReady;

    void Start()
    {
        EarthTRT earthtrt = (Resources.Load("EarthTRT") as GameObject).GetComponent<EarthTRT>();
        cost = earthtrt.Cost();
        cd = earthtrt.TBetPlacements();
    }

    public void select()
    {
        if (GlobalVariables.repelPoints >= cost && (EarthTRT.lastPlacedTime + cd < Time.time || EarthTRT.firstPlacement))
        {
            GlobalVariables.selectedTrt = (GameObject)Resources.Load("EarthTRT");
            GlobalVariables.isDefensiveTRT = false;
            GlobalVariables.isOffensiveTRT = true;
        }
        else if (GlobalVariables.repelPoints < cost)
        {
            displayErrorMessage("Not enough RP");
        }
        else if (EarthTRT.lastPlacedTime + cd >= Time.time && !EarthTRT.firstPlacement)
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
        if (checkReady && Time.time > cd + EarthTRT.lastPlacedTime)
        {
            checkReady = false;
            displayDeployMessage();
        }
    }
}
