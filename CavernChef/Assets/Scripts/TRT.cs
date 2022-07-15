using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TRT : MonoBehaviour
{
    public int TRTID, rarity, upgradeCount;
    public string tooltipDescriptionPlacement, tooltipDescription;
    public Sprite level0, level1, level2, level3;

    public abstract void Reset();

    void Awake()
    {
        upgradeCount = RunManager.upgradeCountList[TRTID];
        Debug.Log("Upgrade Count: " + upgradeCount);
        if (upgradeCount < 2 || level1 == null)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = level0;
        }
        else if (upgradeCount < 4 || level2 == null)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = level1;
        }
        else if (upgradeCount < 6 || level3 == null)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = level2;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = level3;
        }
    }
}
