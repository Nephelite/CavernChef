using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For Math.Max
using System;

/* TODO
Implement class to encapsulate enemy status (name to be decided)
Include stuff like
    hp
    dmg
    speed
    status effects
So that it'll be easier to handle possible status effects

Fix up the 
*/



/* Enemy Status Class

*/

public class EnemyStatus
{
    // FIELDS ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    // Base stats of the enemy
    public float base_hp;
    public float base_speed;
    public float base_dmg;

    // Current hp of the enemy
    public float hp;

    // Frame counter for various status effects
    public int snow_frames;
    public int stun_frames;
    public int wet_frames;

    // Multipliers of status effects
    // THESE CAN BE EDITED
    public const float snow_spd_mult = 0.5f;
    public const float water_spd_mult = 0.9f;
    public const float snow_fire_dmg_mult = 1.5f;
    public const float water_fire_dmg_mult = 0.8f;
    public const float electric_water_dmg_mult = 2.0f;




    // Utility function, -= 1 if >0, =0 if =0
    // Pass by reference cause yeh
    public void decrementRef(ref int x) {
        if (x > 0) {
            x -= 1; 
        } else {
            x = 0;
        }
    }



    // Constructor
    public EnemyStatus(float hp, float speed, float dmg) {
        this.base_hp = hp;
        this.base_speed = speed;
        this.base_dmg = dmg;

        this.hp = hp;
        this.snow_frames = 0;
        this.stun_frames = 0;
        this.wet_frames = 0;
    }


    // SPEED AND STATUS ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    // Get current speed; call every framr
    public float speed() {
        float speed = base_speed;
        // Stun => 0 speed
        if (stun_frames > 0) {
            speed *= 0;
        } else if (isWet()) {
            speed *= water_spd_mult;
        }
        if (isSnowed()) {
            speed *= snow_spd_mult;
        }
        return speed;
    }
    // Update counters relating to the enemy; call every frame
    public void updateStatus() {
        decrementRef(ref snow_frames);
        decrementRef(ref stun_frames);
        decrementRef(ref wet_frames);
    }
    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~



    // BOOLEAN CHECKS ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    // Ded?
    public bool isDead() { return hp <= 0; }
    // Snowed?
    public bool isSnowed() { return snow_frames > 0; }
    // Stunned?
    public bool isStunned() { return stun_frames > 0; }
    // Wet?
    public bool isWet() { return wet_frames > 0; }
    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~



    // APPLYING STATUS EFFECTS ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    // (In general, durations don't stack but can overlap) ~~~~~~~~~~~~~~~~~~~~
    // Snow the enemy; yes snow is a verb now
    public void snow(int frame_count) {
        snow_frames = Math.Max(snow_frames, frame_count);
    }
    // Stun the enemy
    public void stun(int frame_count) {
        stun_frames = Math.Max(stun_frames, frame_count);
    }
    // Wet the enemy
    public void wet(int frame_count) {
        wet_frames = Math.Max(wet_frames, frame_count);
    }
    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~



    // DEALING DAMAGE ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    // Deal basic dmg to an enemy
    public void basicDmg(float bullet_dmg) {
        hp -= bullet_dmg;
    }
    // Deal earth dmg to an enemy
    public void earthDmg(float bullet_dmg) {
        // No special dmg interaction for now
        hp -= bullet_dmg;
    }
    // Deal electric dmg to an enemy
    public void electricDmg(float bullet_dmg) {
        float dmg = bullet_dmg;
        if (isWet()) {
            dmg *= electric_water_dmg_mult;
        }
        hp -= dmg;
    }
    // Deal fire damage to an enemy
    public void fireDmg(float bullet_dmg) {
        float dmg = bullet_dmg;
        if (isSnowed()) {
            dmg *= snow_fire_dmg_mult;
        }
        if (isWet()) {
            dmg *= water_fire_dmg_mult;
        }
        hp -= dmg;
    }
    // Deal snow dmg to an enemy
    public void snowDmg(float bullet_dmg) {
        // No special interaction for now
        hp -= bullet_dmg;
    }
    // Deal water dmg to an enemy
    public void waterDmg(float bullet_dmg) {
        // No special interaction for now
        hp -= bullet_dmg;
    }
    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
}


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
