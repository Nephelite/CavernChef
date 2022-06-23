using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DeleteFile : MonoBehaviour
{
    public void deleteFile(int id)
    {
        File.Delete(Application.persistentDataPath + "/file" + id + ".alm");
        File.Delete(Application.persistentDataPath + "/file" + id + ".run");
    }
}
