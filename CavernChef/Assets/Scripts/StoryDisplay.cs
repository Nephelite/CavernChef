using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryDisplay : MonoBehaviour
{
    public GameObject playButton;

    void Start()
    {
        if (!SaveSystem.CheckForFirstView())
        {
            gameObject.SetActive(false);
        }
    }

    public void select()
    {
        if (!SaveSystem.CheckForFirstView())
        {
            SaveSystem.FirstView();
        }
    }

    public void Reset()
    {
        SaveSystem.DelFirstView();
    }
}
