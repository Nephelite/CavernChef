using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSnow : MonoBehaviour
{
    public int cost;
    public void select()
    {
        if (GlobalVariables.repelPoints >= cost) 
        {
            GlobalVariables.selectedTrt = (GameObject)Resources.Load("SnowTRT");
        }
        // else text shows not enough RP to be implemented
    }
}
