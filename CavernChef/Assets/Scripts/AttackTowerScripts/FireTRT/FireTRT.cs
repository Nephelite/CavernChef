using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// First Bullet created

public class FireTRT : AtkTower
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


    /*
    // Awake is called once before anything else
    // Setup static fields
    void Awake()
    {
        cost_abs_delta = 0;
        tBetAtks_abs_delta = 0;
        range_abs_delta = 0;

        cost_mult = 1;
        tBetAtks_mult = 1;
        range_mult = 1;

        proj_centi_speed_abs_delta = 0;
        proj_dmg_abs_delta = 0;
        proj_AoeRadius_abs_delta = 0;
        proj_effectFrames_abs_delta = 0;

        proj_centi_speed_mult = 1;
        proj_dmg_mult = 1;
        proj_AoeRadius_mult = 1;
        proj_effectFrames_mult = 1;
    }
    */

    // Start is called before the first frame update
    void Start()
    {
        StandardStart();
    }

    // Update is called once per frame
    void Update()
    {
        StandardUpdate();
    }
}













// Copy of the class before comments were cleaned (2022-6-14)

/*
public class FireTRT : AtkTower
{
    // public float atkVal; in FireBullet instead
    // public int GetValue => cost;

    // Set in Unity, contains a reference to a basic bullet
    public GameObject FireBullet;
    
    /* The following fiels have been moved to TRT.cs

    // Time between attacks; can be set here or in Unity
    public float tBetAtks = 2.0f;
    // Range
    public float range = 7.5f;
    // Counts down the time till next atk
    public float cooldown;
    // Position of the turret
    private Vector2 pos;
    * /

    //public List<GameObjects> enemyList; //Might want to make this a priority queue, priority based on distance to travel to reach target. For now priority is based on time added.

    // Start is called before the first frame update
    void Start()
    {
        // Set the fields
        cost = 100;
        tBetAtks = 2.0f;
        range = 7.5f;
        cooldown = 0.0f;
        towerPos = (Vector2) gameObject.transform.position;

        // Deduct RP
        GlobalVariables.repelPoints -= cost;
    }

    // Update is called once per frame
    void Update()
    {
        /* Plan
        1.) Have a cooldown for time bet attacking
        2.) cooldown <= 0 implies ready for an atk
        3.) Create a bullet object when doing the attack and deal 
            with that there

        Code flow:
        if enemy doesn't exist
            if cooldown > 0
                still in cd from last atk, dec as normal
            elif cooldown <= 0
                pass
        elif enemy exists
            if cooldown > 0
                decrement cooldown
            elif cooldown <= 0
                Atk time bois instantiate(bullet)
                reset cooldown to t_bet_atk
        
        Other related Scripts to do:
         - Bullet script for this turret
        */

        /* 2022-6-14 modification to code flow
        if cd > 0:
            decrement cd
        elif enemy exists
            shoot a projectile
            reset cd
        * /

        // Find a target
        // Vector2 towerPos = gameObject.transform.position;
        Enemy target = GlobalVariables.enemyList.findTarget(towerPos, range);

        if (cooldown > 0)   // If reloading
        {
            cooldown -= Time.deltaTime;
        }
        else if (target != null)   // If ready to fire and target in range
        {
            // Fire a bullet
            GameObject bullet = Instantiate(FireBullet, (Vector2) gameObject.transform.position, Quaternion.identity) as GameObject;
            // Set the bullet's target
            bullet.GetComponent<FireBullet>().target = target;
            // Reset the cooldown
            cooldown = tBetAtks;
        }

        /*

        if (target == null)   // If no enemy in range
        {
            if (cooldown > 0)   // Reloading
            {
                cooldown -= Time.deltaTime;
            }
            // Else do nothing
        }
        else   // If enemies exist
        {
            if (cooldown > 0)   // Reloading
            {
                cooldown -= Time.deltaTime;
            }
            else   // Ready to fire
            {
                // Fire a bullet
                GameObject bullet = Instantiate(FireBullet, (Vector2) gameObject.transform.position, Quaternion.identity) as GameObject;
                // Set the bullet's target
                bullet.GetComponent<FireBullet>().target = target;
                // Reset the cooldown
                cooldown = tBetAtks;
            }
        }

        * /



        //Attacking script
        /*
        if (enemyList.Length > 0)
        {
            GameObject target = enemyList[0];
            target.hp -= atkVal;
            if (target == null) 
            {
                enemyList.RemoveAt(0);
            }
        }
         * /

    }
}
*/