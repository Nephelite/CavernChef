using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricTRT : AtkTower
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

        proj_chainLen = 3;
    }


    // Extra fields for Lightning
    // Number of frames to jump between 2 enemies (SET IN UNITY)
    public int proj_framesPerJump;
    // Maximum length of a chain of a lightning
    public static int proj_chainLen = 3;
    // Maximum length of a jump between 2 enemies (SET IN UNITY)
    public float proj_jumpDist;

    public void AddChain()
    {
        proj_chainLen++;
    }


    // Start is called before the first frame update
    void Start()
    {
        StandardStart();
        lastPlacedTime = Time.time;
        firstPlacement = false;
        SelectElectric.checkReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject lightning = StandardUpdate();
        
        // Set the extra fields for lightning if lightning was made
        if (lightning != null) {
            Lightning lightningScript = lightning.GetComponent<Lightning>();
            lightningScript.framesPerJump = proj_framesPerJump;
            lightningScript.chainLen = proj_chainLen;
            lightningScript.jumpDist = proj_jumpDist;
        }
        
        /*
        // Find a target
        Enemy target = GlobalVariables.enemyList.findTarget(towerPos, Range());

        if (cooldown > 0)   // If reloading
        {
            cooldown -= Time.deltaTime;
        }
        else if (target != null)   // If ready to fire and target in range
        {
            // Fire a bullet
            GameObject bullet = Instantiate(Lightning, (Vector2) gameObject.transform.position, Quaternion.identity) as GameObject;
            // Set the bullet's target
            bullet.GetComponent<Lightning>().target = target;
            // Reset the cooldown
            cooldown = TBetAtks();
        }
        // Else, nothing to do


        // Correct the orientation
        base.LookAtEnemy(target);
        */
    }
}

