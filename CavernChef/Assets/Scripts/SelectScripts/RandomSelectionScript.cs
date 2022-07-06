using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomSelectionScript : MonoBehaviour
{
    public List<GameObject> foodsList = new List<GameObject>();
    public static List<GameObject> foodsRemaining = new List<GameObject>(); 
    public static GameObject lastChosenFood;
    public static GameObject choiceOne;
    public static GameObject choiceTwo;
    public static GameObject choiceThree;

    public static void Initialise(List<GameObject> foodsList)
    {
        FindObjectOfType<AudioManager>().StopAllAudio();
        FindObjectOfType<AudioManager>().PlayMusic("FoodSelectTheme");
        foodsRemaining = foodsList;
    }

    void Start()
    {
        if (choiceOne == null && choiceTwo == null && choiceThree == null && RunManager.last3FoodChoicesID == null)
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

                RunManager.seenFoods[choiceOne.GetComponent<FoodProperties>().foodID] = true;
                RunManager.seenFoods[choiceTwo.GetComponent<FoodProperties>().foodID] = true;
                RunManager.seenFoods[choiceThree.GetComponent<FoodProperties>().foodID] = true;
                RunManager.last3FoodChoicesID = new int[] { choiceOne.GetComponent<FoodProperties>().foodID, 
                                                            choiceTwo.GetComponent<FoodProperties>().foodID, 
                                                            choiceThree.GetComponent<FoodProperties>().foodID };
            }
        }
        else if (choiceOne != null && choiceTwo != null && choiceThree != null)
        {
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

            RunManager.seenFoods[choiceOne.GetComponent<FoodProperties>().foodID] = true;
            RunManager.seenFoods[choiceTwo.GetComponent<FoodProperties>().foodID] = true;
            RunManager.seenFoods[choiceThree.GetComponent<FoodProperties>().foodID] = true;
        }
        else
        {
            Vector2 pos1 = new Vector2(-8, 0);
            GameObject one = Instantiate(foodsList[RunManager.last3FoodChoicesID[0]], pos1, Quaternion.identity) as GameObject;
            one.transform.SetParent(this.transform);
            one.transform.localScale += new Vector3(4, 4, 4);

            Vector2 pos2 = new Vector2(0, 0);
            GameObject two = Instantiate(foodsList[RunManager.last3FoodChoicesID[1]], pos2, Quaternion.identity) as GameObject;
            two.transform.SetParent(this.transform);
            two.transform.localScale += new Vector3(4, 4, 4);

            Vector2 pos3 = new Vector2(8, 0);
            GameObject three = Instantiate(foodsList[RunManager.last3FoodChoicesID[2]], pos3, Quaternion.identity) as GameObject;
            three.transform.SetParent(this.transform);
            three.transform.localScale += new Vector3(4, 4, 4);

            choiceOne = foodsList[RunManager.last3FoodChoicesID[0]];
            choiceTwo = foodsList[RunManager.last3FoodChoicesID[1]];
            choiceThree = foodsList[RunManager.last3FoodChoicesID[2]];

            RunManager.seenFoods[RunManager.last3FoodChoicesID[0]] = true;
            RunManager.seenFoods[RunManager.last3FoodChoicesID[1]] = true;
            RunManager.seenFoods[RunManager.last3FoodChoicesID[2]] = true;
        }
    }

    public void Selected(int choice)
    {
        switch (choice)
        {
            case 1:
                lastChosenFood = choiceOne;
                Debug.Log("Chose " + lastChosenFood.name);
                foodsRemaining.Add(choiceOne); //remove in future iterations?
                foodsRemaining.Add(choiceTwo);
                foodsRemaining.Add(choiceThree);
                break;

            case 2:
                lastChosenFood = choiceTwo;
                Debug.Log("Chose " + lastChosenFood.name);
                foodsRemaining.Add(choiceOne);
                foodsRemaining.Add(choiceTwo); //remove in future iterations?
                foodsRemaining.Add(choiceThree);
                break;

            case 3:
                lastChosenFood = choiceThree;
                Debug.Log("Chose " + lastChosenFood.name);
                foodsRemaining.Add(choiceOne);
                foodsRemaining.Add(choiceTwo);
                foodsRemaining.Add(choiceThree); //remove in future iterations?
                break;
        }

        choiceOne = null;
        choiceTwo = null;
        choiceThree = null;

        RunManager.last3FoodChoicesID = null;

        int nextSceneIndex = GlobalVariables.nextSceneToPlay;
        if (nextSceneIndex == 0) // Ensures that if the run is fresh, the first stage of the run will be the grasslands scene.
        {
            GlobalVariables.nextSceneToPlay = 4;
            nextSceneIndex = 4;
        }

        FindObjectOfType<AudioManager>().StopAllAudio();
        switch (nextSceneIndex)
        {
            case 4:
                FindObjectOfType<AudioManager>().PlayMusic("GrasslandTheme");
                break;
            case 5:
                FindObjectOfType<AudioManager>().PlayMusic("CaveTheme");
                break;
            case 6:
                FindObjectOfType<AudioManager>().PlayMusic("FloodedCaveTheme");
                break;
            case 7:
                FindObjectOfType<AudioManager>().PlayMusic("MagmaTheme");
                break;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}
