using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AlmanacEntryDisplay : MonoBehaviour
{
    public Image display;
    public TMP_Text type, ID, TMPText;
    
    public void displayEntry(GameObject entry) //Not the actual GameObjects, but the entry with an AlmanacEntryContainer component
    {
        display.enabled = true;
        display.sprite = entry.GetComponent<AlmanacEntryContainer>().entry.GetComponent<SpriteRenderer>().sprite;
        switch (entry.GetComponent<AlmanacEntryContainer>().type) 
        {
            case 0:
                type.text = "TRT";
                break;
            case 1:
                type.text = "Food";
                break;
            case 2:
                type.text = "Enemy";
                break;
        }

        ID.text = "ID: " + entry.GetComponent<AlmanacEntryContainer>().ID;
        TMPText.text = entry.GetComponent<AlmanacEntryContainer>().entryDescription;
    }

}
