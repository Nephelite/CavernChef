using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSnow : SelectButton
{
    public static bool checkReady;

    void Start()
    {
        SnowTRT snowtrt = (Resources.Load("SnowTRT") as GameObject).GetComponent<SnowTRT>();
        cost = snowtrt.Cost();
        cd = snowtrt.TBetPlacements();
    }

    public void select()
    {
        if (GlobalVariables.repelPoints >= cost && (SnowTRT.lastPlacedTime + cd < Time.time || SnowTRT.firstPlacement))
        {
            GlobalVariables.selectedTrt = (GameObject)Resources.Load("SnowTRT");
            GlobalVariables.isDefensiveTRT = false;
            GlobalVariables.isOffensiveTRT = true;
        }
        else if (GlobalVariables.repelPoints < cost)
        {
            displayErrorMessage("Not enough RP");
        }
        else if (SnowTRT.lastPlacedTime + cd >= Time.time && !SnowTRT.firstPlacement)
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
        if (checkReady && Time.time > cd + SnowTRT.lastPlacedTime)
        {
            checkReady = false;
            displayDeployMessage();
        }
    }
}
