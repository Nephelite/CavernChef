using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image mask;
    public Image fill;
    public Image bar;
    public float maximum;
    public float current;
    public Color colorFill;
    public Color colorBar;

    // Update is called once per frame
    void Update()
    {
        GetCurrentFill();
    }

    public void GetCurrentFill()
    {
        mask.fillAmount = current / maximum;
        fill.color = colorFill;
        bar.color = colorBar;
    }
}
