using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesAndUnlocks : MonoBehaviour
{
    private GameObject food;
    public static int firstUnlock = -1, secondUnlock = -1;

    void Start()
    {
        food = RandomSelectionScript.lastChosenFood;
        List<int> unlocks = food.GetComponent<FoodProperties>().obtainUnlocks();
        if (unlocks != null)
        {
            firstUnlock = unlocks[0];
            RunManager.seenTRTs[firstUnlock] = true;
            if (unlocks.Count > 1)
            {
                secondUnlock = unlocks[1];
                RunManager.seenTRTs[secondUnlock] = true;
            }
        }
    }
}
