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
            Debug.Log(resumed.scene + " " + resumed.next);
            RunManager.accessibleButtonsSaveData = resumed.unlocks;
            GlobalVariables.lastClearedScene = resumed.scene;
            GlobalVariables.nextSceneToPlay = resumed.next;
            RunManager.last3FoodChoicesID = resumed.last3FoodChoicesID;
            SceneManager.LoadScene(3);
            RandomSelectionScript.Initialise(foodsList);
            RunManager.testRun = false;
        }
        else
        {
            Debug.Log("No Previous Run");
        } 
    }
}
