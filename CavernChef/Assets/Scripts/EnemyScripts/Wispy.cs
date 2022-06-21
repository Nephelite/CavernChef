using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// So that I can use Math.Floor()
using System;

/* "CS2030S Labs Plan.txt Style" Summary
class EnemyStatus
    Fields
        float base_hp
        float base_speed
        float base_dmg
        float hp
        etc (probably not needed outside)
    Methods
        EnemyStatus(hp, speed, dmg)
        float getSpeed()
        float getHp()
        void updateStatus()
            Updates frame_count's to decrease by 1 if >0

        bool isDead()
        bool isSnowed()
        bool isStunned()
        bool isWet()

        void snow(int frame_count)
        void stun(int frame_count)
        void wet(int frame_count)

        void basicDmg(float bullet_dmg)
        void earthDmg(float bullet_dmg)
        void electricDmg(float bullet_dmg)
        void fireDmg(float bullet_dmg)
        void snowDmg(float bullet_dmg)
        void waterDmg(float bullet_dmg)
*/

public class Wispy : Enemy
{
    Animator animator;
    bool isDestroyed = false;
    
    /* Fields in `Enemy`, latter 4 are set in base.Start()
    public EnemyStatus status
    public float base_speed;
    public float base_hp;
    public float base_dmg;
    public float priority;
    public static List<GameObject> waypoints;
    */
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        // **************** SET BASE_HP,SPD,DMG IN UNITY ***************************************************************
        status = new EnemyStatus(base_hp, base_centi_speed, base_dmg);

        // Set waypoints and priority
        base.setup();
    }

    void destroyer()
    {
        Destroy(gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        if (!isDestroyed)
        {
            animator.SetFloat("Hp", status.currHp());

            // If dead
            if (status.isDead())
            {
                GlobalVariables.enemyList.remove(gameObject.GetComponent<Enemy>());   // Remove reference
                // speed = 0f;
                isDestroyed = true;
                Invoke("destroyer", 0.5f);
            }
            else //If alive
            {
                this.movementUpdate();
                /*
                // Get the speed given the current status
                float curr_speed = status.currSpeed();

                // Update priority (dist from start)
                priority += curr_speed;

                // Number of waypoints passed
                int floorTilesTraversed = (int)Math.Floor(priority);
                // Fraction of current segment traversed
                float decimalTilesTraversed = priority - floorTilesTraversed;


                // If at last waypoint
                if (floorTilesTraversed >= waypoints.Count - 2)
                {
                    // TODO Deal dmg=1.0f dmg to the food TODO then destroy the Wispy
                    GlobalVariables.enemyList.remove(gameObject.GetComponent<Enemy>());
                    Destroy(gameObject);
                }

                // Waypoints the enemy should be in between rn
                GameObject prevWaypoint = waypoints[floorTilesTraversed];
                GameObject nextWaypoint = waypoints[floorTilesTraversed + 1];

                // Get the position between above waypoints
                Vector3 startPos = prevWaypoint.transform.position;
                Vector3 endPos = nextWaypoint.transform.position;
                base.checkForStall(prevWaypoint, nextWaypoint);

                prevWaypoint.GetComponent<WaypointInternals>().enemiesComingToThisWaypoint.RemoveAll(x => x.Equals(gameObject)); //How does .Equals() work in C# and Unity?
                nextWaypoint.GetComponent<WaypointInternals>().enemiesComingToThisWaypoint.Add(gameObject);

                gameObject.transform.position = Vector2.Lerp(startPos, endPos, decimalTilesTraversed);
                */
            }
        }
    }
}





// Before comment cleaning on 2022-6-14 night/2022-6-15 morning

/*
public class Wispy : Enemy
{
    Animator animator;
    bool isDestroyed = false;
    
    /* Fields in `Enemy`, latter 4 are set in base.Start()
    public EnemyStatus status
    public float speed;
    public float hp;
    public float dmg;
    public float priority;
    public float unitDist;
    public float tilesPerFrame;
    public float tileUnitsTraversed;
    public static List<GameObject> waypoints;
    * /
    
    

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        // **************** SET BSAE_HP,SPD,DMG IN UNITY ***************************************************************
        status = new EnemyStatus(base_hp, base_speed, base_dmg);

        // Set waypoints and priority
        base.setup();

        // Below 4 stuff moved to Enemy.Start(), and will be set in base.Start()

        // waypoints = Spawner.waypointList;
        // Distance between two consecutive tiles cause I'm confused
        // unitDist = Vector2.Distance(waypoints[0], waypoints[1]);
        // tiles per frame
        // tilesPerFrame = speed/unitDist;
        // tileUnitsTraversed = 0;

        // lastWaypointSwitchTime = Time.time;

        // Stuff TODO probably:
        /*
        1.) Collide with projectile logic
        2.) Reach the food logic
        * /
    }

    void destroyer()
    {
        Destroy(gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        if (!isDestroyed)
        {
            animator.SetFloat("Hp", status.currHp());

            // If dead
            if (status.isDead())
            {
                GlobalVariables.enemyList.remove(gameObject.GetComponent<Enemy>());   // Remove reference
                // speed = 0f;
                isDestroyed = true;
                Invoke("destroyer", 0.5f);
            }
            else //If alive
            {
                // Get the speed given the current status
                float curr_speed = status.currSpeed();

                // Update priority (dist from start)
                priority += curr_speed;
                // Tile units traversed
                // tileUnitsTraversed += tilesPerFrame;

                // Number of waypoints passed
                int floorTilesTraversed = (int)Math.Floor(priority);
                // Fraction of current segment traversed
                float decimalTilesTraversed = priority - floorTilesTraversed;


                // If at last waypoint
                if (floorTilesTraversed >= waypoints.Count - 2)
                {
                    // TODO Deal dmg=1.0f dmg to the food TODO then destroy the Wispy
                    GlobalVariables.enemyList.remove(gameObject.GetComponent<Enemy>());
                    Destroy(gameObject);
                }

                // Waypoints the enemy should be in between rn
                GameObject prevWaypoint = waypoints[floorTilesTraversed];
                GameObject nextWaypoint = waypoints[floorTilesTraversed + 1];

                Vector3 startPos = prevWaypoint.transform.position;
                Vector3 endPos = nextWaypoint.transform.position;

                gameObject.transform.position = Vector2.Lerp(startPos, endPos, decimalTilesTraversed);
                //







            }
        }
    }
}
*/
