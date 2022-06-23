using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodViewer : MonoBehaviour
{
    public List<GameObject> foodsList = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < foodsList.Count; i++)
        {
            if (RunManager.seenFoods[i])
            {
                foodsList[i].transform.Find("MissingEntry").gameObject.SetActive(false);
                foodsList[i].SetActive(true);
            }
            else
            {
                GameObject missing = foodsList[i].transform.Find("MissingEntry").gameObject;
                missing.SetActive(true);
                missing.transform.SetParent(this.transform);
                foodsList[i].SetActive(false);
            }
        }
    }
}
