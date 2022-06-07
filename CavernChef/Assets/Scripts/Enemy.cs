using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* TODO
Main goals:
1.) Make speed system consistent (aka change the copy pasted code in Wispy)
2.) Implement priority queue for enemyList
3.) Implement range for turret

- Copy the idea for bullet movement over for enemies
- Implement `EnemyList` class
- Implement abstract `Enemy` class
- Implement abstract 'AtkTower' class

abstract class Enemy extends MonoBehaviour
    Fields
        static float speed
        float hp
        static GameObject[] waypoints (Maybe WayPoints[]?)
        float priority
            Basically distance to walk to get to the food
            Lower value => Higher prio for turrets if in range
    Methods
        Start()
        Update()



(Impl details moved to comment in GlobalVariables.cs)
class EnemyList   (to be placed in GlobalVariables.cs)
    Fields
        Enemy[100] enemies = [null for i in range(100)]
        int numEnemies     = 0
        int numDead        = 0
    Methods
        add(Enemy nextEnemy)
        kill(Enemy deadEnemy)
        rearrange()
        findTarget(Vector2 towerPos, float range)
        reset()
        


// Wait why was I planning this again what
abstract class AtkTower extends MonoBehaviour
*/


public abstract class Enemy : MonoBehaviour
{
    public float speed;
    public float hp;
    public float dmg;   // Dmg dealt to food when in contact
    // Basically how far it is from the start; higher val -> higher prio for turrets
    public float priority;
    // Change of plans, this is how far it is from the food and a higher value means 
    // it should be targetted first
    // Measured by raw speed * number of frames

    // Unit distance between tiles cause I'm confused with how speed works
    public float unitDist;
    // Number of tiles traversed per frame
    public float tilesPerFrame;
    public float tileUnitsTraversed;

    // All enemies probably have the same waypoints
    public List<GameObject> waypoints; // = Spawner.waypointList;
    // 

    // SET IN UNITY; used so that a reference to the waypoint prefab exists

    // "public virtual void" so that it can be called in child classes through
    //     base.Start()
    public virtual void Start()
    {
        // Initialize the 4 latter fields; to be called in each individual enemy's code too
        waypoints = Spawner.waypointList;
        Vector2 pos0 = waypoints[0].transform.position;
        Vector2 pos1 = waypoints[1].transform.position;
        unitDist = Vector2.Distance(pos0, pos1);
        tilesPerFrame = speed/unitDist;
        tileUnitsTraversed = 0;
    }

    /*
    // Start is called before the first frame update
    void Start()
    {
        // waypoints = 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    */
}
