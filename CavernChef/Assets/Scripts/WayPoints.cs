using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    public GameObject[] waypoints;
    public GameObject testEnemyPrefab;


    // Start is called before the first frame update
    void Start()
    {
        Vector2 start = new Vector2(0,0);
        //Use the line below to test enemy spawn and movement (Towers will not target this enemy)
        //Instantiate(testEnemyPrefab, start, Quaternion.identity).GetComponent<Wispy>().waypoints = waypoints;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
