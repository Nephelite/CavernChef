using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlmanacSliding : MonoBehaviour
{
    void Start()
    {
        gameObject.transform.Find("Button").gameObject.SetActive(false);
    }

    void Update()
    {
        if (gameObject.transform.position.y < gameObject.transform.parent.Find("MarkFast").position.y)
        {
            slideFast();
        }
        else if (gameObject.transform.position.y < gameObject.transform.parent.Find("MarkSlow").position.y)
        {
            slideSlow();
        }
        else
        {
            gameObject.transform.Find("Button").gameObject.SetActive(true);
        }
    }

    void slideFast()
    {
        Debug.Log("Sliding");
        Vector3 temp = new Vector3(0, 12.5f, 0);
        gameObject.transform.position += temp;
    }

    void slideSlow()
    {
        Debug.Log("Sliding");
        Vector3 temp = new Vector3(0, 10f, 0);
        gameObject.transform.position += temp;
    }
}
