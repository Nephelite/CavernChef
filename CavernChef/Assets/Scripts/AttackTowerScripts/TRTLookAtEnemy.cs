using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// NOTE
// No longer in use; now a method in AtkTower.cs
// Leaving it here for now



/*
public class TRTLookAtEnemy : AtkTower
{
    // Update is called once per frame
    void Update()
    {
        //Aiming script
        //This script aims at the only generated wisp for now

        Vector2 towerPos = gameObject.transform.position;

        // Getting the range by *magic* (hopefully)
        // float theTheRange = gameObject.GetComponent<SnowTRT>().range;

        Enemy targetToLookAt = GlobalVariables.enemyList.findTarget(towerPos, Range());

        if (targetToLookAt != null)
        {
            //transform.Rotate(new Vector3(0, 0, 50));
            // Transform target = GlobalVariables.enemyList[0].transform;
            //gameObject.transform.LookAt(target);

            /*
THIS IS SPECIFICALLY FOR THE BASIC TURRET ONLY AS OF RN
* /
            Transform target = targetToLookAt.transform;
            float angle = 0;
            Vector3 relative = transform.InverseTransformPoint(target.position);
            angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
            transform.Rotate(0, 0, -angle);

            // Will comment this out for now (Bryce, 2022-6-15)
            // Debug.Log(transform.rotation.x + " " + transform.rotation.y + " " + transform.rotation.z);
            

            /*
            if (targetToLookAt != null) // If an enemy is in range
            { 
                
            }
            * /
        }
    }
}
*/
