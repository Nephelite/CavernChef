using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Might not need this
using TMPro;


public class DisplayZoneNumber : MonoBehaviour
{
    public TMP_Text rpText;

    // Update is called once per frame
    void Start()
    {
        rpText.text = "Zone Number: " + Spawner.zoneNumber;
    }
}
