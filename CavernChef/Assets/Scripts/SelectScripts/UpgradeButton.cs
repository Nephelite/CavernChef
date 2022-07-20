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
    private string chosenUpgrade;
    public Sprite gold, silver, bronze;

    public void select()
    {
        int TRTID = upgrade.getTRTID();
        int upgradeID = upgrade.getUpgradeID(chosenUpgrade);
        RunManager.upgradeCountList[TRTID]++; //increases upgrade counter
        Debug.Log(RunManager.upgradeCountList[upgrade.TRT.GetComponent<TRT>().TRTID]);

        float factor = upgrade.upgradeValues[upgradeID];
        switch (chosenUpgrade)
        {
            case "projspd":
                RunManager.upgradesTracker[TRTID][0]++;
                Debug.Log(chosenUpgrade);

                if (upgrade.areValuesFlat[upgradeID])
                    upgrade.TRT.GetComponent<AtkTower>().AddCentiSpeed(factor);
                else
                {
                    factor = (factor / 100f) + 1f;
                    upgrade.TRT.GetComponent<AtkTower>().MultCentiSpeed(factor);
                }

                break;
            case "atk":
                RunManager.upgradesTracker[TRTID][1]++;
                Debug.Log(chosenUpgrade);

                if (upgrade.TRT.GetComponent<AtkTower>() != null)
                {
                    if (upgrade.areValuesFlat[upgradeID])
                        upgrade.TRT.GetComponent<AtkTower>().AddDmg(factor);
                    else
                    {
                        factor = (factor / 100f) + 1f;
                        upgrade.TRT.GetComponent<AtkTower>().MultDmg(factor);
                    }
                }
                else if (upgrade.TRT.GetComponent<EconTRT>() != null)
                {
                    if (upgrade.areValuesFlat[upgradeID])
                        upgrade.TRT.GetComponent<EconTRT>().AddValue((int)factor);
                    else
                    {
                        factor = (factor / 100f) + 1f;
                        upgrade.TRT.GetComponent<EconTRT>().MultValue(factor);
                    }
                }

                break;
            case "aoe":
                RunManager.upgradesTracker[TRTID][2]++;
                Debug.Log(chosenUpgrade);

                if (upgrade.areValuesFlat[upgradeID])
                    upgrade.TRT.GetComponent<AtkTower>().AddAoeRadius(factor);
                else
                {
                    factor = (factor / 100f) + 1f;
                    upgrade.TRT.GetComponent<AtkTower>().MultAoeRadius(factor);
                }

                break;
            case "cost":
                RunManager.upgradesTracker[TRTID][3]++;
                Debug.Log(chosenUpgrade);

                if (upgrade.TRT.GetComponent<AtkTower>() != null)
                {
                    if (upgrade.areValuesFlat[upgradeID])
                        upgrade.TRT.GetComponent<AtkTower>().AddCost(-(int)factor);
                    else
                    {
                        factor = 1f - (factor / 100f);
                        upgrade.TRT.GetComponent<AtkTower>().MultCost(factor);
                    }
                }
                else if (upgrade.TRT.GetComponent<EconTRT>() != null)
                {
                    if (upgrade.areValuesFlat[upgradeID])
                        upgrade.TRT.GetComponent<EconTRT>().AddCost(-(int)factor);
                    else
                    {
                        factor = 1f - (factor / 100f);
                        upgrade.TRT.GetComponent<EconTRT>().MultCost(factor);
                    }
                }
                else if (upgrade.TRT.GetComponent<BlockageTRT>() != null)
                {
                    if (upgrade.areValuesFlat[upgradeID])
                        upgrade.TRT.GetComponent<BlockageTRT>().AddCost(-(int)factor);
                    else
                    {
                        factor = 1f - (factor / 100f);
                        upgrade.TRT.GetComponent<BlockageTRT>().MultCost(factor);
                    }
                }
                else if (upgrade.TRT.GetComponent<StallTRT>() != null)
                {
                    if (upgrade.areValuesFlat[upgradeID])
                        upgrade.TRT.GetComponent<StallTRT>().AddCost(-(int)factor);
                    else
                    {
                        factor = 1f - (factor / 100f);
                        upgrade.TRT.GetComponent<StallTRT>().MultCost(factor);
                    }
                }

                break;
            case "interval":
                RunManager.upgradesTracker[TRTID][4]++;
                Debug.Log(chosenUpgrade);

                if (upgrade.TRT.GetComponent<AtkTower>() != null)
                {
                    if (upgrade.areValuesFlat[upgradeID])
                        upgrade.TRT.GetComponent<AtkTower>().AddTimeBetAtks(-factor);
                    else
                    {
                        factor = 1f - (factor / 100f);
                        upgrade.TRT.GetComponent<AtkTower>().MultTimeBetAtks(factor);
                    }
                }
                else if (upgrade.TRT.GetComponent<EconTRT>() != null)
                {
                    if (upgrade.areValuesFlat[upgradeID])
                        upgrade.TRT.GetComponent<EconTRT>().AddCycle(-(int)factor);
                    else
                    {
                        factor = 1f - (factor / 100f);
                        upgrade.TRT.GetComponent<EconTRT>().MultCycle(factor);
                    }
                }

                break;
            case "range":
                RunManager.upgradesTracker[TRTID][5]++;
                Debug.Log(chosenUpgrade);

                if (upgrade.areValuesFlat[upgradeID])
                    upgrade.TRT.GetComponent<AtkTower>().AddRange(factor);
                else
                {
                    factor = (factor / 100f) + 1f;
                    upgrade.TRT.GetComponent<AtkTower>().MultRange(factor);
                }

                break;
            case "cd":
                RunManager.upgradesTracker[TRTID][6]++;
                Debug.Log(chosenUpgrade);

                if (upgrade.TRT.GetComponent<AtkTower>() != null)
                {
                    if (upgrade.areValuesFlat[upgradeID])
                        upgrade.TRT.GetComponent<AtkTower>().AddTimeBetPlacements(-factor);
                    else
                    {
                        factor = 1f - (factor / 100f);
                        upgrade.TRT.GetComponent<AtkTower>().MultTimeBetPlacements(factor);
                    }
                }
                else if (upgrade.TRT.GetComponent<EconTRT>() != null)
                {
                    if (upgrade.areValuesFlat[upgradeID])
                        upgrade.TRT.GetComponent<EconTRT>().AddTimeBetPlacements(-factor);
                    else
                    {
                        factor = 1f - (factor / 100f);
                        upgrade.TRT.GetComponent<EconTRT>().MultTimeBetPlacements(factor);
                    }
                }
                else if (upgrade.TRT.GetComponent<BlockageTRT>() != null)
                {
                    if (upgrade.areValuesFlat[upgradeID])
                        upgrade.TRT.GetComponent<BlockageTRT>().AddTimeBetPlacements(-factor);
                    else
                    {
                        factor = 1f - (factor / 100f);
                        upgrade.TRT.GetComponent<BlockageTRT>().MultTimeBetPlacements(factor);
                    }
                }
                else if (upgrade.TRT.GetComponent<StallTRT>() != null)
                {
                    if (upgrade.areValuesFlat[upgradeID])
                        upgrade.TRT.GetComponent<StallTRT>().AddTimeBetPlacements(-factor);
                    else
                    {
                        factor = 1f - (factor / 100f);
                        upgrade.TRT.GetComponent<StallTRT>().MultTimeBetPlacements(factor);
                    }
                }

                break;
            case "special":
                RunManager.upgradesTracker[TRTID][7]++;
                Debug.Log(chosenUpgrade);

                if (upgrade.TRT.GetComponent<StallTRT>() != null)
                {
                    if (upgrade.areValuesFlat[upgradeID])
                        upgrade.TRT.GetComponent<StallTRT>().AddHp((int)factor);
                    else
                    {
                        factor = (factor / 100f) + 1f;
                        upgrade.TRT.GetComponent<StallTRT>().MultHp(factor);
                    }
                }
                else if (upgrade.TRT.GetComponent<ElectricTRT>() != null)
                {
                    upgrade.TRT.GetComponent<ElectricTRT>().AddChain();
                }
                else if (upgrade.TRT.GetComponent<SnowTRT>() != null)
                {
                    if (upgrade.areValuesFlat[upgradeID])
                        upgrade.TRT.GetComponent<SnowTRT>().AddEffectFrames((int)factor);
                    else
                    {
                        factor = (factor / 100f) + 1f;
                        upgrade.TRT.GetComponent<SnowTRT>().MultEffectFrames(factor);
                    }
                }

                break;
        }
    }

    void Start()
    {
        upgrade = manager.getChosen(buttonID - 1); //random TRT

        if (upgrade == null) //specifically for edge cases
        {
            gameObject.SetActive(false);
            return;
        }

        chosenUpgrade = upgrade.getRandomUpgrade(); //random TRT upgrade
        int TRTID = upgrade.getTRTID();
        int upgradeID = upgrade.getUpgradeID(chosenUpgrade);

        switch (chosenUpgrade)
        {
            case "projspd":
                displayText.text = upgrade.areValuesFlat[upgradeID]
                    ? "Attacks from " + upgrade.name + "s are " + upgrade.upgradeValues[upgradeID] + " units faster" 
                    : "Attacks from " + upgrade.name + "s are " + upgrade.upgradeValues[upgradeID] + "% faster";
                gameObject.GetComponent<Button>().image.sprite = bronze;
                break;
            case "atk":
                if (upgrade.name == "Econ TRT")
                    displayText.text = upgrade.name + "s produce " + upgrade.upgradeValues[upgradeID] + " more RP per production"; //Flat
                else
                    displayText.text = upgrade.areValuesFlat[upgradeID]
                        ? upgrade.name + "s deal " + upgrade.upgradeValues[upgradeID] + " more damage"
                        : upgrade.name + "s deal " + upgrade.upgradeValues[upgradeID] + "% more damage";
                gameObject.GetComponent<Button>().image.sprite = silver;
                break;
            case "aoe":
                displayText.text = upgrade.name + "s have " + upgrade.upgradeValues[upgradeID] + " unit more spread on point of impact";
                gameObject.GetComponent<Button>().image.sprite = bronze;
                break;
            case "cost":
                displayText.text = upgrade.areValuesFlat[upgradeID]
                    ? upgrade.name + "s' RP costs are reduced by " + upgrade.upgradeValues[upgradeID]
                    : upgrade.name + "s' RP costs are reduced by " + upgrade.upgradeValues[upgradeID] + "%";
                gameObject.GetComponent<Button>().image.sprite = silver;
                break;
            case "interval":
                if (upgrade.name == "Econ TRT")
                    displayText.text = "Time between " + upgrade.name + "s' RP production is reduced by " + upgrade.upgradeValues[upgradeID] + "%"; //percentage
                else
                    displayText.text = !upgrade.areValuesFlat[upgradeID]
                        ? "Time between " + upgrade.name + "s' attacks are reduced by " + upgrade.upgradeValues[upgradeID] + "%"
                        : "Time between " + upgrade.name + "s' attacks are reduced by " + upgrade.upgradeValues[upgradeID] + " seconds";
                gameObject.GetComponent<Button>().image.sprite = gold;
                break;
            case "range":
                displayText.text = !upgrade.areValuesFlat[upgradeID]
                    ? upgrade.name + "s can target enemies " + upgrade.upgradeValues[upgradeID] + "% farther"
                    : upgrade.name + "s can target enemies " + upgrade.upgradeValues[upgradeID] + " units farther";
                gameObject.GetComponent<Button>().image.sprite = silver;
                break;
            case "cd":
                displayText.text = upgrade.areValuesFlat[upgradeID]
                    ? upgrade.name + "s' placement cooldowns are reduced by " + upgrade.upgradeValues[upgradeID] + " seconds"
                    : upgrade.name + "s' placement cooldowns are reduced by " + upgrade.upgradeValues[upgradeID] + "%";
                gameObject.GetComponent<Button>().image.sprite = silver;
                break;
            case "special":
                if (upgrade.name == "Electric TRT")
                    displayText.text = upgrade.name + "s' attacks strike 1 more enemy";
                else if (upgrade.name == "Snow TRT")
                    displayText.text = upgrade.name + "s slow enemies by " + upgrade.upgradeValues[upgradeID] + "% more";
                else if (upgrade.name == "Pudding")
                    displayText.text = upgrade.name + "s' HP increases by " + upgrade.upgradeValues[upgradeID];
                gameObject.GetComponent<Button>().image.sprite = gold;
                break;
        }

    }
}
