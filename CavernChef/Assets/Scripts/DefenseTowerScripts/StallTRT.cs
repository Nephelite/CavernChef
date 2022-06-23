using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StallTRT : DefensiveTRT
{
    public int cost;
    public float hp; //Pudding and Jelly will have different hp values and costs and potentially cooldowns.
    //Cooldown?

    // Start is called before the first frame update
    void Start()
    {
        GlobalVariables.repelPoints -= cost;
        gameObject.name = "StallTRT(Clone)";
    }

    public void decrementHP(float atk)
    {
        this.hp -= atk;
        Debug.Log("HP Left: " + this.hp);
        if (this.hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
