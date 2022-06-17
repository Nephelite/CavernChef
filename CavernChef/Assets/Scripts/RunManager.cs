using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunManager : MonoBehaviour
{
    public List<GameObject> foodsList = new List<GameObject>();
    public List<GameObject> TRTIndexedList = new List<GameObject>();
    //List: 0 - Econ, 1 - Fire, 2 - Water, 3 - Snow, 4 - Light, 5 - Electric, 6 - Earth 
    public GameObject TRTMenu;
    public static bool testRun = true; //For making game testing easier. Remove in final product.
    public static bool[] accessibleButtonsSaveData = new bool[32]; //NOTE: Change this if needed, it has to be at least greater than the number of TRT tpes placeable.;

    private int testNum = 2; //Change if testing of a certain TRT is needed

    void Start()
    {
        if (testRun)
        {
            for (int i = 0; i < testNum; i++)
            {
                accessibleButtonsSaveData[i] = true;
            }
        }

        for (int i = 0; i < TRTIndexedList.Count; i++)
        {
            TRTIndexedList[i].SetActive(accessibleButtonsSaveData[i]);
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
        SceneManager.LoadScene(3);
    }

    public void newRun()
    {
        for (int i = 0; i < 2; i++)
        {
            accessibleButtonsSaveData[i] = true;
        }

        SceneManager.LoadScene(3);
        RandomSelectionScript.Initialise(foodsList);

    }

    public void saveRun()
    {
        SaveSystem.SaveRun(new Run(accessibleButtonsSaveData, GlobalVariables.lastClearedScene, GlobalVariables.nextSceneToPlay));
    }

    public void loadRun()
    {
        Run currentRun = SaveSystem.LoadRun();
        accessibleButtonsSaveData = currentRun.unlocks;
        
        for (int i = 0; i < TRTIndexedList.Count; i++)
        {
            if (accessibleButtonsSaveData[i])
            {
                TRTIndexedList[i].SetActive(true);
            }
        }

        //Next scene generate
    }

    void newButtonAdded(int i)
    {
        GameObject buttonToAssign = TRTIndexedList[i];
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