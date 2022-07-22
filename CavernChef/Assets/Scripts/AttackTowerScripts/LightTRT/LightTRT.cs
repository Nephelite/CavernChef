using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTRT : AtkTower //NOTE: ATK VALUES ARE IN DDMG PER SECOND FOR THIS TRT
{
    // TRT stats modifiers (modify on upgrade)
    // Absolute mods on base TRT stats
    internal static int cost_abs_delta = 0;
    internal static float tBetAtks_abs_delta = 0;
    internal static float range_abs_delta = 0;
    internal static float tBetPlacements_abs_delta = 0;
    // Multiplier mods on base TRT stats
    internal static float cost_mult = 1;
    internal static float tBetAtks_mult = 1;
    internal static float range_mult = 1;
    internal static float tBetPlacements_mult = 1;

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

    // Time of last placed TRT of this type
    public static float lastPlacedTime;
    // Is the next placement the first TRT placement of this type in the stage
    public static bool firstPlacement;

    // Methods to modify TRT modifiers
    // Additive mods
    public override void AddCost(int delta) {
        cost_abs_delta += delta;
    }
    public override void AddTimeBetAtks(float delta) {
        tBetAtks_abs_delta += delta;
    }
    public override void AddRange(float delta) {
        range_abs_delta += delta;
    }
    public override void AddTimeBetPlacements(float delta) {
        tBetPlacements_abs_delta += delta;
    }
    // Multiplicative mods
    public override void MultCost(float multiplier) {
        cost_mult *= multiplier;
    }
    public override void MultTimeBetAtks(float multiplier) {
        tBetAtks_mult *= multiplier;
    }
    public override void MultRange(float multiplier) {
        range_mult *= multiplier;
    }
    public override void MultTimeBetPlacements(float multiplier) {
        tBetPlacements_mult *= multiplier;
    }



    // Methods to modify Proj modifiers
    // Addtive mods
    public override void AddCentiSpeed(float delta) {
        proj_centi_speed_abs_delta += delta;
    }
    public override void AddDmg(float delta) {
        proj_dmg_abs_delta += delta;
    }
    public override void AddAoeRadius(float delta) {
        proj_AoeRadius_abs_delta += delta;
    }
    public override void AddEffectFrames(int delta) {
        proj_effectFrames_abs_delta += delta;
    }
    // Multiplicative mods
    public override void MultCentiSpeed(float multiplier) {
        proj_centi_speed_mult *= multiplier;
    }
    public override void MultDmg(float multiplier) {
        proj_dmg_mult *= multiplier;
    }
    public override void MultAoeRadius(float multiplier) {
        proj_AoeRadius_mult *= multiplier;
    }
    public override void MultEffectFrames(float multiplier) {
        proj_effectFrames_mult *= multiplier;
    }



    // Methods to calculate modified TRT stats
    public override int Cost() {
        return (int)((base_cost + cost_abs_delta) * cost_mult);
    }
    public override float TBetAtks() {
        return (base_tBetAtks + tBetAtks_abs_delta) * tBetAtks_mult;
    }
    public override float Range() {
        return (base_range + range_abs_delta) * range_mult;
    }
    public override float TBetPlacements() {
        return (base_tBetPlacements + tBetPlacements_abs_delta) * tBetPlacements_mult;
    }

    // Methods to calculate modified projectile stats
    public override float ProjSpeed() {
        return (proj_base_centi_speed + proj_centi_speed_abs_delta) * proj_centi_speed_mult / 100;
    }
    public override float ProjDmg() {
        return (proj_base_dmg + proj_dmg_abs_delta) * proj_dmg_mult;
    }
    public override float ProjAoeRadius() {
        return (proj_base_AoeRadius + proj_AoeRadius_abs_delta) * proj_AoeRadius_mult;
    }
    public override int ProjEffectFrames() {
        return (int)((proj_base_effectFrames + proj_effectFrames_abs_delta) * proj_effectFrames_mult);
    }

    public override void Reset()
    {
        cost_abs_delta = 0;
        tBetAtks_abs_delta = 0;
        range_abs_delta = 0;
        tBetPlacements_abs_delta = 0;

        cost_mult = 1;
        tBetAtks_mult = 1;
        range_mult = 1;
        tBetPlacements_mult = 1;

        proj_centi_speed_abs_delta = 0;
        proj_dmg_abs_delta = 0;
        proj_AoeRadius_abs_delta = 0;
        proj_effectFrames_abs_delta = 0;

        proj_centi_speed_mult = 1;
        proj_dmg_mult = 1;
        proj_AoeRadius_mult = 1;
        proj_effectFrames_mult = 1;
    }


    // The lazer that this instance will use
    internal GameObject ReworkedLazer;
    // Script of the lazer
    internal ReworkedLazer LazerScript;

    // Lazer specific fields
    public float lazerReach;

    private Sound sfx;

    // Start is called before the first frame update
    void Start()
    {
        // Setup the Tower; setting the firingCD doesn't really matter
        StandardStart();
        lastPlacedTime = Time.time;
        firstPlacement = false;
        SelectLight.checkReady = true;

        // Setup the Lazer
        ReworkedLazer = Instantiate(Projectile);
        LazerScript = ReworkedLazer.GetComponent<ReworkedLazer>();

        sfx = FindObjectOfType<AudioManager>().GetSound("Light");

        LazerScript.dmg = ProjDmg();
        LazerScript.center = towerPos;
        LazerScript.reach = lazerReach;
    }

    // Update is called once per frame
    void Update()
    {
        // Find a target
        Enemy target = GlobalVariables.enemyList.findTarget(towerPos, Range());

        if (target != null) {   // If a target exists
            LookAtEnemy(target);
            LazerScript.TurnOn(target);
            if (sfx.source != null && !sfx.source.isPlaying)
                sfx.source.PlayOneShot(sfx.clip);
        } else {   // If a target doesn't exist
            LazerScript.TurnOff();
            if (sfx.source != null && sfx.source.isPlaying)
                sfx.source.Stop();
        }
    }


/*
    // Set in Unity, contains a reference to a basic bullet
    public GameObject OrangeLazer;
    public GameObject OrangeLazerBack;

    // Remember the enemy that was last attacked; used to determine if 
    // OrangeLazer or OrangeLazerBack should be sent
    public Enemy lastTarget;

    // Start is called before the first frame update
    void Start()
    {
        StandardStart();
        lastPlacedTime = Time.time;
        firstPlacement = false;
        SelectLight.checkReady = true;

        // Initialize the last target
        lastTarget = null;
    }

    // Update is called once per frame
    void Update()
    {
        // Find a target
        Enemy target = GlobalVariables.enemyList.findTarget(towerPos, Range());

        if (firingCD > 0)   // If reloading
        {
            firingCD -= Time.deltaTime;
        }
        else if (target != null)   // If ready to fire and target in range
        {
            // Initialize bulle tto be fireed
            GameObject bullet;

            if (target != lastTarget)   // If current target is diff
            {
                // Create a new bullet head
                bullet = Instantiate(OrangeLazer, (Vector2) gameObject.transform.position, Quaternion.identity) as GameObject;
            }
            else   // If current target is still the same
            {
                // Extend lazer by using headless component
                bullet = Instantiate(OrangeLazerBack, (Vector2) gameObject.transform.position, Quaternion.identity) as GameObject;
            }

            // Update lastTarget
            lastTarget = target;

            // Extract the LazerBeam script
            LazerBeam lazer = bullet.GetComponent<LazerBeam>();
            
            // Calculate the trajectory of the lazer
            Vector2 targetPos = target.transform.position;   // Position of the target
            Vector2 traj = targetPos - towerPos;             // Vector from tower to target
            Vector2 unitTraj = traj/traj.magnitude;          // Unit movement of the lazer beam

            // Set the unit vector for movement of the lazer
            lazer.unitTraj = unitTraj;

            // Set other fields of lazer
            lazer.speed = ProjSpeed();
            lazer.dmg = ProjDmg();
        }
        // Else, nothing to do


        // Correct the orientation
        base.LookAtEnemy(target);
    }
*/


}
