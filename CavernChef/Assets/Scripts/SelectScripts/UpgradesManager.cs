using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesManager : MonoBehaviour
{
    /*
     * Nested list structure:
     * 
     * Inner List: Upgrades objects for a specific TRT
     * Outer List: All the inner lists sorted by the TRT index order
     * 
     * E.G:
     * 
     * Outer: Econ list, Fire list, Water list, Snow list, ...
     * 
     * Note: Inner list is contained in the Upgrades object
    */
    public List<Upgrades> upgradesList = new List<Upgrades>();
    private List<Upgrades> chosen = new List<Upgrades>();

    void Start()
    {
        chosen.Clear();
        List<int> nums = new List<int>();
        for (int i = 0; i < RunManager.accessibleButtonsSaveData.Length; i++)
        {
            if (RunManager.accessibleButtonsSaveData[i])
                nums.Add(i);
        }
        for (int i = 0; i < 4; i++)
        {
            int choose = nums[Random.Range(0, nums.Count - i)];
            nums.Remove(choose);
            chosen.Add(upgradesList[choose]);
            Debug.Log("Choose " + choose);
            if (nums.Count == 0)
                break;
        }
        //chosen contains the 4 Upgrades chosen at random
    }

    public Upgrades getChosen(int i)
    {
        if (i < chosen.Count)
            return chosen[i];
        else return null;
    }
}
