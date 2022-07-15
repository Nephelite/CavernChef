using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockageTRT : DefensiveTRT
{
    public static GameObject spawner;
    public EnemyTile enemyTile;
    public int base_cost; //Pudding and Jelly will have different hp values and costs and potentially cooldowns.
    public float base_tBetPlacements;

    // TRT stats modifiers (modify on upgrade)
    // Absolute mods on base TRT stats
    internal static int cost_abs_delta = 0;
    internal static float tBetPlacements_abs_delta = 0;
    // Multiplier mods on base TRT stats
    internal static float cost_mult = 1;
    internal static float tBetPlacements_mult = 1;

    public static float lastPlacedTime;
    public static bool firstPlacement;

    // Additive mods
    public void AddCost(int delta)
    {
        cost_abs_delta += delta;
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
    public void MultTimeBetPlacements(float multiplier)
    {
        tBetPlacements_mult *= multiplier;
    }


    // Methods to calculate modified TRT stats
    public int Cost()
    {
        return (int)((base_cost + cost_abs_delta) * cost_mult);
    }
    public float TBetPlacements()
    {
        return (base_tBetPlacements + tBetPlacements_abs_delta) * tBetPlacements_mult;
    }

    public override void Reset()
    {
        cost_abs_delta = 0;
        tBetPlacements_abs_delta = 0;

        cost_mult = 1;
        tBetPlacements_mult = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Call upgrade apply methods

        bool isBlockageSuccessful = spawner.GetComponent<Spawner>().newPath();
        for (int i = 0; i < spawner.transform.childCount; i++)
        {
            isBlockageSuccessful = isBlockageSuccessful && spawner.transform.GetChild(i).GetComponent<Enemy>().findNewPath();
        }

        if (isBlockageSuccessful)
        {
            spawner.GetComponent<Spawner>().assignPaths();
            for (int i = 0; i < spawner.transform.childCount; i++)
            {
                spawner.transform.GetChild(i).GetComponent<Enemy>().assignNewPath();
            }

            GlobalVariables.repelPoints -= Cost();
            lastPlacedTime = Time.time;
        }
        else
        {
            Debug.Log("Either a spawner or an enemy cannot reach the food with this blockage");
            Destroy(gameObject);
            enemyTile.isBlockage = false;
            //Some error text display animation
        }

    }
}
