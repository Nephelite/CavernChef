using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* "CS2030S Labs Plan.txt Style" summary
class EnemyList
    Fields
        Enemy[100] enemies
        int numSpawned
        int numDead
    Methods
        EnemyList()
        void enemySwap(i1,i2)   (Just a utility function)

        void add(Enemy nextEnemy)
        void remove(enemy deadEnemy)
        void rearrange()

        Enemy findTarget(Vector2 towerPos, float towerRange)
        List<Enemy> AoECasualties(Vector2 projPos, float AoERadius)
        List<Enemy> ChainCasualties(Enemy target, int chainLen)
        List<Enemy> ClosestTo(Vector2 targetPos)

        int aliveEnemies()
        void reset()
*/



// This class is to be used in GlobalVariables.cs

// Represents the list of enemies in the current wave
public class EnemyList
{
    // Intitialized with a size of 100
    // TODO Implement abstract class `Enemy`
    public List<Enemy> enemyList;
    // Number of enemies spawned so far
    public int numSpawned;
    // Number of killed enemies so far
    public int numDead;

    // Constructor
    public EnemyList() {
        enemyList = new List<Enemy>();
        /* Maybe I can try adding it during add instead
        for (int i = 0; i < 100; i++) {
            enemyList.Add(null);
        }
        */
        numSpawned = 0;
        numDead = 0;
    }

    // UTILY FUNCTION
    public void enemySwap(int i1, int i2) {
        Enemy temp = enemyList[i1];
        enemyList[i1] = enemyList[i2];
        enemyList[i2] = temp;
    }



    // Add another enemy into the EnemyList
    // TO-CALL whenever Spawner makes a new enemy
    public void add(Enemy nextEnemy) {
        enemyList.Add(nextEnemy);   // Add new enemy
        numSpawned += 1;            // Increment number of spawned enemies so far
    }

    // Remove an enemy from EnemyList
    // TO-CALL whenever an enemy dies (before Destroy(gameObject))
    public void remove(Enemy deadEnemy) {
        // Index of the enemy that is about to die
        /*
        int ind = numDead;
        while (enemyList[ind] != deadEnemy) {
            ind += 1;
        }
        */

        // Index of the enemy that is about to die
        int ind = enemyList.IndexOf(deadEnemy);

        enemyList[ind] = null;   // Delete reference
        enemySwap(numDead, ind);      // Shove to front
        numDead += 1;            // Increment dead counter

        // "Insertion sort" swapped enemy back in place
        // High probability that this doesn't need many swaps since an enemy at the front
        // is more likely to die first.
        while (ind > numDead && enemyList[ind].isInFrontOf(enemyList[ind-1])) {
            enemySwap(ind, ind-1);
            ind -= 1;
        }
        
    }

    // Rearrange the enemies based on their prio by insertion sort on enemyList[numDead:numSpawned]
    // TO-CALL every frame, somewhere???
    public void rearrange() {
        // Insertion sort on enemyList[L:R]
        int L = numDead;
        int R = numSpawned;

        for (int i = L+1; i < R; i++) {
            // So that i doesn't get modified
            int ind = i;

            // While not in position, shuffle left
            while (ind > L && enemyList[ind].isInFrontOf(enemyList[ind-1])) {
                enemySwap(ind, ind-1);
                ind -= 1;
            }
        }
    }



    // Finds the enemy closest within a certain radius from a certain point
    // TO-CALL whenever an offensive tower fires
    public Enemy findTarget(Vector2 towerPos, float towerRange) {
        // For each existing enemy in order
        for (int i = numDead; i < numSpawned; i++) {
            // Distnace of enemy from tower
            float dist = Vector2.Distance(towerPos, enemyList[i].transform.position);
            // If in range, return it
            if (dist <= towerRange) {
                return enemyList[i];
            }
        }
        // If nothing in range, return null
        return null;
    }

    // List of enemies that get hit by an AoE effect given the center and radius
    public List<Enemy> AoECasualties(Vector2 projPos, float AoERadius) {
        // Linearly check everything in enemyList[L:R]
        int L = numDead;
        int R = numSpawned;
        List<Enemy> ans = new List<Enemy>();

        for (int i = L; i < R; i++) {
            Enemy currEnemy = enemyList[i];                        // Current enemy
            Vector2 currEnemyPos = currEnemy.transform.position;   // Pos of current enemy
            Vector2 delta = currEnemyPos - projPos;                // Vector from proj to enemy
            float dist = delta.magnitude;                          // Dist from proj to enemy
            if (dist <= AoERadius) {                               // If splashed
                ans.Add(currEnemy);                                // Add to ans
            }
        }

        return ans;
    }

    // *****This will be defunct once lightning's behavior is changed*****
    // List of enemies that get hit by a chain effect given the start and length
    public List<Enemy> ChainCasualties(Enemy target, int chainLen) {
        int ind = enemyList.IndexOf(target);   // Index of the target
        List<Enemy> ans = new List<Enemy>();

        // Add the rest
        for (int i = 0; i < chainLen; i++) {
            // If no more enemies left, break
            if (ind >= numSpawned) {
                break;
            }
            ans.Add(enemyList[ind]);   // Add the enemy to the list
            ind += 1 ;                 // Go to the next enemy
        }

        return ans;
    }


    // List of enemies that exist sorted by distance from a target coordinate
    public List<Enemy> ClosestTo(Vector2 center) {
        // Copy the existing enemies
        List<Enemy> enemies = new List<Enemy>();
        for (int i = numDead; i < numSpawned; i++) {
            enemies.Add(enemyList[i]);
        }

        // Get the distance of each existing enemy to the center
        List<float> dists = new List<float>();
        for (int i = numDead; i < numSpawned; i++) {
            Enemy currEnemy = enemyList[i];                   // Current enemy
            Vector2 currPos = currEnemy.transform.position;   // Current position
            Vector2 currDelta = currPos - center;             // Vector diff
            float currDist = currDelta.magnitude;             // Distance
            dists.Add(currDist);                              // Append to dists
        }

        // Perform insertion sort
        int n = enemies.Count;
        for (int ind = 1; ind < n; ind++) {
            int i = ind;                               // Index of term to slide left
            while (i > 0 && dists[i-1] > dists[i]) {   // While there's a need to slide left
                // Swap in ans
                Enemy tempEnemy = enemies[i];
                enemies[i] = enemies[i-1];
                enemies[i-1] = tempEnemy;
                // Swap in dists
                float tempDist = dists[i];
                dists[i] = dists[i-1];
                dists[i-1] = tempDist;
                // Decrement i
                i -= 1;
            }
        }

        // Return the ans
        return enemies;
    }



    // Number of spawned non-dead enemies currently
    public int aliveEnemies() {
        return numSpawned - numDead;
    }

    // Sets the fields back to default
    // TO-CALL at the start of each wave
    public void reset() {
        /*
        for (int i = 0; i < 100; i++) {
            enemyList[i] = null;
        }
        */
        enemyList = new List<Enemy>();
        numSpawned = 0;
        numDead = 0;
    }
}
