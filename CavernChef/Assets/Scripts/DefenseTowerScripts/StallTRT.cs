using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StallTRT : DefensiveTRT
{
    public int base_cost, base_hp; //Pudding and Jelly will have different hp values and costs and potentially cooldowns.
    public float base_tBetPlacements;

    // TRT stats modifiers (modify on upgrade)
    // Absolute mods on base TRT stats
    internal static int cost_abs_delta = 0;
    internal static int hp_abs_delta = 0;
    internal static float tBetPlacements_abs_delta = 0;
    // Multiplier mods on base TRT stats
    internal static float cost_mult = 1;
    internal static float hp_mult = 1;
    internal static float tBetPlacements_mult = 1;

    private float internal_hp;
    public static float lastPlacedTime;
    public static bool firstPlacement;

    // Additive mods
    public void AddCost(int delta)
    {
        cost_abs_delta += delta;
    }
    public void AddHp(int delta)
    {
        hp_abs_delta += delta;
    }
    public void AddTimeBetPlacements(float delta)
    {
        tBetPlacements_abs_delta += delta;
    }
    // Multiplicative mods
    public void MultCost(float multiplier)
    {
        cost_mult *= multiplier;
    }
    public void MultHp(float multiplier)
    {
        hp_mult *= multiplier;
    }
    public void MultTimeBetPlacements(float multiplier)
    {
        tBetPlacements_mult *= multiplier;
    }


    // Methods to calculate modified TRT stats
    public int Cost()
    {
        return (int)((base_cost + cost_abs_delta) * cost_mult);
    }
    public float Hp()
    {
        return (base_hp + hp_abs_delta) * hp_mult;
    }
    public float TBetPlacements()
    {
        return (base_tBetPlacements + tBetPlacements_abs_delta) * tBetPlacements_mult;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Call upgrade apply methods

        GlobalVariables.repelPoints -= Cost();
        gameObject.name = "StallTRT(Clone)";
        this.internal_hp = Hp();
        lastPlacedTime = Time.time;
    }

    public void decrementHP(float atk)
    {
        this.internal_hp -= atk;
        Debug.Log("HP Left: " + this.internal_hp);
        if (this.internal_hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
