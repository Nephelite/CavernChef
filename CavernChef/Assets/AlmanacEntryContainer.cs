using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlmanacEntryContainer : MonoBehaviour
{
    public GameObject entry;
    public int ID, type; //type 0 - TRT, 1 - Food, 2 - enemy
    [TextArea]
    public string entryDescription;
}
//Remove beef jerky, meatball, omelette, popcorn, Pepperoni, Waffles