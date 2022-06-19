using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO Add chaining of zaps (comment added 2022-6-16)

/* How this works (or should work at least) as of 2022-6-20 4:43AM
Lightning fast cause speed of light yes

If it misses it's target (case the target died before it hit), then just 
destroy it for now cause I have no idea how to fix it otherwise as of now.
This shouldn;t be too frequent is the lightning speed is set to something high.

The enemies it'll chain to are determined at the moment of contact with the target.
If any of the enemies to be chained dies before they get chained on, the lightning 
does not extend 1 enemy further back. Once again, as long as the movement is set 
to be fast enough, this shouldn't make it miss too often. 

Besides, the fronter enemies should be the first ones to die anyway.
*/

public class Lightning : Projectile
{
    /* Stats as of 2022-6-
    centi_speed = 
    dmg = 
    effectFrames = 
    */

    // Retain the list of enemies to zap once the chain starts
    public List<Enemy> toZap;
    // Position of the enemies to zap; kept in case an enemy dies before being zapped
    public List<Vector2> toZapPos;
    // Number of enemies to zap
    public int numToZap;
    // Number of frames since the chain started
    public int framesSinceChainStart;
    // Has the chain started?
    public bool chainStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        // Set speed
        base.Setup();
    }

    // Update is called once per frame
    void Update()
    {
        if (chainStarted)   // If chaining
        {
            // Number of frames to move from one enemy to the next
            // With the way this code is designed, this needs to be small to not 
            // mak the electric tower sometimes trash.
            int framesPerEnemy = 3;

            // Update enemy positions
            for (int i = 0; i < numToZap; i++) {
                Enemy enemy_i = toZap[i];
                if (enemy_i != null) {
                    toZapPos[i] = enemy_i.transform.position;
                }
            }
            
            // If div by framesPerEnemy, deal the dmg and stun
            if (framesSinceChainStart%framesPerEnemy == 0) {
                Enemy toDmg = toZap[framesSinceChainStart/3];
                if (toDmg != null) {
                    toDmg.status.electricDmg(dmg);
                    toDmg.status.stun(effectFrames);
                }
            }

            // If at the end, destroy the lightning
            if (framesSinceChainStart == framesPerEnemy * (numToZap-1)) {
                Destroy(gameObject);
            }

            // Move the lightning
            framesSinceChainStart += 1;                                  // Increment framesSinceChainStart
            int hitCount = framesSinceChainStart/framesPerEnemy;         // Number of enemies chained so far
            int framesSinceZap = framesSinceChainStart%framesPerEnemy;   // Number of frames since last dmg dealt
            float fracToNextEnemy = framesSinceZap/framesPerEnemy;       // 
            Enemy currEnemy = toZap[hitCount];                           // Last enemy hit
            Enemy nextEnemy = toZap[hitCount+1];                         // Next enemy to hit
            Vector2 currEnemyPos = currEnemy.transform.position;         // Pos of last enemy hit
            Vector2 nextEnemyPos = nextEnemy.transform.position;         // Pos of next enemy to hit
            Vector2 moveTo = Vector2.Lerp(currEnemyPos, nextEnemyPos, fracToNextEnemy);   // Position between curr and next enemy for the lightning
            gameObject.transform.position = moveTo;                      // Set the desired position
        }
        else   // If not yet chaining
        {
            Vector2 projectilePos = gameObject.transform.position;   // Bullet position
            if (target != null) {                                    // If target is not yet dead,
                targetPos = target.transform.position;               // Update targetPos
            }
            Vector2 traj = targetPos - projectilePos;                // Trajectory to target
            float dist = traj.magnitude;                             // Dist to target

            if (dist < speed)   // If chaining gonna start
            {
                if (target != null) {   // if target is not yet dead
                    // Get chained enemies
                    toZap = GlobalVariables.enemyList.ChainCasualties(target, chainLen);
                    // Get the pos of every chained enemy
                    toZapPos = new List<Vector2>();
                    foreach(Enemy enemy in toZap) {
                        toZapPos.Add(enemy.transform.position);
                    }
                    numToZap = toZap.Count;      // Get number of enemies to zap
                    framesSinceChainStart = 0;   // Initialize frames since start count
                    chainStarted = true;         // Mark chain as started
                } else {
                    Destroy(gameObject);   // Destroy the lightning if the enemy somehow died ig
                }
            }
            else   // Elif not yet hit
            {
                Vector2 delta = traj * speed / dist;                // Movement
                gameObject.transform.position += (Vector3) delta;   // Update coordinates
            }
        }
        

    }
}
