using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* TENTATIVE TOWER IDEAS

Earth - nuke style attacks with slight stun
Snow - aoe slow (50%) for <duration> (aoe might be implemented later instead of now)
Water - enemies become wet for <duration>, slightly slowed, stacks with snow
Electric - Chain up to <max>, slight stun, extra dmg to wet enemies (chain might be implemented later instead of now)
Fire - aoe dmg, if hits snowed/watered enemy will remove snow/water effect but deal extra dmg (aoe might be implemented later instead of now)
        logic for water dealing extra dmg I mean boiling water idk LMAO why not 
        Actually maybe just make it have a minor dmg reduction idk can just tweak values in `Enemy` class
Light - the line dmg tower (this would be a fcking pain so def not now)




(Just theorizing, not yet for now at least)
Earth tower upgrades:
    Significant dmg increase, stun only minimal
Snow tower upgrades:
    increase aoe radius
    maybe minimal dmg increase ig
    increase snowed duration
    (Increasing slow effect would require convoluted logic so maybe fixing it at 50% slow is better)
Water tower upgrades:
    minimal dmg increase, water tower is meant more to complement the other towers
    increase wet duration


Plasma tower could also work as the "Basic TRT"
*/


public abstract class AtkTower : TRT
{
    // Set in Unity, contains a reference to the projectile of the tower
    public GameObject Projectile;

    // Base TRT stats (to set in Unity)
    // Cost of the turret
    public int base_cost;
    // Time between attacks
    public float base_tBetAtks;
    // Range
    public float base_range;
    // Time between placements
    public float base_tBetPlacements;

    // Position of the turret
    internal Vector2 towerPos;
    // CD till next atk
    internal float firingCD;
    // CD till next placement
    internal float placementCD;


    // Fields from the Projectiles so that they're all in 1 spot
    // Speed of the projectile (SET IN UNITY)
    public float proj_base_centi_speed;   // This is in centiunits per frame
    // Dmg of the projectile (SET IN UNITY)
    public float proj_base_dmg;
    // AoE radius, if any
    public float proj_base_AoeRadius;
    // Effect duration, if any (snow, water)
    public int proj_base_effectFrames;
    // Argument of the trajectory of the projectile (SET IN UNITY to og arg of sprite)
    public float proj_arg;

    

    /* Addition is done before multiplication here
    For example:
        base dmg = 5
        +1 dmg and x1.2 dmg
        final dmg = (5+1)*1.2 = 7.2
    */

    // Abstract methods to modify TRT modifiers
    // Additive mods
    public abstract void AddCost(int delta);
    public abstract void AddTimeBetAtks(float delta);
    public abstract void AddRange(float delta);
    public abstract void AddTimeBetPlacements(float delta);
    // Multiplicative mods
    public abstract void MultCost(float multiplier);
    public abstract void MultTimeBetAtks(float multiplier);
    public abstract void MultRange(float multiplier);
    public abstract void MultTimeBetPlacements(float multiplier);

    // Abstract methods to modify Proj modifiers
    // Additive mods
    public abstract void AddCentiSpeed(float delta);
    public abstract void AddDmg(float delta);
    public abstract void AddAoeRadius(float delta);
    public abstract void AddEffectFrames(int delta);
    // Multilicative mods
    public abstract void MultCentiSpeed(float multiplier);
    public abstract void MultDmg(float multiplier);
    public abstract void MultAoeRadius(float multiplier);
    public abstract void MultEffectFrames(float multiplier);

    // Abstract methods to calculate modified TRT stats
    //public abstract int Cost(); //Moved to TRT class
    public abstract float TBetAtks();
    public abstract float Range();
    public abstract float TBetPlacements();

    // Abstract methods to calculate modified projectile stats
    public abstract float ProjSpeed();
    public abstract float ProjDmg();
    public abstract float ProjAoeRadius();
    public abstract int ProjEffectFrames();

    // Start method for standard TRT behavior
    public void StandardStart() {
        // Initialize CD and towerPos
        firingCD = 0.0f;
        towerPos = (Vector2) gameObject.transform.position;

        // Deduct RP
        GlobalVariables.repelPoints -= Cost();
    }

    // Update method for standard TRT behavior
    // Returns a copy of the projectile fired for access in case additional setup is needed
    public GameObject StandardUpdate() {
        // Find a target
        Enemy target = GlobalVariables.enemyList.findTarget(towerPos, Range());
        // Projectile fired by the TRT in the current frame if it exists; put outside else {} due to scoping pains
        GameObject firedProj = null;   // null cause UnAsSiGnEd ErRoR

        if (firingCD > 0)   // If reloading
        {
            firingCD -= Time.deltaTime;
        }
        else if (target != null)   // If ready to fire and target in range
        {
            // Create a projectile to fire
            firedProj = Instantiate(Projectile, (Vector2) towerPos, Quaternion.identity) as GameObject;
            // Get the projectile's script
            Projectile firedProjScript = firedProj.GetComponent<Projectile>();
            // Setup the standard fields of a projectile
            SetupProjectile(firedProjScript, target);
            // Reset the firingCD
            firingCD = TBetAtks();
        }
        // Else, nothing to do

        // Correct the orientation
        LookAtEnemy(target);

        return firedProj;
    }
    
