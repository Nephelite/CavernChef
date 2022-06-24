using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// First Bullet created

public class FireTRT : AtkTower
{
    // Set in Unity, contains a reference to a basic bullet
    public GameObject Fireball;

    // Start is called before the first frame update
    void Start()
    {
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
        /* 2022-6-14 modification to code flow
        if cd > 0:
            decrement cd
        elif enemy exists
            shoot a projectile
            reset cd
        */

        // Find a target
        Enemy target = GlobalVariables.enemyList.findTarget(towerPos, range);

        if (cooldown > 0)   // If reloading
        {
            cooldown -= Time.deltaTime;
        }
        else if (target != null)   // If ready to fire and target in range
        {
            // Fire a bullet
            GameObject bullet = Instantiate(Fireball, (Vector2) gameObject.transform.position, Quaternion.identity) as GameObject;
            // Set the bullet's target
            bullet.GetComponent<Fireball>().target = target;
            // Reset the cooldown
            cooldown = tBetAtks;
        }
        // Else, nothing to do
    }
}













// Copy of the class before comments were cleaned (2022-6-14)

/*
public class FireTRT : AtkTower
{
    // public float atkVal; in FireBullet instead
    // public int GetValue => cost;

    // Set in Unity, contains a reference to a basic bullet
    public GameObject FireBullet;
    
    /* The following fiels have been moved to TRT.cs

    // Time between attacks; can be set here or in Unity
    public float tBetAtks = 2.0f;
    // Range
    public float range = 7.5f;
    // Counts down the time till next atk
    public float cooldown;
    // Position of the turret
    private Vector2 pos;
    * /

    //public List<GameObjects> enemyList; //Might want to make this a priority queue, priority based on distance to travel to reach target. For now priority is based on time added.

    // Start is called before the first frame update
    void Start()
    {
        // Set the fields
        cost = 100;
        tBetAtks = 2.0f;
        range = 7.5f;
        cooldown = 0.0f;
        towerPos = (Vector2) gameObject.transform.position;

        // Deduct RP
        GlobalVariables.repelPoints -= cost;
    }

    // Update is called once per frame
    void Update()
    {
        /* Plan
        1.) Have a cooldown for time bet attacking
        2.) cooldown <= 0 implies ready for an atk
        3.) Create a bullet object when doing the attack and deal 
            with that there

        Code flow:
        if enemy doesn't exist
            if cooldown > 0
                still in cd from last atk, dec as normal
            elif cooldown <= 0
                pass
        elif enemy exists
            if cooldown > 0
                decrement cooldown
            elif cooldown <= 0
                Atk time bois instantiate(bullet)
                reset cooldown to t_bet_atk
        
        Other related Scripts to do:
         - Bullet script for this turret
        */

        /* 2022-6-14 modification to code flow
        if cd > 0:
            decrement cd
        elif enemy exists
            shoot a projectile
            reset cd
        * /

        // Find a target
        // Vector2 towerPos = gameObject.transform.position;
        Enemy target = GlobalVariables.enemyList.findTarget(towerPos, range);

        if (cooldown > 0)   // If reloading
        {
            cooldown -= Time.deltaTime;
        }
        else if (target != null)   // If ready to fire and target in range
        {
            // Fire a bullet
            GameObject bullet = Instantiate(FireBullet, (Vector2) gameObject.transform.position, Quaternion.identity) as GameObject;
            // Set the bullet's target
            bullet.GetComponent<FireBullet>().target = target;
            // Reset the cooldown
            cooldown = tBetAtks;
        }

        /*

        if (target == null)   // If no enemy in range
        {
            if (cooldown > 0)   // Reloading
            {
                cooldown -= Time.deltaTime;
            }
            // Else do nothing
        }
        else   // If enemies exist
        {
            if (cooldown > 0)   // Reloading
            {
                cooldown -= Time.deltaTime;
            }
            else   // Ready to fire
            {
                // Fire a bullet
                GameObject bullet = Instantiate(FireBullet, (Vector2) gameObject.transform.position, Quaternion.identity) as GameObject;
                // Set the bullet's target
                bullet.GetComponent<FireBullet>().target = target;
                // Reset the cooldown
                cooldown = tBetAtks;
            }
        }

        * /



        //Attacking script
        /*
        if (enemyList.Length > 0)
        {
            GameObject target = enemyList[0];
            target.hp -= atkVal;
            if (target == null) 
            {
                enemyList.RemoveAt(0);
            }
        }
         * /

    }
}
*/