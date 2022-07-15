using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Upgrades
{
    public string name;
    public GameObject projectile; //dmg/AoE/speed/special
    public GameObject TRT; //else
    public List<string> upgrades = new List<string>();
    public List<float> upgradeValues = new List<float>();
    public List<bool> areValuesFlat = new List<bool>();

    //Here, include various trt-specific upgrade values;

    public int getUpgradeID(string find)
    {
        return upgrades.FindIndex(x => x == find);
    }

    public int getTRTID()
    {
        return TRT.GetComponent<TRT>().TRTID;
    }

    public string getRandomUpgrade()
    {
        return upgrades[Random.Range(0, upgrades.Count)];
    }

    public void Reset()
    {
        TRT.GetComponent<TRT>().Reset();
    }
}