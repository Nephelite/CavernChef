using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UnlockButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public List<GameObject> TRTList = new List<GameObject>();
    public int trtIndex;
    public Sprite gold, silver, bronze;
    public List<Sprite> TRTImages = new List<Sprite>();
    public int ButtonID;
    public TMP_Text placement, description, cost;

    // Start is called before the first frame update
    void Start()
    {
        Sprite[] rarities = new Sprite[] { bronze, silver, gold };
        trtIndex = 0;
        int iter = 0, index = ButtonID == 1 ? UpgradesAndUnlocks.firstUnlock : UpgradesAndUnlocks.secondUnlock;
        if (index > -1)
        {
            gameObject.transform.Find("Tooltip").gameObject.SetActive(false);
            trtIndex = index;
            gameObject.transform.Find("Display").GetComponent<Image>().sprite = TRTImages[trtIndex];
            /*
            while (gameObject.transform.Find(iter.ToString()) != null)
            {
                if (iter != index)
                {
                    gameObject.transform.Find(iter.ToString()).gameObject.SetActive(false);
                }
                if (iter == index)
                {
                    gameObject.transform.Find(iter.ToString()).gameObject.SetActive(true);
                }
                iter++;

                if (iter > 100)
                {
                    break;
                }
            }
            */
            gameObject.GetComponent<Button>().image.sprite = rarities[TRTList[trtIndex].GetComponent<TRT>().rarity];
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void selectUnlock()
    {
        Debug.Log("Unlocking " + trtIndex);
        /*
        int iter = 0;
        while (gameObject.transform.Find(iter.ToString()) != null)
        {
            gameObject.transform.Find(iter.ToString()).gameObject.SetActive(false);
            iter++;

            if (iter > 100)
            {
                break;
            }
        }
        */
        RunManager.accessibleButtonsSaveData[trtIndex] = true;
        UpgradesAndUnlocks.firstUnlock = -1;
        UpgradesAndUnlocks.secondUnlock = -1;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject tooltip = gameObject.transform.Find("Tooltip").gameObject;
        tooltip.SetActive(true);
        placement.text = "Deploy type:\n" + TRTList[trtIndex].GetComponent<TRT>().tooltipDescriptionPlacement;
        description.text = TRTList[trtIndex].GetComponent<TRT>().tooltipDescription;
        cost.text = "Cost: " + TRTList[trtIndex].GetComponent<TRT>().Cost() + " RP";
        Debug.Log("tooptip generate");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.transform.Find("Tooltip").gameObject.SetActive(false);
        Debug.Log("removing tooltip");
    }
}
