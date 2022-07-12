using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTile : MonoBehaviour
{
    public GameObject TRT;
    public bool isBlockage;

    private bool CanPlaceTRT()
    {
        return TRT == null;
    }

    void OnMouseDown()
    {
        if (GlobalVariables.selectedTrt != null && GlobalVariables.isDefensiveTRT && CanPlaceTRT())
        {
            TRT = (GameObject)Instantiate(GlobalVariables.selectedTrt, new Vector2(this.transform.position.x + 0.5f, this.transform.position.y + 0.5f), Quaternion.identity);
            if (TRT.GetComponent<BlockageTRT>() != null)
            {
                if (gameObject.transform.Find("WayPointTemplate(Clone)") != null && 
                    gameObject.transform.Find("WayPointTemplate(Clone)").gameObject.GetComponent<WaypointInternals>().enemiesComingToThisWaypoint.Count > 0)
                {
                    bool check = false;
                    for (int i = 0; i < gameObject.transform.Find("WayPointTemplate(Clone)").gameObject.GetComponent<WaypointInternals>().enemiesComingToThisWaypoint.Count; i++)
                    {
                        if (gameObject.transform.Find("WayPointTemplate(Clone)").gameObject.GetComponent<WaypointInternals>().enemiesComingToThisWaypoint[i] != null)
                        {
                            check = true; //There exists an enemy that isn't dead coming to this tile.
                        }
                        else
                        {
                            gameObject.transform.Find("WayPointTemplate(Clone)").gameObject.GetComponent<WaypointInternals>().enemiesComingToThisWaypoint.RemoveAt(i);
                            i--;
                        }
                    }

                    if (check) //Alive enemy coming
                    {
                        Debug.Log("Enemy is coming to this tile");
                        Destroy(TRT);
                        return ;
                    }
                    else //Dead enemy corpse to be ignored
                    {
                        TRT.GetComponent<BlockageTRT>().enemyTile = gameObject.GetComponent<EnemyTile>();
                        isBlockage = true;
                    }
                } //Bug: If placement fails, the tick probably still happens
                else
                {
                    TRT.GetComponent<BlockageTRT>().enemyTile = gameObject.GetComponent<EnemyTile>();
                    isBlockage = true; //change to include a check in near future
                }
            }
            //GameObject parent = this.transform.parent.gameObject;
            //TRT.transform.SetParent(parent.transform);
            //this.transform.SetParent(TRT.transform);
            TRT.transform.SetParent(this.transform);
            GlobalVariables.isDefensiveTRT = false;
            GlobalVariables.selectedTrt = null; //Comment out this line to disable one-at-a-time selection
        }
    }
}
