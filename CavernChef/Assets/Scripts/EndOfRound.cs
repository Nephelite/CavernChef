using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfRound : MonoBehaviour
{
    
    public void end()
    {
        GlobalVariables.lastClearedScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(8);
    }

}
