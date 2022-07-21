using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For math methods
using System;

public class ReworkedLazer : Projectile
{
    // Dmg and arg fields already exist in Projectile

    internal LineRenderer lr;     // Reference to the line renderer
    internal GameObject start;    // Empty GameObject to mark the start of the lazer
    internal GameObject end;      // Empty GameObject to mark the end of the lazer
    internal Vector2 startPos;    // start.transform.position
    internal Vector2 endPos;      // end.transform.position

    // Determine lazer position (along with Projectile.arg)
    internal Vector2 center;      // Coincides with the position of the TRT
    internal Vector2 unitTraj;    // Trajectory of the lazer with unit length
    internal float r_0 = 0.25f;   // Radius of the dead zone around the TRT
    internal float r_1 = 34.0f;   // Radius of the farthest reaches of the lazer

    internal float reach;         // Half the width of the lazer
    internal bool on;             // Is it on?

    

    public bool IsOn() {
        return this.on;
    }

    // Turn on with a given target
    public void TurnOn(Enemy target) {
        this.on = true;
        Vector2 targetPos = target.transform.position;
        Vector2 traj = targetPos - center;
        unitTraj = traj/traj.magnitude;
    }

    // Turn off
    public void TurnOff() {
        this.on = false;
    }

    // Sets the lazer to the endpoints defined by start and end
    public void SnapToEndpoints() {
        lr.SetPosition(0, start.transform.position);
        lr.SetPosition(1, end.transform.position);
    }

    // Positions the lazer based on the current value of Projectile.arg 
    // and whether the lazer is on or not
    public void PositionLazer() {
        // Update endpoints based on whether it's on or off
        if (IsOn()) {
            // Get position wrt center of rotation
            Vector2 startPos_delta = unitTraj * r_0;
            Vector2 endPos_delta = unitTraj * r_1;
            // Get position wrt the actual Scene
            startPos = center + startPos_delta;
            endPos = center + endPos_delta;
            // Set position of empty game objects start and end
            start.transform.position = startPos;
            end.transform.position = endPos;
        } else {   // Elif off
            // Shove it to the corner
            startPos = new Vector2(0,0);
            endPos = new Vector2(0,0);
            start.transform.position = startPos;
            end.transform.position = endPos;
        }
        // Snap the lazer to the endpoints
        SnapToEndpoints();
    }

    // Deal dmg to all the enemies that are within reach of the lazer 
    // if the lazer is currently on
    public void DealDmg() {
        if (IsOn()) {
            List<Enemy> toHit = GlobalVariables.enemyList.LineCasualties(startPos, endPos, reach);
            for (int i = 0; i < toHit.Count; i++) {
                Enemy currEnemy = toHit[i];
                currEnemy.status.lightDmg(dmg);
            }
        }
        // Else, nothing to do
    } 


    // Start is called before the first frame update
    void Start()
    {
        // Initialize fields
        // dmg set by TRT
        lr = gameObject.GetComponent<LineRenderer>();
        start = new GameObject("start");
        end = new GameObject("end");
        startPos = new Vector2(0,0);
        endPos = new Vector2(0,0);
        start.transform.position = startPos;
        end.transform.position = endPos;

        // center set by TRT
        // unitTraj has no value to set yet
        // reach set by TRT
        on = false;


        // Setup Line Renderer
        lr.positionCount = 2;
        lr.startWidth = 2 * reach;
        lr.endWidth = 2 * reach;
        
        SnapToEndpoints();
    }

    // Update is called once per frame
    void Update()
    {
        PositionLazer();
        DealDmg();
    }
}
