using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// So that I can use Math.Floor()
using System;

public class Wispy : Enemy
{
    /*

    /* Wispy Properties * /
    public static float speed = 5.0f;   // Wispy fast
    // Currently a float in case fractional dmg is a thing ig
    public float hp = 10.0f;   // Wispy fragile

    /* Wispy Movement * /
    // [HideInInspector]
    public List<GameObject> waypoints;
    private int currentWaypoint = 0;
    private float lastWaypointSwitchTime;
    //public float speed = wispSpeed;

    */

    
    /* Fields in `Enemy`, latter 4 are set in base.Start()
    public float speed;
    public float hp;
    public float dmg;
    public float priority;
    public float unitDist;
    public float tilesPerFrame;
    public float tileUnitsTraversed;
    public static List<GameObject> waypoints;
    */
    
    

    // Start is called before the first frame update
    void Start()
    {
        

        speed = 0.1f;
        hp = 10.0f;
        dmg = 1.0f;   // TENTATIVE
        priority = 0;

        // Set waypoints, unitDist, and tilesPerFrame
        base.Start();

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
        */
    }

    // Update is called once per frame
    void Update()
    {
        // If dead
        if (hp <= 0)
        {
            GlobalVariables.enemyList.remove(gameObject.GetComponent<Enemy>());   // Remove reference
            Destroy(gameObject);
        }


        // Farther from the start by `speed`
        priority += speed;
        // Tile units traversed
        tileUnitsTraversed += tilesPerFrame;

        // Number of waypoints passed
        int floorTileUnitsTraversed = (int) Math.Floor(tileUnitsTraversed);
        // Fraction of current segment traversed
        float decimalTileUnitsTraversed = tileUnitsTraversed - floorTileUnitsTraversed;


        // If at last waypoint
        if (floorTileUnitsTraversed >= waypoints.Count - 2)
        {
            // TODO Deal dmg=1.0f dmg to the food TODO then destroy the Wispy
            GlobalVariables.enemyList.remove(gameObject.GetComponent<Enemy>());
            Destroy(gameObject);   
        }

        // Waypoints the enemy should be in between rn
        GameObject prevWaypoint = waypoints[floorTileUnitsTraversed];
        GameObject nextWaypoint = waypoints[floorTileUnitsTraversed+1];

        Vector3 startPos = prevWaypoint.transform.position;
        Vector3 endPos = nextWaypoint.transform.position;

        gameObject.transform.position = Vector2.Lerp(startPos, endPos, decimalTileUnitsTraversed);
        //
        

        



        // TODO
        /* How I did it last time for Basic Bullet
            Vector2 traj = enemyPos - bulletPos;                 // Trajectory
            float dist = traj.magnitude;   
                Vector2 delta = traj * speed / dist;
                gameObject.transform.position += (Vector3) delta;//Type cast needed since .trans.pos is 3-dim
        */





        /*

        // FOLLOWING THE PATH (Start)
        // 1 
        Vector3 startPosition = waypoints[currentWaypoint].transform.position;     // Prev waypoint's coord
        Vector3 endPosition = waypoints[currentWaypoint + 1].transform.position;   // Next waypoint's coord

        // 2 
        float pathLength = Vector3.Distance(startPosition, endPosition);   // dist(prev waypt, next waypt)
        float totalTimeForPath = pathLength / speed;                       // time from prev to next waypt
        float currentTimeOnPath = Time.time - lastWaypointSwitchTime;      // time on curr segment so far
        // Lerp(X,Y,frac) moves the thing to the point `frac` of the way along the vector \vec{XY}
        gameObject.transform.position = Vector2.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);

        // 3 
        if (gameObject.transform.position.Equals(endPosition))   // Reached next waypt
        {
            if (currentWaypoint < waypoints.Count - 2)  //Not yet at the end
            {
                // 3.a 
                currentWaypoint++;                    // Next segment
                lastWaypointSwitchTime = Time.time;   // Reset last switch time
                // TODO: Rotate into move direction (not really req rn, some games 
                // like The Creeps don't even do this)
            }
            else   //At the end
            {
                // 3.b 
                Destroy(gameObject);

            /*
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
            * /

            // TODO: deduct bsae hp
            // TODO: 
            }
        }
        // FOLLOWING THE PATH (End)

        */

    }
}
