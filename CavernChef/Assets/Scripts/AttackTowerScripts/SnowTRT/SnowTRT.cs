using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowTRT : AtkTower
{
    // Set in Unity, contains a reference to a basic bullet
    public GameObject Snowball;

    // Slow duration and dmg to be put into the created bullet

    // Start is called before the first frame update
    void Start()
    {
        //Call upgrade apply methods

        // Set cost, tBetAtks, and range in Unity

        // Initialize CD and towerPos
        cooldown = 0.0f;
        towerPos = (Vector2) gameObject.transform.position;

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
            // Fire a bullet
            GameObject bullet = Instantiate(Snowball, (Vector2) gameObject.transform.position, Quaternion.identity) as GameObject;
            // Set the bullet's target
            bullet.GetComponent<Snowball>().target = target;
            // Reset the cooldown
            cooldown = tBetAtks;
        }
        // Else, nothing to do


        // Correct the orientation
        base.LookAtEnemy();
    }
}

