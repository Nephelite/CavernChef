using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomSelectionScript : MonoBehaviour
{
    public static List<GameObject> foodsRemaining = new List<GameObject>(); 
    public static GameObject lastChosenFood;
    public static GameObject choiceOne;
    public static GameObject choiceTwo;
    public static GameObject choiceThree;

    public static void Initialise(List<GameObject> foodsList)
    {
        foodsRemaining = foodsList;
    }

    void Start()
    {
        lastChosenFood = null; //Resets previous choice
        int len = foodsRemaining.Count;
        if (len <= 2)
        {
            Debug.Log("Error, too few foods left");
        }
        else
        {
            int first = Random.Range(0, len);
            int second = Random.Range(0, len - 1);
            int third = Random.Range(0, len - 2);
            choiceOne = foodsRemaining[first];
            foodsRemaining.RemoveAt(first);
            choiceTwo = foodsRemaining[second];
            foodsRemaining.RemoveAt(second);
            choiceThree = foodsRemaining[third];
            foodsRemaining.RemoveAt(third);

            Vector2 pos1 = new Vector2(-8, 0);
            GameObject one = Instantiate(choiceOne, pos1, Quaternion.identity) as GameObject;
            one.transform.SetParent(this.transform);
            one.transform.localScale += new Vector3(4, 4, 4);
            
            Vector2 pos2 = new Vector2(0, 0);
            GameObject two = Instantiate(choiceTwo, pos2, Quaternion.identity) as GameObject;
            two.transform.SetParent(this.transform);
            two.transform.localScale += new Vector3(4, 4, 4);

            Vector2 pos3 = new Vector2(8, 0);
            GameObject three = Instantiate(choiceThree, pos3, Quaternion.identity) as GameObject;
            three.transform.SetParent(this.transform);
            three.transform.localScale += new Vector3(4, 4, 4);
        }
    }

    public void Selected(int choice)
    {
        switch (choice)
        {
            case 1:
                lastChosenFood = choiceOne;
                Debug.Log("Chose " + lastChosenFood.name);
                foodsRemaining.Add(choiceTwo);
                foodsRemaining.Add(choiceThree);
                choiceOne = null;
                choiceTwo = null;
                choiceThree = null;
                break;

            case 2:
                lastChosenFood = choiceTwo;
                Debug.Log("Chose " + lastChosenFood.name);
                foodsRemaining.Add(choiceOne);
                foodsRemaining.Add(choiceThree);
                choiceOne = null;
                choiceTwo = null;
                choiceThree = null;
                break;

            case 3:
                lastChosenFood = choiceThree;
                Debug.Log("Chose " + lastChosenFood.name);
                foodsRemaining.Add(choiceOne);
                foodsRemaining.Add(choiceTwo);
                choiceOne = null;
                choiceTwo = null;
                choiceThree = null;
                break;
        }

        int nextSceneIndex = GlobalVariables.nextSceneToPlay;
        if (nextSceneIndex == 0) // Ensures that if the run is fresh, the first stage of the run will be the grasslands scene.
        {
            GlobalVariables.nextSceneToPlay = 4;
            nextSceneIndex = 4;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}
