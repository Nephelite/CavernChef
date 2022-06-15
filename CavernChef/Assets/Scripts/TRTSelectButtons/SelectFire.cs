using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectFire : MonoBehaviour
{
    public int cost;
    public void select()
    {
        if (GlobalVariables.repelPoints >= cost) 
        {
            GlobalVariables.selectedTrt = (GameObject)Resources.Load("FireTRT");
        }
        // else text shows not enough RP to be implemented
    }
}
