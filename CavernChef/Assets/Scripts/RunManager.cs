using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunManager : MonoBehaviour
{
    public List<GameObject> foodsList = new List<GameObject>();
    public List<GameObject> TRTButtonsIndexedList = new List<GameObject>(); //Need to manually add all of the buttons from canvas
    //List: 0 - Econ, 1 - Fire, 2 - Water, 3 - Snow, 4 - Light, 5 - Electric, 6 - Earth, 7 - Blockage, 8 - Pudding, 
    public GameObject TRTMenu;
    public static bool testRun = true, contRun; //For making game testing easier. Remove in final product.
    public static bool[] accessibleButtonsSaveData = new bool[32]; //NOTE: Change this if needed, it has to be at least greater than the number of TRT tpes placeable.;
    public static bool[] seenFoods = new bool[32], seenEnemies = new bool[32], seenTRTs = new bool[32];
    public static int[] last3FoodChoicesID;
    public static int[] upgradeCountList = new int[32];
    // Indices for upgrades - 0: projspd, 1: atk, 2: aoe, 3: cost, 4: interval, 5: range, 6: cd, 7: special
    public static int[] upgradesEcon = new int[8]; // Active: 1: value, 3: cost, 4: interval, 6: cd
    public static int[] upgradesFire = new int[8]; // Active: 0: projspd, 1: atk, 2: aoe, 3: cost, 4: interval, 5: range, 6: cd
    public static int[] upgradesWater = new int[8]; // Active: 0: projspd, 1: atk, 3: cost, 4: interval, 5: range, 6: cd
    public static int[] upgradesSnow = new int[8]; // Active: 0: projspd, 1: atk, 2: aoe, 3: cost, 4: interval, 5: range, 6: cd, 7: special
    public static int[] upgradesLight = new int[8]; // Active: 0: projspd, 1: atk, 2: aoe, 3: cost, 4: interval, 5: range, 6: cd
    public static int[] upgradesElectric = new int[8]; // Active: 0: projspd, 1: atk, 2: aoe, 3: cost, 4: interval, 5: range, 6: cd, 7: special
    public static int[] upgradesEarth = new int[8]; // Active: 0: projspd, 1: atk, 3: cost, 4: interval, 5: range, 6: cd
    public static int[] upgradesBlockage = new int[8]; // Active: 3: cost, 6: cd;
    public static int[] upgradesPudding = new int[8]; // Active: 3: cost, 6: cd, 7: special (hp);
    public static int[][] upgradesTracker = new int[][] {upgradesEcon, upgradesFire, upgradesWater, upgradesSnow, upgradesLight, upgradesElectric, upgradesEarth, upgradesBlockage, upgradesPudding}; //Merely pointers, doesn't need to be saved per run

    private int testNum = 9; //Change if testing of a certain TRT is needed

    void Start()
    {
        AudioManager manager = FindObjectOfType<AudioManager>();
        manager.UpdateNewMusicVolume();
        manager.UpdateNewSFXVolume();

        if (GlobalVariables.settings == null)
        {
            GlobalVariables.settings = new Settings(1f, 1f, true);
        }

        if (!GlobalVariables.settings.lightingToggle && FindObjectOfType<Grid>() != null) 
        {
            FindObjectOfType<Grid>().gameObject.transform.Find("Light").gameObject.SetActive(false);
        }

        if (testRun)
        {
            for (int i = 0; i < testNum; i++)
            {
                accessibleButtonsSaveData[i] = true;
            }
        }

        for (int i = 0; i < TRTButtonsIndexedList.Count; i++)
        {
            TRTButtonsIndexedList[i].SetActive(accessibleButtonsSaveData[i]);
        }

        seenTRTs[0] = true;
        seenTRTs[1] = true;

        if (contRun)
        {
            reloadUpgrades();
            contRun = false;
        }

        for (int i = 0; i < upgradesTracker.Length; i++)
        {
            string res = "";
            for (int j = 0; j < 7; j++)
            {
                res += upgradesTracker[i][j] + " ";
            }
            Debug.Log(res);
        }
    }

    public void nextStage()
    {
        if (GlobalVariables.nextSceneToPlay < 4 || GlobalVariables.nextSceneToPlay > 7)
        {
            Debug.Log("Error: Play the game properly");
            GlobalVariables.nextSceneToPlay = Random.Range(4, 8);
        }
        UpgradesAndUnlocks.firstUnlock = -1;
        UpgradesAndUnlocks.secondUnlock = -1;
        FindObjectOfType<AudioManager>().StopAllAudio();
        FindObjectOfType<AudioManager>().PlayMusic("FoodSelectTheme");
        SceneManager.LoadScene(3);
    }

    public void newRun()
    {
        for (int i = 0; i < 2; i++)
        {
            accessibleButtonsSaveData[i] = true;
        }
        for (int i = 2; i < accessibleButtonsSaveData.Length; i++)
        {
            accessibleButtonsSaveData[i] = false;
        }

        RandomSelectionScript.choiceOne = null;
        RandomSelectionScript.choiceTwo = null;
        RandomSelectionScript.choiceThree = null;
        last3FoodChoicesID = null;
        FindObjectOfType<UpgradesManager>().ResetAll();
        upgradeCountList = new int[32];
        // Indices for upgrades - 0: projspd, 1: atk, 2: aoe, 3: cost, 4: interval, 5: range, 6: cd, 7: special
        upgradesEcon = new int[8]; // Active: 1: value, 3: cost, 4: interval, 6: cd
        upgradesFire = new int[8]; // Active: 0: projspd, 1: atk, 2: aoe, 3: cost, 4: interval, 5: range, 6: cd
        upgradesWater = new int[8]; // Active: 0: projspd, 1: atk, 3: cost, 4: interval, 5: range, 6: cd
        upgradesSnow = new int[8]; // Active: 0: projspd, 1: atk, 2: aoe, 3: cost, 4: interval, 5: range, 6: cd, 7: special
        upgradesLight = new int[8]; // Active: 0: projspd, 1: atk, 2: aoe, 3: cost, 4: interval, 5: range, 6: cd
        upgradesElectric = new int[8]; // Active: 0: projspd, 1: atk, 2: aoe, 3: cost, 4: interval, 5: range, 6: cd, 7: special
        upgradesEarth = new int[8]; // Active: 0: projspd, 1: atk, 3: cost, 4: interval, 5: range, 6: cd
        upgradesBlockage = new int[8]; // Active: 3: cost, 6: cd;
        upgradesPudding = new int[8]; // Active: 3: cost, 6: cd, 7: special (hp);

        Spawner.zoneNumber = 0;
        SceneManager.LoadScene(3);
        RandomSelectionScript.Initialise(foodsList);
    }

    public void saveRun()
    {
        SaveSystem.SaveRun(new Run(accessibleButtonsSaveData, GlobalVariables.lastClearedScene, GlobalVariables.nextSceneToPlay, last3FoodChoicesID, Spawner.zoneNumber,
            upgradeCountList, upgradesTracker[0], upgradesTracker[1], upgradesTracker[2], upgradesTracker[3], upgradesTracker[4], upgradesTracker[5], upgradesTracker[6], upgradesTracker[7], upgradesTracker[8]));
        SaveSystem.SaveCurrentFile(new SaveFile(RunManager.seenTRTs, RunManager.seenFoods, RunManager.seenEnemies));
    }

    public void saveFile() //Add to victory scene
    {
        SaveSystem.SaveCurrentFile(new SaveFile(RunManager.seenTRTs, RunManager.seenFoods, RunManager.seenEnemies));
    }

    public static void loadFile()
    {
        SaveFile file = SaveSystem.LoadFile();
        if (file != null)
        {
            seenTRTs = file.seenTRTs;
            seenFoods = file.seenFoods;
            seenEnemies = file.seenEnemies;
            GlobalVariables.numOfWins = file.numOfWins;
        }
    }

    public static void reloadUpgrades()
    {
        UpgradesManager upManager = FindObjectOfType<UpgradesManager>();
        for (int i = 0; i < upgradesTracker.Length; i++)
        {
            switch(i)
            {
                case 0: // Reload Econ Upgrades, 1: value, 3: cost, 4: interval, 6: cd
                    EconTRT econ = (Resources.Load("EconTRT") as GameObject).GetComponent<EconTRT>();
                    for (int j = 0; j < upgradesEcon.Length; j++) // Cycling through upgrade types
                    {
                        if (upgradesEcon[j] > 0)
                        {
                            for (int l = 0; l < upgradesEcon[j]; l++)
                            {
                                switch (j)
                                {
                                    case 1:
                                        econ.AddValue((int) upManager.upgradesList[0].upgradeValues[0]);
                                        break;
                                    case 3:
                                        econ.MultCost(1f - (upManager.upgradesList[0].upgradeValues[1] / 100f));
                                        break;
                                    case 4:
                                        econ.MultCycle(1f - (upManager.upgradesList[0].upgradeValues[2] / 100f));
                                        break;
                                    case 6:
                                        econ.MultTimeBetPlacements(1f - (upManager.upgradesList[0].upgradeValues[3] / 100f));
                                        break;
                                }
                            }
                        }
                    }
                    break;
                case 1: // Active: 0: projspd, 1: atk, 2: aoe, 3: cost, 4: interval, 5: range, 6: cd
                    FireTRT fire = (Resources.Load("FireTRT") as GameObject).GetComponent<FireTRT>();
                    for (int j = 0; j < upgradesFire.Length; j++) // Cycling through upgrade types
                    {
                        if (upgradesFire[j] > 0)
                        {
                            for (int l = 0; l < upgradesFire[j]; l++)
                            {
                                switch (j)
                                {
                                    case 0:
                                        fire.MultCentiSpeed(1f + (upManager.upgradesList[1].upgradeValues[0] / 100f));
                                        break;
                                    case 1:
                                        fire.AddDmg(upManager.upgradesList[1].upgradeValues[1]);
                                        break;
                                    case 2:
                                        fire.MultAoeRadius(1f + (upManager.upgradesList[1].upgradeValues[2] / 100));
                                        break;
                                    case 3:
                                        fire.MultCost(1f - (upManager.upgradesList[1].upgradeValues[3] / 100f));
                                        break;
                                    case 4:
                                        fire.MultTimeBetAtks(1f - (upManager.upgradesList[1].upgradeValues[4] / 100f));
                                        break;
                                    case 5:
                                        fire.AddRange(upManager.upgradesList[1].upgradeValues[5]);
                                        break;
                                    case 6:
                                        fire.MultTimeBetPlacements(1f - (upManager.upgradesList[1].upgradeValues[6] / 100f));
                                        break;
                                }
                            }
                        }
                    }
                    break;
                case 2: // Active: 0: projspd, 1: atk, 3: cost, 4: interval, 5: range, 6: cd
                    WaterTRT water = (Resources.Load("WaterTRT") as GameObject).GetComponent<WaterTRT>();
                    for (int j = 0; j < upgradesWater.Length; j++) // Cycling through upgrade types
                    {
                        if (upgradesWater[j] > 0)
                        {
                            for (int l = 0; l < upgradesWater[j]; l++)
                            {
                                switch (j)
                                {
                                    case 0:
                                        water.MultCentiSpeed(1f + (upManager.upgradesList[2].upgradeValues[0] / 100f));
                                        break;
                                    case 1:
                                        water.AddDmg(upManager.upgradesList[2].upgradeValues[1]);
                                        break;
                                    case 3:
                                        water.MultCost(1f - (upManager.upgradesList[2].upgradeValues[3] / 100f));
                                        break;
                                    case 4:
                                        water.MultTimeBetAtks(1f - (upManager.upgradesList[2].upgradeValues[4] / 100f));
                                        break;
                                    case 5:
                                        water.AddRange(upManager.upgradesList[2].upgradeValues[5]);
                                        break;
                                    case 6:
                                        water.MultTimeBetPlacements(1f - (upManager.upgradesList[2].upgradeValues[6] / 100f));
                                        break;
                                }
                            }
                        }
                    }
                    break;
                case 3: // Active: 0: projspd, 1: atk, 2: aoe, 3: cost, 4: interval, 5: range, 6: cd, 7: special
                    SnowTRT snow = (Resources.Load("SnowTRT") as GameObject).GetComponent<SnowTRT>();
                    for (int j = 0; j < upgradesSnow.Length; j++) // Cycling through upgrade types
                    {
                        if (upgradesSnow[j] > 0)
                        {
                            for (int l = 0; l < upgradesSnow[j]; l++)
                            {
                                switch (j)
                                {
                                    case 0:
                                        snow.MultCentiSpeed(1f + (upManager.upgradesList[3].upgradeValues[0] / 100f));
                                        break;
                                    case 1:
                                        snow.AddDmg(upManager.upgradesList[3].upgradeValues[1]);
                                        break;
                                    case 2:
                                        snow.MultAoeRadius(1f + (upManager.upgradesList[3].upgradeValues[2] / 100f));
                                        break;
                                    case 3:
                                        snow.MultCost(1 - (upManager.upgradesList[3].upgradeValues[3] / 100f));
                                        break;
                                    case 4:
                                        snow.MultTimeBetAtks(1 - (upManager.upgradesList[3].upgradeValues[4] / 100f));
                                        break;
                                    case 5:
                                        snow.AddRange(upManager.upgradesList[3].upgradeValues[5]);
                                        break;
                                    case 6:
                                        snow.MultTimeBetPlacements(1f - (upManager.upgradesList[3].upgradeValues[6] / 100f));
                                        break;
                                    case 7:
                                        snow.AddEffectFrames((int)upManager.upgradesList[3].upgradeValues[7]);
                                        break;
                                }
                            }
                        }
                    }
                    break;
                case 4: // Active: 0: projspd, 1: atk, 2: aoe, 3: cost, 4: interval, 5: range, 6: cd
                    LightTRT light = (Resources.Load("LightTRT") as GameObject).GetComponent<LightTRT>();
                    for (int j = 0; j < upgradesLight.Length; j++) // Cycling through upgrade types
                    {
                        if (upgradesLight[j] > 0)
                        {
                            for (int l = 0; l < upgradesLight[j]; l++)
                            {
                                switch (j)
                                {
                                    case 0:
                                        light.MultCentiSpeed(1f + (upManager.upgradesList[4].upgradeValues[0] / 100f));
                                        break;
                                    case 1:
                                        light.AddDmg(upManager.upgradesList[4].upgradeValues[1]);
                                        break;
                                    case 2:
                                        light.MultAoeRadius(1f + (upManager.upgradesList[4].upgradeValues[2] / 100f));
                                        break;
                                    case 3:
                                        light.MultCost(1f - (upManager.upgradesList[4].upgradeValues[3] / 100f));
                                        break;
                                    case 4:
                                        light.MultTimeBetAtks(1f - (upManager.upgradesList[4].upgradeValues[4] / 100f));
                                        break;
                                    case 5:
                                        light.AddRange(upManager.upgradesList[4].upgradeValues[5]);
                                        break;
                                    case 6:
                                        light.MultTimeBetPlacements(1f - (upManager.upgradesList[4].upgradeValues[6] / 100f));
                                        break;
                                }
                            }
                        }
                    }
                    break;
                case 5: // Active: 0: projspd, 1: atk, 2: aoe, 3: cost, 4: interval, 5: range, 6: cd, 7: special
                    ElectricTRT electric = (Resources.Load("ElectricTRT") as GameObject).GetComponent<ElectricTRT>();
                    for (int j = 0; j < upgradesElectric.Length; j++) // Cycling through upgrade types
                    {
                        if (upgradesElectric[j] > 0)
                        {
                            for (int l = 0; l < upgradesElectric[j]; l++)
                            {
                                switch (j)
                                {
                                    case 0:
                                        electric.MultCentiSpeed(1 + (upManager.upgradesList[5].upgradeValues[0] / 100f));
                                        break;
                                    case 1:
                                        electric.AddDmg(upManager.upgradesList[5].upgradeValues[1]);
                                        break;
                                    case 2:
                                        electric.MultAoeRadius(1f + (upManager.upgradesList[5].upgradeValues[2] / 100f));
                                        break;
                                    case 3:
                                        electric.MultCost(1f - (upManager.upgradesList[5].upgradeValues[3] / 100f));
                                        break;
                                    case 4:
                                        electric.MultTimeBetAtks(1f - (upManager.upgradesList[5].upgradeValues[4] / 100f));
                                        break;
                                    case 5:
                                        electric.AddRange(upManager.upgradesList[5].upgradeValues[5]);
                                        break;
                                    case 6:
                                        electric.MultTimeBetPlacements(1f - (upManager.upgradesList[5].upgradeValues[6] / 100f));
                                        break;
                                    case 7:
                                        electric.AddChain();
                                        break;
                                }
                            }
                        }
                    }
                    break;
                case 6: // Active: 0: projspd, 1: atk, 3: cost, 4: interval, 5: range, 6: cd
                    EarthTRT earth = (Resources.Load("EarthTRT") as GameObject).GetComponent<EarthTRT>();
                    for (int j = 0; j < upgradesEarth.Length; j++) // Cycling through upgrade types
                    {
                        if (upgradesEarth[j] > 0)
                        {
                            for (int l = 0; l < upgradesEarth[j]; l++)
                            {
                                switch (j)
                                {
                                    case 0:
                                        earth.MultCentiSpeed(1f + (upManager.upgradesList[6].upgradeValues[0] / 100f));
                                        break;
                                    case 1:
                                        earth.MultDmg(1f + (upManager.upgradesList[6].upgradeValues[1] / 100f));
                                        break;
                                    case 3:
                                        earth.MultCost(1f - (upManager.upgradesList[6].upgradeValues[2] / 100f));
                                        break;
                                    case 4:
                                        earth.MultTimeBetAtks(1f - (upManager.upgradesList[6].upgradeValues[3] / 100f));
                                        break;
                                    case 5:
                                        earth.AddRange(upManager.upgradesList[6].upgradeValues[4]);
                                        break;
                                    case 6:
                                        earth.MultTimeBetPlacements(1f - (upManager.upgradesList[6].upgradeValues[5] / 100f));
                                        break;
                                }
                            }
                        }
                    }
                    break;
                case 7:
                    BlockageTRT block = (Resources.Load("BlockageTRT") as GameObject).GetComponent<BlockageTRT>();
                    for (int j = 0; j < upgradesBlockage.Length; j++) // Cycling through upgrade types
                    {
                        if (upgradesBlockage[j] > 0)
                        {
                            for (int l = 0; l < upgradesBlockage[j]; l++)
                            {
                                switch (j)
                                {
                                    case 3:
                                        block.MultCost(1f - (upManager.upgradesList[7].upgradeValues[0] / 100f));
                                        break;
                                    case 6:
                                        block.MultTimeBetPlacements(1f - (upManager.upgradesList[7].upgradeValues[1] / 100f));
                                        break;
                                }
                            }
                        }
                    }
                    break;
                case 8: // Active: 3: cost, 6: cd, 7: special (hp);
                    StallTRT stall = (Resources.Load("PuddingTRT") as GameObject).GetComponent<StallTRT>();
                    for (int j = 0; j < upgradesPudding.Length; j++) // Cycling through upgrade types
                    {
                        if (upgradesPudding[j] > 0)
                        {
                            for (int l = 0; l < upgradesPudding[j]; l++)
                            {
                                switch (j)
                                {
                                    case 3:
                                        stall.MultCost(1f - (upManager.upgradesList[8].upgradeValues[0] / 100f));
                                        break;
                                    case 6:
                                        stall.MultTimeBetPlacements(1 - (upManager.upgradesList[8].upgradeValues[1] / 100f));
                                        break;
                                    case 7:
                                        stall.AddHp((int)upManager.upgradesList[8].upgradeValues[2]);
                                        break;
                                }
                            }
                        }
                    }
                    break;
            }
        }
    }

    public void loadRun() //not in use I think
    {
        Run currentRun = SaveSystem.LoadRun();
        accessibleButtonsSaveData = currentRun.unlocks;
        
        for (int i = 0; i < TRTButtonsIndexedList.Count; i++)
        {
            if (accessibleButtonsSaveData[i])
            {
                TRTButtonsIndexedList[i].SetActive(true);
            }
        }

        last3FoodChoicesID = currentRun.last3FoodChoicesID;
        Spawner.zoneNumber = currentRun.numZones;
        //Next scene generate
    }

    void newButtonAdded(int i)
    {
        GameObject buttonToAssign = TRTButtonsIndexedList[i];
        buttonToAssign.transform.SetParent(TRTMenu.transform);
        accessibleButtonsSaveData[i] = true;
    }

    //Temporary testing function to be removed
    public void unlockTRT() //Note: Need to refresh to see new unlocks
    {
        int i = 0;
        while (accessibleButtonsSaveData[i])
        {
            i++;
        }
        if (i < 7)
        {
            accessibleButtonsSaveData[i] = true;
        }
    }
}