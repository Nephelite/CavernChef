using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO Add AoE (probably noticeable radius) (comment added 2022-6-16)

public class Snowball : Projectile
{
    /* Stats as of 2022-6-
    centi_speed = 
    dmg = 
    effectFrames = 
    */

    // Start is called before the first frame update
    void Start()
    {
        // Set speed
        base.Setup();
        FindObjectOfType<AudioManager>().Play("Snow");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 projectilePos = gameObject.transform.position;   // Bullet position
        if (target != null) {                                    // If target is not yet dead,
            targetPos = target.transform.position;               // Update targetPos
        }
        Vector2 traj = targetPos - projectilePos;                // Trajectory to target
        float dist = traj.magnitude;                             // Dist to target

        if (dist < speed)   // If target is hit
        {
            // Get list of enemies hit by the AoE
            List<Enemy> hit = GlobalVariables.enemyList.AoECasualties(targetPos, AoeRadius);
            // Hit every hit enemy
            foreach (Enemy enemy in hit)
            {
                // Deal damage
                enemy.status.snowDmg(dmg);
                // Apply status effect
                enemy.status.snow(effectFrames);
            }
            /*
            if (target != null) {   // if target is not yet dead
                // Deal damage (possibly AoE)
                target.status.snowDmg(dmg);
                // Apply status effect (if any; possibly AoE)
                target.status.snow(effectFrames);
            }
            */
            Destroy(gameObject);   // Destroy the bullet itself
        }
        else   // Elif not yet hit
        {
            Vector2 delta = traj * speed / dist;                // Movement
            gameObject.transform.position += (Vector3) delta;   // Update coordinates
        }
    }
}
