using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectElectric : MonoBehaviour
{
    public int cost;
    public void select()
    {
        if (GlobalVariables.repelPoints >= cost) 
        {
            GlobalVariables.selectedTrt = (GameObject)Resources.Load("ElectricTRT");
            GlobalVariables.isDefensiveTRT = false;
            GlobalVariables.isOffensiveTRT = true;
        }
        // else text shows not enough RP to be implemented
    }
}
