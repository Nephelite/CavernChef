using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRTBasicOffense : TRT
{
    public float atkVal;
    public int GetValue => cost;


    // Set in Unity, contains a reference to a basic bullet
    public GameObject BasicBullet;
    // Time between attacks; can be set here or in Unity
    public float tBetAtks = 2.0f;
    // Range
    public float range = 7.5f;
    // Counts down the time till next atk
    public float countdown;
    // Position of the turret
    private Vector2 pos;

    //public List<GameObjects> enemyList; //Might want to make this a priority queue, priority based on distance to travel to reach target. For now priority is based on time added.

    // Start is called before the first frame update
    void Start()
    {
        GlobalVariables.repelPoints -= cost;
        countdown = 0.0f;
        pos = (Vector2) gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        /* Plan
        1.) Have a countdown for time bet attacking
        2.) countdown <= 0 implies ready for an atk
        3.) Create a bullet object when doing the attack and deal 
            with that there

        Code flow:
        if enemy doesn't exist
            if countdown > 0
                still in cd from last atk, dec as normal
            elif countdown <= 0
                pass
        elif enemy exists
            if countdown > 0
                decrement cooldown
            elif countdown <= 0
                Atk time bois instantiate(bullet)
                reset countdown to t_bet_atk
        
        Other related Scripts to do:
         - Bullet script for this turret
        */

        // Find a target
        Vector2 towerPos = gameObject.transform.position;
        Enemy target = GlobalVariables.enemyList.findTarget(towerPos, range);

        if (target == null)   // If no enemy in range
        {
            if (countdown > 0)   // Reloading
            {
                countdown -= Time.deltaTime;
            }
            // Else do nothing
        }
        else   // If enemies exist
        {
            if (countdown > 0)   // Reloading
            {
                countdown -= Time.deltaTime;
            }
            else   // Ready to fire
            {
                // Fire a bullet
                GameObject bullet = Instantiate(BasicBullet, (Vector2) gameObject.transform.position, Quaternion.identity) as GameObject;
                // Set the bullet's target
                bullet.GetComponent<TRTBasicBullet>().target = target;
                // Reset the cooldown
                countdown = tBetAtks;
            }
        }



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
         */

    }
}
