using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewRun : MonoBehaviour
{
    public List<GameObject> foodsList = new List<GameObject>();
    public void Begin()
    {
        SceneManager.LoadScene(1);
        RandomSelectionScript.Initialise(foodsList);

    }
}
