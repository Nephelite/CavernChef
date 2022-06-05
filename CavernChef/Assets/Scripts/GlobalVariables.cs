using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalVariables : MonoBehaviour
{
    public static int repelPoints;
    public static GameObject selectedTrt;
    public static List<GameObject> Grid = new List<GameObject>();
    /* POSSIBLE FUTURE OPTIMIZATION
    In the worst case, you could spawn the whole wave before killing anything
    and popping the front element repeatedly would be O(n^2)
    O(n) can be done with self-resizing array and 2 pointers but not now (and possibly not at all)
    */
    public static List<GameObject> enemyList = new List<GameObject>();

    //For pathfinding purposes in the future
    public List<GameObject> enemySpawns = new List<GameObject>();
    public List<GameObject> foodPoints = new List<GameObject>();



    void Update() 
    {
        if (enemyList.Count > 0 && enemyList[0] == null)
        {
            enemyList.RemoveAt(0);
        }
    }
    /*
    void Update()
    {
        if (selectedTrt != null && Input.GetKeyDown(KeyCode.Mouse0))
        {
            int x_pos = (int)Input.mousePosition.x;
            int y_pos = (int)Input.mousePosition.y;

            //if (x_pos >= 39 && x_pos <= 938 && y_pos >= 257.5 && y_pos <= 565.5) 
            //{
                Debug.Log("Registered Input");
                GameObject block = Instantiate(selectedTrt, Input.mousePosition, Quaternion.identity) as GameObject;
                block.transform.SetParent(this.transform);
                Grid.Add(block);
            //}

            selectedTrt = null;
        }
    }
    */
}
