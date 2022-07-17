using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedChanger : MonoBehaviour
{
    public static int speedMultiplier;
    public TMP_Text displayText;
    public GameObject x1, x2, x3;

    // Start is called before the first frame update
    void Start()
    {
        if (speedMultiplier == 0)
            speedMultiplier = 1;

        if (speedMultiplier != 1)
            x1.SetActive(false);

        if (speedMultiplier != 2)
            x2.SetActive(false);

        if (speedMultiplier != 3)
            x3.SetActive(false);

        Time.timeScale = speedMultiplier;
        displayText.text = "x" + speedMultiplier;
    }

    public void increaseSpeed()
    {
        if (speedMultiplier < 3)
        {
            speedMultiplier++;
        }
        else
        {
            speedMultiplier = 1;
        }


        if (speedMultiplier != 1)
            x1.SetActive(false);
        else
            x1.SetActive(true);

        if (speedMultiplier != 2)
            x2.SetActive(false);
        else 
            x2.SetActive(true);

        if (speedMultiplier != 3)
            x3.SetActive(false);
        else
            x3.SetActive(true);

        Time.timeScale = speedMultiplier;
        displayText.text = "x" + speedMultiplier;
    }
}
