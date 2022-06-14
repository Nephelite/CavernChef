using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For Math.Max
using System;



/* "CS2030S Labs Plan.txt Style" Summary
class EnemyStatus (moved to the bottom)
    Fields
        float base_hp
        float base_speed
        float base_dmg
        float hp
        etc (probably not needed outside)
    Methods
        EnemyStatus(hp, speed, dmg)
        float currSpeed()
        float currHp()
        void updateStatus()
            Updates frame_count's to decrease by 1 if >0

        bool isDead()
        bool isSnowed()
        bool isStunned()
        bool isWet()

        void snow(int frame_count)
        void stun(int frame_count)
        void wet(int frame_count)

        void basicDmg(float bullet_dmg)
        void earthDmg(float bullet_dmg)
        void electricDmg(float bullet_dmg)
        void fireDmg(float bullet_dmg)
        void snowDmg(float bullet_dmg)
        void waterDmg(float bullet_dmg)
*/



public abstract class Enemy : MonoBehaviour
{
    public EnemyStatus status;

    // For setting in unity; will be put into status
    public float base_speed;
    public float base_hp;
    public float base_dmg;   // Dmg dealt to food

    // Distance from the start; higher val -> higher prio for turrets
    public float priority;

    // Waypoints to path the enemy (from Kevin's A Star pathfinding)
    public List<GameObject> waypoints;



    // "public virtual void" so that it can be called in child classes through
    //     base.Start()
    public virtual void setup()
    {
        // Initialize the waypoints and priority
        waypoints = Spawner.waypointList;
        priority = 0;
    }
}




// Summary of EnemyStatus at the top

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
    public const float water_fire_dmg_mult = 0.9f;
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
    public float currSpeed() {
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
    public float currHp() {
        return hp;
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
            snow_frames = 0;
        }
        if (isWet()) {
            dmg *= water_fire_dmg_mult;
            wet_frames = 0;
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





// Enemy class before comment cleaning on 2022-6-14 night/2022-6-15 morning

/*
public abstract class Enemy : MonoBehaviour
{
    public EnemyStatus status;

    // For setting in unity; will be put into status
    public float base_speed;
    public float base_hp;
    public float base_dmg;   // Dmg dealt to food when in contact

    // Basically how far it is from the start; higher val -> higher prio for turrets
    // Is also equal to the number of tiles traversed
    public float priority;
    // Measured by raw speed * number of frames

    /*
    // Unit distance between tiles cause I'm confused with how speed works
    public float unitDist;
    // Number of tiles traversed per frame
    public float tilesPerFrame;
    public float tileUnitsTraversed;
    * /

    // All enemies probably have the same waypoints
    public List<GameObject> waypoints; // = Spawner.waypointList;
    // 

    // SET IN UNITY; used so that a reference to the waypoint prefab exists

    // "public virtual void" so that it can be called in child classes through
    //     base.Start()
    public virtual void setup()
    {
        // Initialize the waypoints and priority
        waypoints = Spawner.waypointList;
        priority = 0;

        /*
        // Initialize the 4 latter fields; to be called in each individual enemy's code too
        waypoints = Spawner.waypointList;
        Vector2 pos0 = waypoints[0].transform.position;
        Vector2 pos1 = waypoints[1].transform.position;
        unitDist = Vector2.Distance(pos0, pos1);
        tilesPerFrame = speed/unitDist;
        tileUnitsTraversed = 0;
        * /
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
    * /
}
*/
