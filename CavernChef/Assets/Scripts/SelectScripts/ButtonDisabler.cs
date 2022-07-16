using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonDisabler : MonoBehaviour
{
    public static bool buttonClicked = false;

    // Start is called before the first frame update
    void Start()
    {
        /*
        for (int i = 1; i < 5; i++) //Disables the upgrade windows for now, because they aren't implemented yet
        {
            gameObject.transform.Find("Canvas").Find("UpgradesAndUnlocks").Find("Upgrade " + i).gameObject.SetActive(false);
        }
        */

        if (UpgradesAndUnlocks.firstUnlock < 0)
        {
            gameObject.transform.Find("Canvas").Find("UpgradesAndUnlocks").Find("Unlock " + 1).gameObject.SetActive(false);
            gameObject.transform.Find("Canvas").Find("UpgradesAndUnlocks").Find("Upgrade " + 4).gameObject.SetActive(true);
        }
        else
        {
            gameObject.transform.Find("Canvas").Find("UpgradesAndUnlocks").Find("Unlock " + 1).gameObject.SetActive(true);
            gameObject.transform.Find("Canvas").Find("UpgradesAndUnlocks").Find("Upgrade " + 4).gameObject.SetActive(false);
        }

        if (UpgradesAndUnlocks.secondUnlock < 0)
        {
            gameObject.transform.Find("Canvas").Find("UpgradesAndUnlocks").Find("Unlock " + 2).gameObject.SetActive(false);
            gameObject.transform.Find("Canvas").Find("UpgradesAndUnlocks").Find("Upgrade " + 3).gameObject.SetActive(true);
        }
        else
        {
            gameObject.transform.Find("Canvas").Find("UpgradesAndUnlocks").Find("Unlock " + 2).gameObject.SetActive(true);
            gameObject.transform.Find("Canvas").Find("UpgradesAndUnlocks").Find("Upgrade " + 3).gameObject.SetActive(false);
        }

        gameObject.transform.Find("Canvas").Find("Advance").gameObject.SetActive(false);
        gameObject.transform.Find("Canvas").Find("Skip").gameObject.SetActive(true);
    }

    void Update()
    {
        if (buttonClicked)
        {
            buttonClicked = false;
            gameObject.transform.Find("Canvas").Find("UpgradesAndUnlocks").gameObject.SetActive(false);
            gameObject.transform.Find("Canvas").Find("Advance").gameObject.SetActive(true);
            gameObject.transform.Find("Canvas").Find("Skip").gameObject.SetActive(false); 
        }
    }
}
