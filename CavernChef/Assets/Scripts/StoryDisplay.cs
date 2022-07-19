using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryDisplay : MonoBehaviour
{
    public GameObject playButton;
    // Start is called before the first frame update
    void Start()
    {
        if (!SaveSystem.CheckForFirstView())
        {
            SaveSystem.FirstView();
            playButton.SetActive(false);
        }
    }

    public void Reset()
    {
        SaveSystem.DelFirstView();
    }
}
