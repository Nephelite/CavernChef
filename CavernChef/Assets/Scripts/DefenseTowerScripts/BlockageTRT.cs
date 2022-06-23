using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockageTRT : DefensiveTRT
{
    public int cost;
    public static GameObject spawner; //Temp
    //Cooldown?

    // Start is called before the first frame update
    void Start()
    {
        GlobalVariables.repelPoints -= cost;
        spawner.GetComponent<Spawner>().newPath();
        for (int i = 0; i < spawner.transform.childCount; i++)
        {
            Debug.Log("Enemy finding new path");
            spawner.transform.GetChild(i).GetComponent<Enemy>().findNewPath();
        }
    }
}
