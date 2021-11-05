using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class saveLoadWallText : MonoBehaviour {

    /// <summary>
    /// Check if a File for the Waiting Room texts already exist. If thats not the case, make one, so that on each fresh boot in a new environment a text is available
    /// </summary>
    private void Awake()
    {
        Directory.CreateDirectory(Application.dataPath + "/WallText");
        string path = Application.dataPath + "/WallText/WallText" + 0 + ".txt";
        string str = "";
        WaitingRoomText waitText = new WaitingRoomText();

        try
        {
            str = File.ReadAllText(path);
            Debug.Log("ausgelesen");
        }
        catch (IOException e)
        {
            Debug.Log("Fehler beim auslesen der Datei:" + e);
        }

        if (str.Equals(""))
        {
            waitText = new WaitingRoomText("Bitte haben Sie noch einen Moment Geduld! Sie können gleich zu dem/den Prüfer(n) ins Büro, um mit den Aufgaben zu starten.", "Im später folgenden Gespräch werden sowohl Bild als auch Ton aufgezeichnet werden!", "Bitte stehen Sie auf.\r\nDer/Die Prüfer ist/sind jetzt für Sie bereit.");
            str = JsonUtility.ToJson(waitText);

            Debug.Log(waitText.initialText);

            try
            {
                File.WriteAllText(path, str);
            }
            catch (IOException e)
            {
                Debug.Log("Fehler beim schreiben der Datei:" + e);
            }

            path = Application.dataPath + "/WallText/WallText" + 1 + ".txt";
            waitText = new WaitingRoomText("Please be patient for a moment! You can go straight to the auditor(s) into the office to start with the tasks any moment.", "In the following conversation both picture and sound will be recorded!", "Please stand up.\r\nThe auditor(s) is/are now ready for you.");
            str = JsonUtility.ToJson(waitText);

            try
            {
                File.WriteAllText(path, str);
            }
            catch (IOException e)
            {
                Debug.Log("Fehler beim schreiben der Datei:" + e);
            }
        }
        else
        {
            //Nothing to do
        }

    }	
    
}
