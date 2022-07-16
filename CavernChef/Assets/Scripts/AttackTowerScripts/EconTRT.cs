using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconTRT : TRT //Consider making this inherit from AtkTower
{
    public int base_value, base_cost;
    public float base_tCycle, base_tBetPlacements;

    // TRT stats modifiers (modify on upgrade)
    // Absolute mods on base TRT stats
    internal static int value_abs_delta = 0;
    internal static int cost_abs_delta = 0;
    internal static float tCycle_abs_delta = 0;
    internal static float tBetPlacements_abs_delta = 0;
    // Multiplier mods on base TRT stats
    internal static float value_mult = 1;
    internal static float cost_mult = 1;
    internal static float tCycle_mult = 1;
    internal static float tBetPlacements_mult = 1;

    private float savedCycleDuration;
    public static float lastPlacedTime;
    public static bool firstPlacement;

    // Additive mods
    public void AddValue(int delta)
    {
        value_abs_delta += delta;
    }
    public void AddCost(int delta)
    {
        cost_abs_delta += delta;
    }
    public void AddCycle(float delta)
    {
        tCycle_abs_delta += delta;
    }
    public void AddTimeBetPlacements(float delta)
    {
        tBetPlacements_abs_delta += delta;
    }
    // Multiplicative mods
    public void MultValue(float multiplier)
    {
        value_mult *= multiplier;
    }
    public void MultCost(float multiplier)
    {
        cost_mult *= multiplier;
    }
    public void MultCycle(float multiplier)
    {
        tCycle_mult *= multiplier;
    }
    public void MultTimeBetPlacements(float multiplier)
    {
        tBetPlacements_mult *= multiplier;
    }


    // Methods to calculate modified TRT stats
    public int Value()
    {
        return (int)((base_value + value_abs_delta) * value_mult);
    }
    public override int Cost()
    {
        return (int)((base_cost + cost_abs_delta) * cost_mult);
    }
    public float TCycle()
    {
        return (base_tCycle + tCycle_abs_delta) * tCycle_mult;
    }
    public float TBetPlacements()
    {
        return (base_tBetPlacements + tBetPlacements_abs_delta) * tBetPlacements_mult;
    }

    public override void Reset()
    {
        value_abs_delta = 0;
        cost_abs_delta = 0;
        tCycle_abs_delta = 0;
        tBetPlacements_abs_delta = 0;

        value_mult = 1;
        cost_mult = 1;
        tCycle_mult = 1;
        tBetPlacements_mult = 1;
    }


    void Start()
    {
        //Call apply upgrades method?
        GlobalVariables.repelPoints -= Cost();
        savedCycleDuration = TCycle();
        lastPlacedTime = Time.time;
        firstPlacement = false;
        SelectEcon.checkReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (savedCycleDuration <= 0)
        {
            Debug.Log("Producing " + Value() + " RP");
            GlobalVariables.repelPoints += Value();
            savedCycleDuration = TCycle();
            FindObjectOfType<AudioManager>().Play("Econ");
        }
        else
        {
            savedCycleDuration -= Time.deltaTime;
        }
    }
}
