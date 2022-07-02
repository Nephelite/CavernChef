using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockButtonOne : MonoBehaviour
{
    public List<GameObject> TRTList = new List<GameObject>();
    public int trtIndex;
    public Sprite gold, silver, bronze;

    // Start is called before the first frame update
    void Start()
    {
        Sprite[] rarities = new Sprite[] { bronze, silver, gold };
        trtIndex = 0;
        int iter = 0, index = UpgradesAndUnlocks.firstUnlock;
        if (index > -1)
        {
            trtIndex = index;
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
            gameObject.GetComponent<Button>().image.sprite = rarities[TRTList[trtIndex].GetComponent<TRT>().rarity];
        }
        else
        {
            trtIndex = index;
            while (gameObject.transform.Find(iter.ToString()) != null)
            {
                gameObject.transform.Find(iter.ToString()).gameObject.SetActive(false);
                iter++;

                if (iter > 100)
                {
                    break;
                }
            }
        }
    }

    public void selectUnlock()
    {
        Debug.Log("Unlocking " + trtIndex);
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

        RunManager.accessibleButtonsSaveData[trtIndex] = true;
        UpgradesAndUnlocks.firstUnlock = -1;
        UpgradesAndUnlocks.secondUnlock = -1;
        ButtonDisabler.buttonClicked = true;
    }
}
