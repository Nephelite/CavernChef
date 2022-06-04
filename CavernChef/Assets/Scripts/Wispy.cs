using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wispy : MonoBehaviour
{
    /* Wispy Properties */
    public static float speed = 5.0f;   // Wispy fast
    // Currently a float in case fractional dmg is a thing ig
    public float hp = 10.0f;   // Wispy fragile

    /* Wispy Movement */
    // [HideInInspector]
    public GameObject[] waypoints;
    private int currentWaypoint = 0;
    private float lastWaypointSwitchTime;
    //public float speed = wispSpeed;


    // Start is called before the first frame update
    void Start()
    {
        lastWaypointSwitchTime = Time.time;

        // Stuff TODO probably:
        /*
        1.) Collide with projectile logic
        2.) Reach the food logic
        */
    }

    // Update is called once per frame
    void Update()
    {
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
            if (currentWaypoint < waypoints.Length - 2)  //Not yet at the end
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
            */

            // TODO: deduct bsae hp
            // TODO: 
            }
        }
        // FOLLOWING THE PATH (End)

    }
}
