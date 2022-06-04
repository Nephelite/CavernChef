using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// using System.Runtime.dll;

/*
class EnemyList   (to be placed in GlobalVariables.cs)
    Fields
        Enemy[100] enemies = [null for i in range(100)]
        int numSpawned     = 0
            Tracks right border of where rearrange() should sort
        int numDead        = 0
            Tracks left border of where rearrange() should sort
    Methods
        swap(i1, i2)
            Utility function to swap enemyList[i1] and enemyList[i2]
        add(Enemy nextEnemy)
            Call in spawner when a new enemy is released
            Increment `numSpawned`
            Add enemy to `enemies`
        remove(Enemy deadEnemy)
            Call right before destroying `deadEnemy` in `deadEnemy`'s script
            Shove it over to the left of `enemies`
            Increment `numDead`
        rearrange()
            Rearrange based on prio
            Use insertion sort, which is fass for almost sorted lists
            Call every frame (TODO decide where to call)
        findTarget(Vector2 towerPos, float range)
            Call whenever a tower fires; finds the enemy in `enemyList` with 
            the highest prio within a given range from a given position.
            Returns null if there is no enemy in range.
        count()
            number of nonded enemies
        reset()
            Call at the start/end (TODO decide where) of each wave
            Sets all fields back to defeault values
*/


// Represents the list of enemies in the current wave
public class EnemyList
{
    // Intitialized with a size of 100
    // TODO Implement abstract class `Enemy`
    public Enemy[] enemyList;
    // Number of enemies spawned so far
    public int numSpawned;
    // Number of killed enemies so far
    public int numDead;

    // Constructor
    public EnemyList() {
        enemyList = new Enemy[100];
        numSpawned = 0;
        numDead = 0;
    }

    // UTILY FUNCTION
    public void swap(int i1, int i2) {
        (enemyList[i1],enemyList[i2]) = (enemyList[i2],enemyList[i1]);
    }

    // Add another enemy into the EnemyList
    // TO-CALL whenever Spawner makes a new enemy
    public void add(Enemy nextEnemy) {
        enemyList[numSpawned] = nextEnemy;   // Add new enemy
        numSpawned += 1;                     // Increment number of spawned enemies so far
    }

    // Remove an enemy from EnemyList
    // TO-CALL whenever an enemy dies (before Destroy(gameObject))
    public void remove(Enemy deadEnemy) {
        // Index of the enemy that is about to die
        int ind = numDead;
        while (enemyList[ind] != deadEnemy) {
            ind += 1;
        }

        enemyList[ind] = null;   // Delete reference
        swap(numDead, ind);      // Shove to front
        numDead += 1;            // Increment dead counter

        // "Insertion sort" swapped enemy back in place
        while (ind > numDead && enemyList[ind].priority < enemyList[ind-1].priority) {
            swap(ind, ind-1);
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
            while (ind > L && enemyList[ind].priority < enemyList[ind-1].priority) {
                swap(ind, ind-1);
                ind -= 1;
            }
        }
    }

    // Finds the enemy with the highest priority within a certain radius from a certain point
    // TO-CALL whenever an offensive tower fires
    public Enemy findTarget(Vector2 towerPos, float towerRange) {
        // For each existing enemy in order
        for (int i = numDead; i < numSpawned; i++) {
            // If in range, return it
            if (Vector2.Distance(towerPos, enemyList[i].transform.position) <= towerRange) {
                return enemyList[i];
            }
        }
        // If nothing in range, return null
        return null;
    }

    // Number of spawned non-dead enemies currently
    public int count() {
        return numSpawned - numDead;
    }

    // Sets the fields back to default
    // TO-CALL at the start of each wave
    public void reset() {
        for (int i = 0; i < 100; i++) {
            enemyList[i] = null;
        }
        numSpawned = 0;
        numDead = 0;
    }
}


public class GlobalVariables : MonoBehaviour
{
    public static int repelPoints;
    public static GameObject selectedTrt;
    public static List<GameObject> Grid = new List<GameObject>();
    /* POSSIBLE FUTURE OPTIMIZATION
    In the worst case, you could spawn the whole wave before killing anything
    and popping the front element repeatedly would be O(n^2)
    O(n) can be done with self-resizing array and 2 pointers but not now (and possibly not at all)
    */
    public static List<GameObject> enemyList = new List<GameObject>();



    // NEWMARKER
    // Leaving the old enemyList and testing on enemy_list for noew cause there's a 
    // good chance that something's gonna break
    public static EnemyList enemy_list = new EnemyList();



    void Update() 
    {
        // With the new `EnemyList`:tm: this should be done within `Enemy` or it's subclass instead
        if (enemyList.Count > 0 && enemyList[0] == null)
        {
            enemyList.RemoveAt(0);
        }
    }
    /*
    void Update()
    {
        if (selectedTrt != null && Input.GetKeyDown(KeyCode.Mouse0))
        {
            int x_pos = (int)Input.mousePosition.x;
            int y_pos = (int)Input.mousePosition.y;

            //if (x_pos >= 39 && x_pos <= 938 && y_pos >= 257.5 && y_pos <= 565.5) 
            //{
                Debug.Log("Registered Input");
                GameObject block = Instantiate(selectedTrt, Input.mousePosition, Quaternion.identity) as GameObject;
                block.transform.SetParent(this.transform);
                Grid.Add(block);
            //}

            selectedTrt = null;
        }
    }
    */
}
