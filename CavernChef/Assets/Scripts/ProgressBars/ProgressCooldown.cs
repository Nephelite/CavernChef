using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressCooldown : MonoBehaviour
{
    public SelectButton button;
    public ProgressBar bar;
    public Image barFill, fill;

    // Start is called before the first frame update
    void Start()
    {
        bar.maximum = button.GetCooldown();
        bar.current = button.GetCurrentCooldownProgress();
        barFill.enabled = false;
        fill.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        bar.current = button.GetCurrentCooldownProgress();
        if (bar.current < bar.maximum)
        {
            barFill.enabled = true;
            fill.enabled = true;
        }
        else
        {
            barFill.enabled = false;
            fill.enabled = false;
        }
    }
}
