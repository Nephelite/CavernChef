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

    // Number of frames to jump between 2 enemies (SET IN UNITY)
    public int framesPerJump;
    // Maximum length of a chain of a lightning (SET IN UNITY)
    public int chainLen;
    // Maximum length of a jump between 2 enemies (SET IN UNITY)
    public float jumpDist;

    // Number of enemies that have been zapped, including null targets
    internal int numZapped;
    // True iff chaining action has started
    internal bool chaining;
    // Number of frames since the last hit
    internal int framesSinceLastHit;
    // List of enemies that have been zapped (don't zap something twice)
    internal List<Enemy> zapped;
    // Position of the last hit target
    internal Vector2 lastTargetPos;

    // Why not directly modify `target` and `targetPos`?
    /*
    // The next target to hit in the chain
    internal Enemy nextTarget;
    // Position of the next target to hit in the chain
    internal Vector2 nextTargetPos;
    */

    // Start is called before the first frame update
    void Start()
    {
        // Set speed
        base.Setup();

        // Setup fields
        numZapped = 0;
        chaining = false;
        framesSinceLastHit = 0;
        zapped = new List<Enemy>();

        // Sound FX
        FindObjectOfType<AudioManager>().Play("Electric");
    }

    // Update is called once per frame
    void Update()
    {
        // Angle projectile properly
        base.AngleTowardsTarget();

        if (chaining)
        {
            framesSinceLastHit += 1;

            if (target != null) {                        // If target not dead
                targetPos = target.transform.position;   // Update targetPos
            }

            // Move the lightning
            float fracToNextEnemy = (float)framesSinceLastHit / (float)framesPerJump;   // Get frac to next enemy
            Vector2 moveTo = Vector2.Lerp(lastTargetPos, targetPos, fracToNextEnemy);   // Find next pos
            gameObject.transform.position = moveTo;                                     // Set next pos

            if (framesSinceLastHit == framesPerJump) {   // If target hit
                framesSinceLastHit = 0;      // Reset framesSinceLastHit
                numZapped += 1;              // Increment numZapped
                lastTargetPos = targetPos;   // Update lastTargetPos

                if (target != null) {   // If target is alive
                    target.status.electricDmg(dmg);     // Deal dmg
                    target.status.stun(effectFrames);   // Stun
                    zapped.Add(target);                 // Record as zapped
                }

                if (numZapped == chainLen) {   // If maximum chain length reached
                    Destroy(gameObject);   // Destroy
                    return;                // End
                }


                // Get enemies sorted by dist to current pos
                List<Enemy> sortedEnemies = GlobalVariables.enemyList.ClosestTo(gameObject.transform.position);
                int i = 0;   // Index of next enemy to zap wrt sortedEnemies

                while (i < sortedEnemies.Count) {   // Iterate through sortedEnemies
                    if (zapped.Contains(sortedEnemies[i])) {   // If already zapped
                        i += 1;                         // Continue to next Enemy
                    } else {                            // If not yet zapped
                        break;                          // i is the desired index; break here
                    }
                }

                if (i >= sortedEnemies.Count) {   // If no unzapped enemy exists
                    Destroy(gameObject);                     // Destroy
                    return;                                  // End
                } else {                          // If an unzapped enemy exists
                    target = sortedEnemies[i];               // Update target
                    targetPos = target.transform.position;   // Update targetPos
                }
            }
        }
        else  // Not yet chaining
        {
            Vector2 projectilePos = gameObject.transform.position;   // Bullet position
            if (target != null) {                                    // If target is not yet dead,
                targetPos = target.transform.position;               // Update targetPos
            }
            Vector2 traj = targetPos - projectilePos;                // Trajectory to target
            float dist = traj.magnitude;                             // Dist to target

            if (dist < speed)   // If original target is hit
            {
                chaining = true;             // Start chaining
                numZapped += 1;              // Increment numZapped
                lastTargetPos = targetPos;   // Update lastTargetPos

                if (target != null) {   // If target is not yet dead
                    target.status.electricDmg(dmg);     // Deal dmg
                    target.status.stun(effectFrames);   // Stun
                    zapped.Add(target);                 // Add target to list of zapped enemies
                }

                // Get enemies sorted by dist to current pos
                List<Enemy> sortedEnemies = GlobalVariables.enemyList.ClosestTo(gameObject.transform.position);
                if (sortedEnemies.Count != 0) {   // If there exists at least 1 other target
                    target = sortedEnemies[0];               // Update target
                    targetPos = target.transform.position;   // Update targetPos
                } else {   // If no other target
                    Destroy(gameObject);                     // Destroy
                    return;                                  // End
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

/* Code flow for Update()

Angle towards target (target gets updated every retargeting)

if chaining:
    framesSinceLastHit += 1

    if target alive:
        update targetPos
    Move required fraction of way from lastTargetPos to targetPos

    if framesSinceChainStart == framesPerJump:
        update numZapped, lastTargetPos
        framesSinceLastHit = 0

        if target alive:
            electric dmg
            stun
            add to zapped
    
        if numZapped = chainLen:
            destroy lightning
        
        find closest alive enemy not in zapped
        if closest alive unzapped enemy exists:
            if near enough:
                update target, targetPos, lastTargetPos
            elif not near enough:
                destroy lightning
        
        

elif not yet chaining:
    Get trajectory to target

    if dist to target <= speed:
        update chaining, numZapped, lastTargetPos

        if target alive:
            electric dmg
            stun
            add to zapped

        find closest alive enemy
        if closest alive enemy exists:
            if near enough:
                update target, targetPos, lastTargetPos
            elif not near enough:
                destroy lightning
        elif no other alive enemy:
            destroy lightning
            RETURN

    elif dist to target > speed:
        if target alive:
            update targetPos
        Move along traj closer to targetPos by speed

*/















// OLD BEHAVIOR (removed 2022-7-5)
/*
    // Retain the list of enemies to zap once the chain starts
    internal List<Enemy> toZap;
    // Position of the enemies to zap; kept in case an enemy dies before being zapped
    internal List<Vector2> toZapPos;
    // Number of enemies to zap
    internal int numToZap;
    // Number of frames since the chain started
    internal int framesSinceChainStart;
    // Has the chain started?
    internal bool chainStarted = false;

    // Update is called once per frame
    void Update()
    {
        if (chainStarted)   // If chaining
        {
            // Number of frames to move from one enemy to the next
            // With the way this code is designed, this needs to be small to not 
            // mak the electric tower sometimes trash.
            int framesPerEnemy = 10;

            // Update enemy positions
            for (int i = 0; i < numToZap; i++) {
                Enemy enemy_i = toZap[i];
                if (enemy_i != null) {
                    toZapPos[i] = enemy_i.transform.position;
                }
            }
            
            // If div by framesPerEnemy, deal the dmg and stun
            if (framesSinceChainStart%framesPerEnemy == 0) {
                Enemy toDmg = toZap[framesSinceChainStart/framesPerEnemy];
                if (toDmg != null) {
                    toDmg.status.electricDmg(dmg);
                    toDmg.status.stun(effectFrames);
                }
            }

            // If at the end, destroy the lightning
            if (framesSinceChainStart == framesPerEnemy * (numToZap-1)) {
                Destroy(gameObject);
                return;
            }

            // Move the lightning
            int hitCount = framesSinceChainStart/framesPerEnemy;         // Number of enemies chained so far
            int framesSinceZap = framesSinceChainStart%framesPerEnemy;   // Number of frames since last dmg dealt
            float fracToNextEnemy = (float)framesSinceZap/ (float)framesPerEnemy;
            Enemy currEnemy = toZap[hitCount];                           // Last enemy hit
            Enemy nextEnemy = toZap[hitCount+1];                         // Next enemy to hit
            Vector2 currEnemyPos = currEnemy.transform.position;         // Pos of last enemy hit
            Vector2 nextEnemyPos = nextEnemy.transform.position;         // Pos of next enemy to hit
            Vector2 moveTo = Vector2.Lerp(currEnemyPos, nextEnemyPos, fracToNextEnemy);   // Position between curr and next enemy for the lightning
            gameObject.transform.position = moveTo;                      // Set the desired position
            framesSinceChainStart += 1;                                  // Increment framesSinceChainStart

            // Angle projectile properly
            if (nextEnemy != null) {
                base.AngleTowardsPosition(nextEnemy.transform.position);
            }
        }
        else   // If not yet chaining
        {
            // Angle projectile properly
            base.AngleTowardsTarget();

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
*/
