using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ChairHandler : MonoBehaviour {

    public GameObject secondChair;
    public GameObject thirdChair;
    private Setting setting;

    /// <summary>
    /// get settings file from data folder and save as Settings Object
    /// </summary>
    public void Awake()
    {
        string path = Application.dataPath + "/Settings/Settings.txt";
        string str = null;
        setting = new Setting();

        try
        {
            str = File.ReadAllText(path);
            Debug.Log("ausgelesen");
        }
        catch (IOException e)
        {
            Debug.Log("Fehler beim auslesen der Datei:" + e);
        }

        if (!str.Equals(null)) 
        {
            setting = JsonUtility.FromJson<Setting>(str);
        }

        setChairs(setting);
    }

    /// <summary>
    /// Get NPC count from settings object and set the chairs accordingly
    /// </summary>
    /// <param name="setting"></param>
    public void setChairs(Setting setting)
    {
        if (setting.oneAuditor)
        {
            secondChair.SetActive(false);
            thirdChair.SetActive(false);
        }
    }
}
