using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTRT : AtkTower
{
    // Set in Unity, contains a reference to a basic bullet
    //public GameObject LightLaser;

    // Start is called before the first frame update
    void Start()
    {
        // Set the fields
        cost = 300;
        tBetAtks = 2.0f;
        range = 10.0f;
        cooldown = 0.0f;
        towerPos = (Vector2)gameObject.transform.position;

        // Deduct RP
        GlobalVariables.repelPoints -= cost;
    }

    // Update is called once per frame
    void Update()
    {
        // Find a target
        Enemy target = GlobalVariables.enemyList.findTarget(towerPos, range);

        if (cooldown > 0)   // If reloading
        {
            cooldown -= Time.deltaTime;
        }
        else if (target != null)   // If ready to fire and target in range
        {
            /* TODO implement LazerBeam
            */
        }
        // Else, nothing to do
    }
}
