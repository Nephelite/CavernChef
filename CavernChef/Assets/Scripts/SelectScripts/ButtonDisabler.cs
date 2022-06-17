using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDisabler : MonoBehaviour
{
    public static bool buttonClicked = false;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i < 3; i++) //Disables the
        {
            gameObject.transform.Find("Canvas").Find("UpgradesAndUnlocks").Find("Upgrade " + i).gameObject.SetActive(false);
        }

        if (UpgradesAndUnlocks.firstUnlock < 0)
        {
            gameObject.transform.Find("Canvas").Find("UpgradesAndUnlocks").Find("Unlock " + 1).gameObject.SetActive(false);
        }

        if (UpgradesAndUnlocks.secondUnlock < 0)
        {
            gameObject.transform.Find("Canvas").Find("UpgradesAndUnlocks").Find("Unlock " + 2).gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (buttonClicked)
        {
            buttonClicked = false;
            gameObject.transform.Find("Canvas").Find("UpgradesAndUnlocks").gameObject.SetActive(false);
        }
    }
}
