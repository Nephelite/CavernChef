using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Might not need this
using TMPro;

public class RPScript : MonoBehaviour
{
    public TMP_Text rpText;

    void increaseRP(int value)
    {
        GlobalVariables.repelPoints += value;
    }

    void decreaseRP(int value)
    {
        GlobalVariables.repelPoints -= value;
    }

    void Start()
    {
        GlobalVariables.repelPoints = 500;
    }

    // Update is called once per frame
    void Update()
    {
        rpText.text = "RP: " + GlobalVariables.repelPoints;

        //Temporary for testing points
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            increaseRP(100);
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            decreaseRP(100);
        }
    }
}
