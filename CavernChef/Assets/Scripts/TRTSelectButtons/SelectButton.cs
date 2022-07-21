using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SelectButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int cost, upgradeCount;
    public GameObject TRT, radiusCircle, message, readyToDeployMessage;
    private GameObject visibleRange;
    public Sprite level0, level1, level2, level3;
    public float cd;
    public GameObject costDisplayObject;

    void Awake()
    {
        upgradeCount = RunManager.upgradeCountList[TRT.GetComponent<TRT>().TRTID];
        Debug.Log("Upgrade Count: " + upgradeCount);
        if (upgradeCount < 2 || level1 == null)
        {
            gameObject.GetComponent<Image>().sprite = level0;
        }
        else if (upgradeCount < 4 || level2 == null)
        {
            gameObject.GetComponent<Image>().sprite = level1;
        }
        else if (upgradeCount < 6 || level3 == null)
        {
            gameObject.GetComponent<Image>().sprite = level2;
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = level3;
        }
        GameObject costDisplay = Instantiate(costDisplayObject, gameObject.transform.position + new Vector3(35, -35, 0), Quaternion.identity) as GameObject;
        costDisplay.transform.SetParent(gameObject.transform);
        costDisplay.GetComponent<TextMeshProUGUI>().text = TRT.GetComponent<TRT>().Cost().ToString();
    }

    public float GetCooldown()
    {
        return TRT.GetComponent<TRT>().TBetPlacements();
    }

    public float GetCurrentCooldownProgress()
    {
        if (TRT.GetComponent<EconTRT>() != null)
            return EconTRT.firstPlacement ? TRT.GetComponent<TRT>().TBetPlacements() : Time.time - EconTRT.lastPlacedTime;
        else if (TRT.GetComponent<FireTRT>() != null)
            return FireTRT.firstPlacement ? TRT.GetComponent<TRT>().TBetPlacements() : Time.time - FireTRT.lastPlacedTime;
        else if (TRT.GetComponent<WaterTRT>() != null)
            return WaterTRT.firstPlacement ? TRT.GetComponent<TRT>().TBetPlacements() : Time.time - WaterTRT.lastPlacedTime;
        else if (TRT.GetComponent<SnowTRT>() != null)
            return SnowTRT.firstPlacement ? TRT.GetComponent<TRT>().TBetPlacements() : Time.time - SnowTRT.lastPlacedTime;
        else if (TRT.GetComponent<LightTRT>() != null)
            return LightTRT.firstPlacement ? TRT.GetComponent<TRT>().TBetPlacements() : Time.time - LightTRT.lastPlacedTime;
        else if (TRT.GetComponent<ElectricTRT>() != null)
            return ElectricTRT.firstPlacement ? TRT.GetComponent<TRT>().TBetPlacements() : Time.time - ElectricTRT.lastPlacedTime;
        else if (TRT.GetComponent<EarthTRT>() != null)
            return EarthTRT.firstPlacement ? TRT.GetComponent<TRT>().TBetPlacements() : Time.time - EarthTRT.lastPlacedTime;
        else if (TRT.GetComponent<BlockageTRT>() != null)
            return BlockageTRT.firstPlacement ? TRT.GetComponent<TRT>().TBetPlacements() : Time.time - BlockageTRT.lastPlacedTime;
        else if (TRT.GetComponent<StallTRT>() != null)
            return StallTRT.firstPlacement ? TRT.GetComponent<TRT>().TBetPlacements() : Time.time - StallTRT.lastPlacedTime;
        else
            return 0;
    }

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
            circle.rectTransform.sizeDelta = new Vector2(TRT.GetComponent<AtkTower>().Range() * 48, TRT.GetComponent<AtkTower>().Range() * 48);
            visibleRange.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(visibleRange);
    }

    private void removeMessage()
    {
        Destroy(message);
    }

    public void displayErrorMessage(string msg)
    {
        if (message == null)
        {
            message = new GameObject();
            message.AddComponent<TextMeshProUGUI>();
            message.GetComponent<TextMeshProUGUI>().text = msg;
            message.layer = 4;
            message.transform.position = gameObject.transform.position + new Vector3(-100, -10, 0);
            message.GetComponent<TextMeshProUGUI>().fontSize = 24;
            message.GetComponent<TextMeshProUGUI>().raycastTarget = false;
            message.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 50);
            message.transform.SetParent(gameObject.transform.parent.parent.parent);
            Invoke("removeMessage", 1.0f);
        }
    }

    private void removeDeployMessage()
    {
        Destroy(readyToDeployMessage);
    }

    public void displayDeployMessage()
    {
        if (readyToDeployMessage == null)
        {
            readyToDeployMessage = new GameObject();
            readyToDeployMessage.AddComponent<TextMeshProUGUI>();
            readyToDeployMessage.GetComponent<TextMeshProUGUI>().text = "Ready";
            readyToDeployMessage.layer = 4;
            readyToDeployMessage.transform.position = gameObject.transform.position + new Vector3(-60, -10, 0);
            readyToDeployMessage.GetComponent<TextMeshProUGUI>().fontSize = 24;
            readyToDeployMessage.GetComponent<TextMeshProUGUI>().raycastTarget = false;
            readyToDeployMessage.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 50);
            readyToDeployMessage.transform.SetParent(gameObject.transform.parent.parent.parent);
            Invoke("removeDeployMessage", 0.5f);
        }
        if (visibleRange != null)
            Destroy(visibleRange);
    }
}
