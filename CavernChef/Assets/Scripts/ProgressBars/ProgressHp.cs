using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressHp : MonoBehaviour
{
    public ProgressBar bar;
    public GameObject foodPoint;
    public Image food;
    public TMP_Text hpDisplay;

    void Start()
    {
        if (RandomSelectionScript.lastChosenFood != null)
            food.sprite = RandomSelectionScript.lastChosenFood.GetComponent<SpriteRenderer>().sprite;
        else
            food.sprite = foodPoint.GetComponent<FoodBehaviour>().defaultFood.GetComponent<SpriteRenderer>().sprite;
        bar.maximum = FoodBehaviour.FoodHP;
        hpDisplay.text = bar.maximum.ToString() + " / " + bar.maximum.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        bar.current = FoodBehaviour.FoodHP;
        hpDisplay.text = FoodBehaviour.FoodHP + " / " + bar.maximum.ToString();
    }
}
