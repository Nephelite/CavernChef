using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeButton : MonoBehaviour
{
    public UpgradesManager manager;
    public int buttonID;
    public Upgrades upgrade;
    public TMP_Text displayText;
    public Sprite gold, silver, bronze;

    void Start()
    {
        upgrade = manager.getChosen(buttonID - 1);
        if (upgrade == null) //specifically for edge cases
        {
            gameObject.SetActive(false);
            return;
        }

        string chosenUpgrade = upgrade.getRandomUpgrade();

        switch (chosenUpgrade)
        {
            case "projspd":
                displayText.text = "Attacks from " + upgrade.name + "s are " + "x%" + " faster";
                gameObject.GetComponent<Button>().image.sprite = bronze;
                break;
            case "atk":
                if (upgrade.name == "Econ TRT")
                {
                    displayText.text = upgrade.name + "s produce " + "x" + " more RP";
                }   
                else
                {
                    displayText.text = upgrade.name + "s deal " + "x" + " more damage";
                }
                gameObject.GetComponent<Button>().image.sprite = silver;
                break;
            case "aoe":
                displayText.text = upgrade.name + "s have " + "x%" + "  more spread on point of impact";
                gameObject.GetComponent<Button>().image.sprite = bronze;
                break;
            case "cost":
                displayText.text = upgrade.name + "s cost " + "x" + " RP less";
                gameObject.GetComponent<Button>().image.sprite = silver;
                break;
            case "interval":
                if (upgrade.name == "Econ TRT")
                {
                    displayText.text = upgrade.name + "s produce RP " + "x%" + " faster";
                }
                else
                {
                    displayText.text = upgrade.name + "s attack " + "x%" + " faster";
                }
                gameObject.GetComponent<Button>().image.sprite = gold;
                break;
            case "range":
                displayText.text = upgrade.name + "s can target enemies " + "x%" + " farther";
                gameObject.GetComponent<Button>().image.sprite = silver;
                break;
            case "cd":
                displayText.text = upgrade.name + " placements have " + "x%" + " shorter cooldown";
                gameObject.GetComponent<Button>().image.sprite = silver;
                break;
            case "special":
                if (upgrade.name == "Electric TRT")
                {
                    displayText.text = upgrade.name + "s' attacks strike 1 more enemy";
                }
                else if (upgrade.name == "Snow TRT")
                {
                    displayText.text = upgrade.name + "s slow enemies by " + "x%" + " more";
                }
                else if (upgrade.name == "Pudding")
                {
                    displayText.text = upgrade.name + "s' HP increases by " + "x%";
                }
                gameObject.GetComponent<Button>().image.sprite = gold;
                break;
        }

    }
}
