using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SaveSystem.DeleteRun();
        RandomSelectionScript.choiceOne = null;
        RandomSelectionScript.choiceTwo = null;
        RandomSelectionScript.choiceThree = null;
        RunManager.last3FoodChoicesID = null;
        Spawner.zoneNumber = 0;
    }

    public void goBack()
    {
        SceneManager.LoadScene(2);
    }

}