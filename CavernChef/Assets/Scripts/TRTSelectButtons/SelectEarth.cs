using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectEarth : MonoBehaviour
{
    public int cost;
    public void select()
    {
        if (GlobalVariables.repelPoints >= cost) 
        {
            GlobalVariables.selectedTrt = (GameObject)Resources.Load("EarthTRT");
            GlobalVariables.isDefensiveTRT = false;
            GlobalVariables.isOffensiveTRT = true;
        }
        // else text shows not enough RP to be implemented
    }
}
