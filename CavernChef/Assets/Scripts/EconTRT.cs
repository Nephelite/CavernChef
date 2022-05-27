using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconTRT : MonoBehaviour
{
    public int value, cost;
    public float cycleDuration;
    private float savedCycleDuration;

    void Start()
    {
        GlobalVariables.repelPoints -= cost;
        savedCycleDuration = cycleDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (cycleDuration <= 0)
        {
            Debug.Log("Producing " + value + " RP");
            GlobalVariables.repelPoints += value;
            cycleDuration = savedCycleDuration;
        }
        else
        {
            cycleDuration -= Time.deltaTime;
        }
        
    }
}
