using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRTViewer : MonoBehaviour
{
    public List<GameObject> TRTList = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < TRTList.Count; i++)
        {
            if (RunManager.seenTRTs[i])
            {
                TRTList[i].transform.Find("MissingEntry").gameObject.SetActive(false);
                TRTList[i].SetActive(true);
            }
            else
            {
                GameObject missing = TRTList[i].transform.Find("MissingEntry").gameObject;
                missing.SetActive(true);
                missing.transform.SetParent(this.transform);
                TRTList[i].SetActive(false);
            }
        }
    }
}