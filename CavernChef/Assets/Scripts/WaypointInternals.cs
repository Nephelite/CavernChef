using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointInternals : MonoBehaviour
{
    public int tileIndex;
    public List<GameObject> adjWaypoints = new List<GameObject>();
    private int xLength = 32, yLength = 11;
    public bool isEnemyOnTile = false; //True if enemy is on the tile this waypoint is on. Do not allow for obstructions to be placed on waypoints with this variable being true.
}
