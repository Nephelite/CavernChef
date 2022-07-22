using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void FirstView()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + ".firstView";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, true);
        stream.Close();
        Debug.Log("View for the first time");
    }

    public static bool CheckForFirstView()
    {
        string path = Application.persistentDataPath + ".firstView";
        return File.Exists(path);
    }

    public static void DelFirstView()
    {
        string path = Application.persistentDataPath + ".firstView";
        File.Delete(path);
    }

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

    public static void DeleteRun()
    {
        string path = Application.persistentDataPath + "/file" + GlobalVariables.SaveFileID + ".run";
        if (File.Exists(path))
        {
            File.Delete(path);
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

    public static void SaveCurrentSettings(Settings currentSettings)
    {
        if (currentSettings == null)
            currentSettings = new Settings(1f, 1f, true);
        Debug.Log("Settings: Music - " + currentSettings.musicVol + " SFX: " + currentSettings.SFXVol);
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/file" + GlobalVariables.SaveFileID + ".set";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, currentSettings);
        stream.Close();
        Debug.Log("Saved File");
    }

    public static Settings LoadSettings()
    {
        string path = Application.persistentDataPath + "/file" + GlobalVariables.SaveFileID + ".set";
        // File.Delete(path);
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            Settings file = formatter.Deserialize(stream) as Settings;
            stream.Close();
            return file;
        }
        else
        {
            Debug.Log("No previous settings found in " + path);
            return new Settings(1f, 1f, true);
        }
    }

    public static Settings LoadAnySetting()
    {
        string path = " ";
        int i = 1;
        while (i < 4 && !File.Exists(path))
        {
            path = Application.persistentDataPath + "/file" + i + ".set";
            i++;
        }

        if (File.Exists(path))
        {
            Debug.Log(path);
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            Settings file = formatter.Deserialize(stream) as Settings;
            stream.Close();
            return file;
        }
        else
        {
            Debug.Log("No previous settings in any file");
            return new Settings(1f, 1f, true);
        }
    }

    public static void ResetAllData()
    {
        DelFirstView();
        for (int i = 0; i < 4; i++)
        {
            string pathRun = Application.persistentDataPath + "/file" + i + ".run";
            string pathAlmanac = Application.persistentDataPath + "/file" + i + ".alm";
            string pathSettings = Application.persistentDataPath + "/file" + i + ".set";
            if (File.Exists(pathRun))
                File.Delete(pathRun);
            if (File.Exists(pathAlmanac))
                File.Delete(pathAlmanac);
            if (File.Exists(pathSettings))
                File.Delete(pathSettings);
        }
    }
}
