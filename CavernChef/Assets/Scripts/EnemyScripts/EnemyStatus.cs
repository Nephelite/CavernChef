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
        EnemyStatus(hp, centi_speed, dmg)
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

// Handles interactions regarding hp, speed, dmg, and status effects
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
    public EnemyStatus(float hp, float centi_speed, float dmg) {
        this.base_hp = hp;
        this.base_speed = centi_speed / 100;
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

    //STALLING ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //Methods to set speed to 0, and restore it
    public float stop()
    {
        float speed = base_speed;
        base_speed = 0;
        return speed;
    }

    public void restoreSpeed(float speed)
    {
        base_speed = speed;
    }
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
