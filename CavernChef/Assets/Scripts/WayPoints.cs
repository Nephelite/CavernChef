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
        Instantiate(testEnemyPrefab).GetComponent<Wispy>().waypoints = waypoints;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
