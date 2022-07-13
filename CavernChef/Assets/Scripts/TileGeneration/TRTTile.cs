using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRTTile : MonoBehaviour
{
    private GameObject TRT, visibleRange, ghostTRT;
    public GameObject radiusCircle;

    private bool CanPlaceTRT()
    {
        return TRT == null;
    }

    void OnMouseDown()
    {
        if (GlobalVariables.selectedTrt != null && GlobalVariables.isOffensiveTRT && CanPlaceTRT())
        {
            TRT = (GameObject) Instantiate (GlobalVariables.selectedTrt, new Vector2(this.transform.position.x + 0.5f, this.transform.position.y + 0.5f), Quaternion.identity);
            //GameObject parent = this.transform.parent.gameObject;
            //TRT.transform.SetParent(parent.transform);
            //this.transform.SetParent(TRT.transform);
            TRT.transform.SetParent(this.transform);
            GlobalVariables.isOffensiveTRT = false;
            GlobalVariables.selectedTrt = null; //Comment out this line to disable one-at-a-time selection
            Destroy(visibleRange);
        }
    }

    void OnMouseOver()
    {
        if (GlobalVariables.isOffensiveTRT && CanPlaceTRT() && visibleRange == null && GlobalVariables.selectedTrt.GetComponent<AtkTower>() != null)
        {
            visibleRange = Instantiate(radiusCircle, gameObject.transform.position + new Vector3(0.5f, 0.5f, 0), Quaternion.identity) as GameObject;
            visibleRange.transform.localScale += new Vector3(GlobalVariables.selectedTrt.GetComponent<AtkTower>().range * 0.78f, 
                                                            GlobalVariables.selectedTrt.GetComponent<AtkTower>().range * 0.78f, 
                                                            0);
            //new Vector3(0.4, 0.4, 0) is unit circle for the radius circle

            ghostTRT = new GameObject();
            ghostTRT.transform.position = gameObject.transform.position + new Vector3(0.5f, 0.5f, 0);
            SpriteRenderer trt = ghostTRT.AddComponent<SpriteRenderer>();
            trt.sprite = GlobalVariables.selectedTrt.GetComponent<SpriteRenderer>().sprite;
            ghostTRT.transform.localScale *= 1.5f;
        }
        else if (GlobalVariables.isOffensiveTRT && CanPlaceTRT() && ghostTRT == null && GlobalVariables.selectedTrt.GetComponent<EconTRT>() != null)//Econ TRT
        {
            ghostTRT = new GameObject();
            ghostTRT.transform.position = gameObject.transform.position + new Vector3(0.5f, 0.5f, 0);
            SpriteRenderer trt = ghostTRT.AddComponent<SpriteRenderer>();
            trt.sprite = GlobalVariables.selectedTrt.GetComponent<SpriteRenderer>().sprite;
            ghostTRT.transform.localScale *= 3;
        }
    }

    void OnMouseExit()
    {
        if (visibleRange != null)
            Destroy(visibleRange);

        if (ghostTRT != null)
            Destroy(ghostTRT);
    }
}
