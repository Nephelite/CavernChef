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

    //Here, include various trt-specific upgrade values;



    public string getRandomUpgrade()
    {
        return upgrades[Random.Range(0, upgrades.Count)];
    }
}