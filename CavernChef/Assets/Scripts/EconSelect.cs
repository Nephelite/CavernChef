using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconSelect : MonoBehaviour
{
    public int cost;
    public void select()
    {
        if (GlobalVariables.repelPoints >= cost)
        {
            GlobalVariables.selectedTrt = (GameObject)Resources.Load("EconTRT");
        }
        // else text shows not enough RP to be implemented
    }
}
