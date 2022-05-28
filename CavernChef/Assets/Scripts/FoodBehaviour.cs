using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBehaviour : MonoBehaviour
{
    public GameObject defaultFood; //For Testing purposes only, to allow for individual testing of the game without clicking through the menus
    // Start is called before the first frame update
    void Start()
    {
        GameObject chosen = RandomSelectionScript.lastChosenFood == null ? defaultFood : RandomSelectionScript.lastChosenFood;
        GameObject food = Instantiate(chosen, new Vector2(0, 0), Quaternion.identity) as GameObject;
        Debug.Log(food.transform.position.x + " " + food.transform.position.y);
        food.transform.SetParent(this.transform);
        food.transform.position = this.transform.position;
    }
}