    // Sets up the fields of a projectile using fields stored here and a given target
    // Additional setup will be needed for special projectiles
    public void SetupProjectile(Projectile proj, Enemy target) {
        proj.speed = ProjSpeed();
        proj.dmg = ProjDmg();
        proj.AoeRadius = ProjAoeRadius();
        proj.effectFrames = ProjEffectFrames();

        proj.arg = this.proj_arg;

        proj.target = target;
        proj.targetPos = target.transform.position;
    }


    // Orients the TRT to face the enemy
    protected void LookAtEnemy(Enemy targetToLookAt)
    {
        if (targetToLookAt != null)
        {
            Transform target = targetToLookAt.transform;
            float angle = 0;
            Vector3 relative = transform.InverseTransformPoint(target.position);
            angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
            transform.Rotate(0, 0, -angle);
        }
    }
}

/*
    // Base TRT stats (to set in Unity)
    // Cost of the turret
    public int base_cost;
    // Time between attacks
    public float base_tBetAtks;
    // Range
    public float base_range;

    // TRT stats modifiers (modify on upgrade)
    // Absolute mods on base TRT stats
    internal static int cost_abs_delta = 0;
    internal static float tBetAtks_abs_delta = 0;
    internal static float range_abs_delta = 0;
    // Multiplier mods on base TRT stats
    internal static float cost_mult = 1;
    internal static float tBetAtks_mult = 1;
    internal static float range_mult = 1;

    // Position of the turret
    internal Vector2 towerPos;
    // CD till next atk
    internal float cooldown;



    // Fields from the Projectiles so that they're all in 1 spot
    // Speed of the projectile (SET IN UNITY)
    public static float proj_base_centi_speed;   // This is in centiunits per frame
    // Dmg of the projectile (SET IN UNITY)
    public static float proj_base_dmg;
    // AoE radius, if any
    public static float proj_base_AoeRadius;
    // Effect duration, if any (snow, water)
    public static int proj_base_effectFrames;

    // Projectile stats modifiers
    // Absolute mods on base proj stats
    internal static float proj_centi_speed_abs_delta = 0;
    internal static float proj_dmg_abs_delta = 0;
    internal static float proj_AoeRadius_abs_delta = 0;
    internal static int proj_effectFrames_abs_delta = 0;
    // Multiplier mods on base proj stats
    internal static float proj_centi_speed_mult = 1;
    internal static float proj_dmg_mult = 1;
    internal static float proj_AoeRadius_mult = 1;
    internal static float proj_effectFrames_mult = 1;

    // Argument of the trajectory of the projectile (SET IN UNITY to og arg of sprite)
    public float proj_arg;



    // Methods to modify TRT modifiers
    public static void AddCost(int delta) {
        cost_abs_delta += delta;
    }
    public static void AddTimeBetAtks(float delta) {
        tBetAtks_abs_delta += delta;
    }
    public static void AddRange(float delta) {
        range_abs_delta += delta;
    }
    public static void MultCost(float multiplier) {
        cost_mult *= multiplier;
    }
    public static void MultTimeBetAtks(float multiplier) {
        tBetAtks_mult *= multiplier;
    }
    public static void MultRange(float multiplier) {
        range_mult *= multiplier;
    }

    // Methods to calculate modified TRT stats
    public static int Cost() {
        return (int)((base_cost + cost_abs_delta) * cost_mult);
    }
    public static float TBetAtks() {
        return (base_tBetAtks + tBetAtks_abs_delta) * tBetAtks_mult;
    }
    public static float Range() {
        return (base_range + range_abs_delta) * range_mult;
    }



    // Methods to modify Proj modifiers
    public static void AddCentiSpeed(float delta) {
        proj_centi_speed_abs_delta += delta;
    }
    public static void AddDmg(float delta) {
        proj_dmg_abs_delta += delta;
    }
    public static void AddAoeRadius(float delta) {
        proj_AoeRadius_abs_delta += delta;
    }
    public static void AddEffectFrames(int delta) {
        proj_effectFrames_abs_delta += delta;
    }
    public static void MultCentiSpeed(float multiplier) {
        proj_centi_speed_mult *= multiplier;
    }
    public static void MultDmg(float multiplier) {
        proj_dmg_mult *= multiplier;
    }
    public static void MultAoeRadius(float multiplier) {
        proj_AoeRadius_mult *= multiplier;
    }
    public static void MultEffectFrames(float multiplier) {
        proj_effectFrames_mult *= multiplier;
    }

    // Methods to calculate modified projectile stats
    public static float ProjSpeed() {
        return (proj_base_centi_speed + proj_centi_speed_abs_delta) * proj_centi_speed_mult / 100;
    }
    public static float ProjDmg() {
        return (proj_base_dmg + proj_dmg_abs_delta) * proj_dmg_mult;
    }
    public static float ProjAoeRadius() {
        return (proj_base_AoeRadius + proj_AoeRadius_abs_delta) * proj_AoeRadius_mult;
    }
    public static int ProjEffectFrames() {
        return (int)((proj_base_effectFrames + proj_effectFrames_abs_delta) * proj_effectFrames_mult);
    }
*/