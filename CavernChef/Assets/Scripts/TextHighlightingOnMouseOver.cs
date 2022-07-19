using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TextHighlightingOnMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text highlight;

    public void OnPointerEnter(PointerEventData eventData)
    {
        highlight.color = new Color32(55, 112,  197, 255);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highlight.color = new Color32(0, 0, 0, 255);
    }
}
