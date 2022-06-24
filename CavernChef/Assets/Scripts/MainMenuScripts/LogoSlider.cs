using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoSlider : MonoBehaviour
{
    void Update()
    {
        if (gameObject.transform.position.y < gameObject.transform.parent.Find("Mark").position.y)
        {
            slide();
        }
        else if (gameObject.GetComponent<Image>().color.a < 1f)
        {
            var logo = gameObject.GetComponent<Image>();
            logo.color = new Color(logo.color.r, logo.color.g, logo.color.b, logo.color.a + 0.01f);
        }
    }

    void slide()
    {
        Debug.Log("Sliding");
        Vector3 temp = new Vector3(0, 2.5f, 0);
        gameObject.transform.position += temp;
        var logo = gameObject.GetComponent<Image>();
        logo.color = new Color(logo.color.r, logo.color.g, logo.color.b, logo.color.a + 0.01f);
    }
}
