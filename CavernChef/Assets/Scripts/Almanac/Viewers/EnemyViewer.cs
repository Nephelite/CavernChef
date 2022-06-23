using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyViewer : MonoBehaviour
{
    public List<GameObject> enemyList = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (RunManager.seenEnemies[i])
            {
                enemyList[i].transform.Find("MissingEntry").gameObject.SetActive(false);
                enemyList[i].SetActive(true);
            }
            else
            {
                GameObject missing = enemyList[i].transform.Find("MissingEntry").gameObject;
                missing.SetActive(true);
                missing.transform.SetParent(this.transform);
                enemyList[i].SetActive(false);
            }
        }
    }
}
