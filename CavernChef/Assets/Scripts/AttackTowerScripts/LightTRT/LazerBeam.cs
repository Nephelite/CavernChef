using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
pointed up
angle theta
toa
tan(theta) = -x/y
theta = arctan(-x/y)
*/

public class LazerBeam : Projectile
{
    // Unit vector for movement; set in LightTRT.cs
    internal Vector2 unitTraj;
    // Movement per frame of the bullet
    internal Vector2 frameTraj;

    // Distance to travel before despawning
    internal float travelLimit = 45.0f;
    // Number of frames before despawning
    internal int framesLimit;
    // Frames since creation
    internal int framesPassed;

    // Enemies that have been hit so far; used to prevent double hitting
    internal List<Enemy> hit;
    
    // *AoERadius will be used here to handle the "is enemy near enough lazer to geet hit" interactions



    // Start is called before the first frame update
    void Start()
    {
        // Set speed
        // base.Setup();

        // Calculate frameTraj
        frameTraj = unitTraj * speed;

        // Calculate the framesLimit
        float framesThatAreWillBeNeededForTraveling = travelLimit/speed;
        framesLimit = (int) framesThatAreWillBeNeededForTraveling + 2;   // +2 for allowance

        // Initialize framesPassed and hit
        framesPassed = 0;
        hit = new List<Enemy>();

        // TODO Orient the lazer properly TODO
        float x = unitTraj.x;
        float y = unitTraj.y;
        float angle = Mathf.Atan(-x/y) * Mathf.Rad2Deg;
        gameObject.transform.Rotate(0,0,angle);
    }



    // Update is called once per frame
    void Update()
    {
        // If at the end, destroy object
        if (framesPassed >= framesLimit) {
            Destroy(gameObject);
            return;
        }

        framesPassed += 1;                            // Update framesPassed
        gameObject.transform.position += (Vector3) frameTraj;   // Update position

        // List of enemies near enough the lazer to get grazed by it
        List<Enemy> possibleHits = GlobalVariables.enemyList.AoECasualties(gameObject.transform.position, AoeRadius);

        // Do the necessary steps for each enemy
        foreach (Enemy enemy in possibleHits) {
            if (!hit.Contains(enemy)) {       // If enemy hasn't been hit by this lazer segment yet
                enemy.status.lightDmg(dmg);   // Deal dmg to the enemy
                hit.Add(enemy);               // Add enemy to the list of enemies hit by this lazer segment
            }
        }
    }
}
