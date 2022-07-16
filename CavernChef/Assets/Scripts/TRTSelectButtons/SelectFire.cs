using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectFire : SelectButton
{
    public static bool checkReady;

    void Start()
    {
        FireTRT firetrt = (Resources.Load("FireTRT") as GameObject).GetComponent<FireTRT>();
        cost = firetrt.Cost();
        cd = firetrt.TBetPlacements();
    }

    public void select()
    {
        if (GlobalVariables.repelPoints >= cost && (FireTRT.lastPlacedTime + cd < Time.time || FireTRT.firstPlacement))
        {
            GlobalVariables.selectedTrt = (GameObject)Resources.Load("FireTRT");
            GlobalVariables.isDefensiveTRT = false;
            GlobalVariables.isOffensiveTRT = true;
        }
        else if (GlobalVariables.repelPoints < cost)
        {
            displayErrorMessage("Not enough RP");
        }
        else if (FireTRT.lastPlacedTime + cd >= Time.time && !FireTRT.firstPlacement)
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
        if (checkReady && Time.time > cd + FireTRT.lastPlacedTime)
        {
            checkReady = false;
            displayDeployMessage();
        }
    }
}
