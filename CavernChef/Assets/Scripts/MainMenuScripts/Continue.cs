using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class Continue : MonoBehaviour
{
    public List<GameObject> foodsList = new List<GameObject>();
    public void accessSave() 
    {
        string path = Application.persistentDataPath + "/file" + GlobalVariables.SaveFileID + ".run";
        if (File.Exists(path))
        {
            Run resumed = SaveSystem.LoadRun();
            for (int i = 0; i < resumed.unlocks.Length; i++)
            {
                Debug.Log(resumed.unlocks[i]);
            }
            RunManager.accessibleButtonsSaveData = resumed.unlocks;
            GlobalVariables.lastClearedScene = resumed.scene;
            GlobalVariables.nextSceneToPlay = resumed.next;
            RunManager.last3FoodChoicesID = resumed.last3FoodChoicesID;
            Spawner.zoneNumber = resumed.numZones;
            SceneManager.LoadScene(3);
            RandomSelectionScript.Initialise(foodsList);
            RunManager.testRun = false;
            RunManager.upgradeCountList = resumed.upgradeCountList;
            for (int i = 0; i < 7; i++)
            {
                Debug.Log("Econ: " + resumed.upgradesEcon[i]);
            }
            RunManager.upgradesEcon = resumed.upgradesEcon;
            RunManager.upgradesFire = resumed.upgradesFire;
            RunManager.upgradesWater = resumed.upgradesWater;
            RunManager.upgradesSnow = resumed.upgradesSnow;
            RunManager.upgradesLight = resumed.upgradesLight;
            RunManager.upgradesElectric = resumed.upgradesElectric;
            RunManager.upgradesEarth = resumed.upgradesEarth;
            RunManager.upgradesBlockage = resumed.upgradesBlockage;
            RunManager.upgradesPudding = resumed.upgradesPudding;
            RunManager.contRun = true;
        }
        else
        {
            Debug.Log("No Previous Run");
        } 
    }
}
