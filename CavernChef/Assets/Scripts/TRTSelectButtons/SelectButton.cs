using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int cost;
    public GameObject TRT, radiusCircle;
    private GameObject visibleRange;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (TRT.GetComponent<AtkTower>() != null && TRT.GetComponent<AtkTower>().Range() > 0)
        {
            Debug.Log("Can place");
            visibleRange = new GameObject();
            visibleRange.layer = 5;
            Image circle = visibleRange.AddComponent<Image>();
            circle.sprite = radiusCircle.GetComponent<SpriteRenderer>().sprite;
            circle.raycastTarget = false;
            circle.maskable = false;
            visibleRange.transform.SetParent(gameObject.transform);
            visibleRange.transform.position = gameObject.transform.position;
            circle.rectTransform.sizeDelta = new Vector2(TRT.GetComponent<AtkTower>().Range() * 50, TRT.GetComponent<AtkTower>().Range() * 50);
            visibleRange.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(visibleRange);
    }
}
