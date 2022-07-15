using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconTRT : TRT //Consider making this inherit from AtkTower
{
    public int value, cost;
    public float cycleDuration, cooldown;
    private float savedCycleDuration;
    public static float lastPlacedTime;
    public static bool firstPlacement;

    void Start()
    {
        //Call apply upgrades method?
        GlobalVariables.repelPoints -= cost;
        savedCycleDuration = cycleDuration;
        lastPlacedTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (cycleDuration <= 0)
        {
            Debug.Log("Producing " + value + " RP");
            GlobalVariables.repelPoints += value;
            cycleDuration = savedCycleDuration;
            FindObjectOfType<AudioManager>().Play("Econ");
        }
        else
        {
            cycleDuration -= Time.deltaTime;
        }
        
    }
}
