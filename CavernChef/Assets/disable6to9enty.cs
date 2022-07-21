using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disable6to9enty : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.Find("ForestSlime").gameObject.SetActive(false);
        gameObject.transform.Find("Skeleton").gameObject.SetActive(false);
        gameObject.transform.Find("FloodedSlime").gameObject.SetActive(false);
        gameObject.transform.Find("MagmaSlime").gameObject.SetActive(false);
    }
}
