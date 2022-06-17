using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodProperties : MonoBehaviour
{
    public int foodID;
    public List<int> associatedUnlocks = new List<int>();
    public List<int> associatedUpgrades = new List<int>();

    //Returns a List of at most 2 random integers from associatedUnlocks, each representing the ID of a TRT that the food can unlock
    //The ID of any TRT that is already unlocked will not show up here.
    public List<int> obtainUnlocks()
    {
        Debug.Log(associatedUnlocks.Count);
        List<int> removed = new List<int>();
        //This loop removes all IDs in associatedUnlocks that have been already unlocked in the save file.
        for (int i = 0; i < associatedUnlocks.Count; i++)
        {
            Debug.Log("Checking: " + associatedUnlocks[i]);
            if (RunManager.accessibleButtonsSaveData[associatedUnlocks[i]])
            {
                Debug.Log("Already Unlocked " + associatedUnlocks[i]);
                removed.Add(associatedUnlocks[i]);
                associatedUnlocks.RemoveAt(i);
                i--;
            }

        }

        //At this juncture, associatedUnlocks will have only indices of TRTs the player has not unlocked.
        
        if (associatedUnlocks.Count < 1) //All unlocks from this food has already been unlocked
        {
            associatedUnlocks.AddRange(removed); //Adds removed unlocks back to the list of possible unlocks. (This might not be needed, but I worry that the data will be preserved into new runs.)
            return null; //All possible upgrades exhausted
        }
        else if (associatedUnlocks.Count == 1) //The food only has 1 unlock left that has not been unlocked by the player
        {
            List<int> res = new List<int>();
            res.Add(associatedUnlocks[0]);
            associatedUnlocks.AddRange(removed); //Adds removed unlocks back to the list of possible unlocks. (This might not be needed, but I worry that the data will be preserved into new runs.)
            Debug.Log("One unlock");
            return res;
        }
        else if (associatedUnlocks.Count == 2) //The food only has 2 unlocks left that has not been unlocked by the player
        {
            List<int> res = new List<int>();
            res.Add(associatedUnlocks[0]);
            res.Add(associatedUnlocks[1]);
            associatedUnlocks.AddRange(removed); //Adds removed unlocks back to the list of possible unlocks. (This might not be needed, but I worry that the data will be preserved into new runs.)
            Debug.Log("Two unlocks");
            return res;
        }
        else
        {
            List<int> res = new List<int>();
            int randomOne = associatedUnlocks[Random.Range(0, associatedUnlocks.Count)];
            res.Add(randomOne);
            associatedUnlocks.Remove(randomOne);
            removed.Add(randomOne);
            int randomTwo = associatedUnlocks[Random.Range(0, associatedUnlocks.Count)];
            res.Add(randomTwo);
            associatedUnlocks.Remove(randomTwo);
            removed.Add(randomTwo);

            associatedUnlocks.AddRange(removed); //Adds removed unlocks back to the list of possible unlocks. (This might not be needed, but I worry that the data will be preserved into new runs.)
            Debug.Log("Two random unlocks");
            return res;
        }
    }
}
