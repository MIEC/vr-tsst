using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveParticipantNumber : MonoBehaviour {

    [SerializeField]
    private InputField inputField;

    private string path;

    /// <summary>
    /// Clicking on start will save the current participant number to a file
    /// </summary>
    public void OnButtonClicked()
    {
        path = Application.dataPath + "/ParticipantNumber" + "/PN.txt";
        string text = inputField.text;

        try
        {
            File.WriteAllText(path, text);
        }
        catch (IOException e)
        {
            Debug.Log("Fehler beim auslesen der Datei:" + e);
        }

    }
}
