using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconSelect : MonoBehaviour
{
    public void select()
    {
        GlobalVariables.selectedTrt = (GameObject)Resources.Load("EconTRT");
    }
}
