using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricTRT : AtkTower
{
    // Set in Unity, contains a reference to a basic bullet
    public GameObject Lightning;

    // Slow duration and dmg to be put into the created bullet

    // Start is called before the first frame update
    void Start()
    {
        // Set the fields
        cost = 250;
        tBetAtks = 1.0f;
        range = 7.0f;
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
            /* TODO implement Lightning then uncomment this
            // Fire a bullet
            GameObject bullet = Instantiate(Lightning, (Vector2) gameObject.transform.position, Quaternion.identity) as GameObject;
            // Set the bullet's target
            bullet.GetComponent<Snowballt>().target = target;
            // Reset the cooldown
            cooldown = tBetAtks;
            */
        }
        // Else, nothing to do
    }
}
