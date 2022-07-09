using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTRT : AtkTower
{
    // Set in Unity, contains a reference to a basic bullet
    public GameObject OrangeLazer;
    public GameObject OrangeLazerBack;

    // Remember the enemy that was last attacked; used to determine if 
    // OrangeLazer or OrangeLazerBack should be sent
    public Enemy lastTarget;

    // Start is called before the first frame update
    void Start()
    {
        //Call upgrade apply methods

        // Set cost, tBetAtks, and range in Unity

        // Initialize CD and towerPos
        cooldown = 0.0f;
        towerPos = (Vector2)gameObject.transform.position;

        // Initialize the last target
        lastTarget = null;

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
            // Initialize bulle tto be fireed
            GameObject bullet;

            if (target != lastTarget)   // If current target is diff
            {
                // Create a new bullet head
                bullet = Instantiate(OrangeLazer, (Vector2) gameObject.transform.position, Quaternion.identity) as GameObject;
            }
            else   // If current target is still the same
            {
                // Extend lazer by using headless component
                bullet = Instantiate(OrangeLazerBack, (Vector2) gameObject.transform.position, Quaternion.identity) as GameObject;
            }

            // Update lastTarget
            lastTarget = target;

            // Extract the LazerBeam script
            LazerBeam lazer = bullet.GetComponent<LazerBeam>();
            
            // Calculate the trajectory of the lazer
            Vector2 targetPos = target.transform.position;   // Position of the target
            Vector2 traj = targetPos - towerPos;             // Vector from tower to target
            Vector2 unitTraj = traj/traj.magnitude;          // Unit movement of the lazer beam

            // Set the unit vector for movement of the lazer
            lazer.unitTraj = unitTraj;
        }
        // Else, nothing to do


        // Correct the orientation
        base.LookAtEnemy();
    }
}
