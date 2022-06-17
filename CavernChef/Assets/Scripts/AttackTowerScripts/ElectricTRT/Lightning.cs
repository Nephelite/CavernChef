using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO Add chaining of zaps (comment added 2022-6-16)

public class Lightning : Projectile
{
    /* Stats as of 2022-6-
    centi_speed = 
    dmg = 
    effect_frames = 
    */

    // Start is called before the first frame update
    void Start()
    {
        // Set speed
        base.Setup();
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
            if (target != null) {   // if target is not yet dead
                // Deal damage (possibly AoE)
                target.status.electricDmg(dmg);
                // Apply status effect (if any; possibly AoE)
                target.status.stun(effect_frames);
            }
            Destroy(gameObject);   // Destroy the bullet itself
        }
        else   // Elif not yet hit
        {
            Vector2 delta = traj * speed / dist;                // Movement
            gameObject.transform.position += (Vector3) delta;   // Update coordinates
        }
    }
}
