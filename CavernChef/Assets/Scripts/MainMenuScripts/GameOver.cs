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
        Spawner.waypointList.Clear();
        Spawner.waypointSecondList.Clear();
        GlobalVariables.enemyList.reset();
    }

    public void goBack(bool isVictory)
    {
        if (isVictory)
        {
            GlobalVariables.numOfWins += 1;
        }
        SceneManager.LoadScene(2);

        FindObjectOfType<AudioManager>().PlayMusic("MenuTheme");
    }

}
