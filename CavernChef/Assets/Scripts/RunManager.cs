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
    public static bool testRun = true; //For making game testing easier. Remove in final product.
    public static bool[] accessibleButtonsSaveData = new bool[32]; //NOTE: Change this if needed, it has to be at least greater than the number of TRT tpes placeable.;
    public static bool[] seenFoods = new bool[32], seenEnemies = new bool[32], seenTRTs = new bool[32];
    public static int[] last3FoodChoicesID;

    private int testNum = 9; //Change if testing of a certain TRT is needed

    void Start()
    {
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
        Spawner.zoneNumber = 0;
        SceneManager.LoadScene(3);
        RandomSelectionScript.Initialise(foodsList);
    }

    public void saveRun()
    {
        SaveSystem.SaveRun(new Run(accessibleButtonsSaveData, GlobalVariables.lastClearedScene, GlobalVariables.nextSceneToPlay, last3FoodChoicesID, Spawner.zoneNumber));
        SaveSystem.SaveCurrentFile(new SaveFile(RunManager.seenTRTs, RunManager.seenFoods, RunManager.seenEnemies));
    }

    public void saveFile()
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
        }
    }

    public void loadRun()
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