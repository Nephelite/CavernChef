using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesAndUnlocks : MonoBehaviour
{
    public List<GameObject> TRTs = new List<GameObject>();
    private GameObject food;
    public static int firstUnlock = -1, secondUnlock = -1;

    void Start()
    {
        food = RandomSelectionScript.lastChosenFood;
        List<int> unlocks = food.GetComponent<FoodProperties>().obtainUnlocks();
        Debug.Log(food.GetComponent<FoodProperties>().foodID);
        if (unlocks != null)
        {
            firstUnlock = unlocks[0];
            if (unlocks.Count > 1)
            {
                secondUnlock = unlocks[1];
            }
        }
        Debug.Log("Unlocks: " + firstUnlock + " " + secondUnlock);

        for (int i = 0; i < 10; i++)
        {
            Debug.Log("ID " + i + ": " + RunManager.accessibleButtonsSaveData[i].ToString());
        }
    }
}
