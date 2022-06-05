using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Generates Waypoints on every enemy path tile
public class WaypointGenerator : MonoBehaviour
{
    public GameObject waypointPrefab;
    public static List<GameObject> allWaypoints = new List<GameObject>(); //In case we need the waypoints all in 1 place in the future
    private int xLength = 32, yLength = 11;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < GridGenerator.validEnemyTiles.Count; i++)
        {
            if (GridGenerator.validEnemyTiles[i].name == "EnemyPaths(Clone)")
            {
                GameObject waypoint = Instantiate(waypointPrefab, GridGenerator.validEnemyTiles[i].transform.position + new Vector3(0.5f, 0.5f, 0), Quaternion.identity) as GameObject;
                allWaypoints.Add(waypoint);
                waypoint.transform.SetParent(GridGenerator.validEnemyTiles[i].transform);
                waypoint.GetComponent<WaypointInternals>().tileIndex = i;
            }
        }

        for (int i = 0; i < GridGenerator.validEnemyTiles.Count; i++)
        {
            if (GridGenerator.validEnemyTiles[i].name == "EnemyPaths(Clone)")
            {
                if (i / xLength != yLength - 1 && GridGenerator.validEnemyTiles[i + xLength].name == "EnemyPaths(Clone)") //consider adding a prefab to check, instead of a name
                {
                    GridGenerator.validEnemyTiles[i].transform.Find("WayPointTemplate(Clone)").gameObject.GetComponent<WaypointInternals>().
                        adjWaypoints.Add(GridGenerator.validEnemyTiles[i + xLength].transform.Find("WayPointTemplate(Clone)").gameObject);
                }
                if (i % xLength != 0 && GridGenerator.validEnemyTiles[i - 1].name == "EnemyPaths(Clone)")
                {
                    GridGenerator.validEnemyTiles[i].transform.Find("WayPointTemplate(Clone)").gameObject.GetComponent<WaypointInternals>().
                        adjWaypoints.Add(GridGenerator.validEnemyTiles[i - 1].transform.Find("WayPointTemplate(Clone)").gameObject);
                }
                if (i / xLength != 0 && GridGenerator.validEnemyTiles[i - xLength].name == "EnemyPaths(Clone)")
                {
                    GridGenerator.validEnemyTiles[i].transform.Find("WayPointTemplate(Clone)").gameObject.GetComponent<WaypointInternals>().
                        adjWaypoints.Add(GridGenerator.validEnemyTiles[i - xLength].transform.Find("WayPointTemplate(Clone)").gameObject);
                }
                if (i / xLength != xLength - 1 && GridGenerator.validEnemyTiles[i + 1].name == "EnemyPaths(Clone)")
                {
                    GridGenerator.validEnemyTiles[i].transform.Find("WayPointTemplate(Clone)").gameObject.GetComponent<WaypointInternals>().
                        adjWaypoints.Add(GridGenerator.validEnemyTiles[i + 1].transform.Find("WayPointTemplate(Clone)").gameObject);
                }
            }
        }

        Debug.Log("Waypoints Done");
    }
}
