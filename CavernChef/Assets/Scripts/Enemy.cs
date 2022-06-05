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
    // Speed for each enemy type is consistent
    public static float speed;
    // Hp is per instance
    public float hp;
    // All enemies probably have the same waypoints
    public static GameObject[] waypoints;
    // Basically how close it is to the food; lower val -> higher prio for turrets
    public float priority;

    // SET IN UNITY; used so that a reference to the waypoint prefab exists



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
