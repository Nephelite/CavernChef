using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AccessSaveFile : MonoBehaviour
{
    public void accessSave(int saveID)
    {
        SceneManager.LoadScene(2);
        GlobalVariables.SaveFileID = saveID;
    }
}
