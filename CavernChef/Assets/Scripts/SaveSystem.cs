using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{

    public static void SaveRun(Run currentRun)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/file" + GlobalVariables.SaveFileID + ".run";
        FileStream stream = new FileStream (path, FileMode.Create);
        formatter.Serialize(stream, currentRun);
        stream.Close();
        Debug.Log("Saved Run");
    }

    public static Run LoadRun()
    {
        string path = Application.persistentDataPath + "/file" + GlobalVariables.SaveFileID + ".run";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream (path, FileMode.Open);
            Run run = formatter.Deserialize(stream) as Run;
            return run;
        }
        else
        {
            Debug.Log("No run save file found in " + path);
            return null;
        }
    }

    public static void SaveCurrentFile(SaveFile currentFile)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/file" + GlobalVariables.SaveFileID + ".alm";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, currentFile);
        stream.Close();
        Debug.Log("Saved File");
    }

    public static SaveFile LoadFile()
    {
        string path = Application.persistentDataPath + "/file" + GlobalVariables.SaveFileID + ".alm";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveFile file = formatter.Deserialize(stream) as SaveFile;
            return file;
        }
        else
        {
            Debug.Log("No save file found in " + path);
            return null;
        }
    }

}
