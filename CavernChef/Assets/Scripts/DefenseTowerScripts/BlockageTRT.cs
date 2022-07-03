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

            GlobalVariables.repelPoints -= cost;
        }
        else
        {
            Debug.Log("Either a spawner or an enemy cannot reach the food with this blockage");
            Destroy(gameObject);
            //Some error text display animation
        }

    }
}
