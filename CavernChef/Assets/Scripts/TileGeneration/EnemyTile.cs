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
            //GameObject parent = this.transform.parent.gameObject;
            //TRT.transform.SetParent(parent.transform);
            //this.transform.SetParent(TRT.transform);
            TRT.transform.SetParent(this.transform);
            GlobalVariables.isDefensiveTRT = false;
            GlobalVariables.selectedTrt = null; //Comment out this line to disable one-at-a-time selection

            if (TRT.GetComponent<BlockageTRT>() != null)
                isBlockage = true; //change to include a check in near future
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
