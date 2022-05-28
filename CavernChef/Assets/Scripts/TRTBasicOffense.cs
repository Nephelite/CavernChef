using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRTBasicOffense : TRT
{
    public float atkVal;
    public int GetValue => cost;

    //public List<GameObjects> enemyList; //Might want to make this a priority queue, priority based on distance to travel to reach target. For now priority is based on time added.

    // Start is called before the first frame update
    void Start()
    {
        GlobalVariables.repelPoints -= cost;
    }

    // Update is called once per frame
    void Update()
    {
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
         */

    }
}
