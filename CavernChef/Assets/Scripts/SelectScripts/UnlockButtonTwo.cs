using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockButtonTwo : MonoBehaviour
{
    public int trtIndex;

    // Start is called before the first frame update
    void Start()
    {
        trtIndex = 0;
        int iter = 0, index = UpgradesAndUnlocks.secondUnlock;
        if (index > -1)
        {
            Debug.Log("Offer: " + index);
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
