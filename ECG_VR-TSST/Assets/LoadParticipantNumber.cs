using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadParticipantNumber : MonoBehaviour {


    [SerializeField]
    private InputField inputField;

    private string path;

    /// <summary>
    /// On Program start up search for the file that holds the last Participant Number and create a directory for the file if it is absent
    /// If it does not exist, use 1 as a default number
    /// </summary>
    private void Awake()
    {
        Directory.CreateDirectory(Application.dataPath + "/ParticipantNumber");
        path = Application.dataPath + "/ParticipantNumber" + "/PN.txt";
        string text = "";

        if (File.Exists(path))
        {
            try
            {
                text = File.ReadAllText(path);
            }
            catch (IOException e)
            {
                Debug.Log("Fehler beim auslesen der Datei:" + e);
            }
        }
        else
        {
            text = "1";
        }

        inputField.text = text;
    }
}
