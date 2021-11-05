using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadSettings : MonoBehaviour
{
    [SerializeField] private Toggle _firstIsSecond;
    public Dropdown pruefer;
    public Dropdown announcement;
    public Dropdown sprache;
    public Text savedText;
    public Sprite[] allSprites;

    /// <summary>
    /// Gathers every information from the options menu
    /// </summary>
    public void Save()
    {
        Dropdown[] dropdowns = new Dropdown[3];
        dropdowns[0] = pruefer;
        dropdowns[1] = announcement;
        dropdowns[2] = sprache;
        string[] firstRound = getFirstRound();
        string[] secondRound = getSecondRound();
        ConvertSettings(dropdowns, firstRound, secondRound, _firstIsSecond);
    }

    /// <summary>
    /// Loads the existing settings file
    /// </summary>
    public void Load()
    {
        string path = Application.dataPath + "/Settings/Settings.txt";
        string str = "";
        Setting setting = new Setting();
        try
        {
             str = File.ReadAllText(path);
        }catch(IOException e)
        {
            Debug.Log("Fehler beim auslesen der Datei:" + e);
        }

        if (!str.Equals(""))
        {
            setting = JsonUtility.FromJson<Setting>(str);
        }

        if (setting.oneAuditor)
        {
            pruefer.value = 0;
        }
        else
        {
            pruefer.value = 1;
        }

        if (setting.secondRoundAnnouncement)
        {
            announcement.value = 0;
        }
        else
        {
            announcement.value = 1;
        }

        sprache.value = setting.language;

        _firstIsSecond.isOn = setting.firstIsSecond;

        for(int i = 1; i <= 2; i++)
        {
            for(int j = 1; j <= 5; j++)
            {
                GameObject.FindGameObjectWithTag(i + "R" + j + "T").transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                GameObject.FindGameObjectWithTag(i + "R" + j + "T").transform.GetChild(0).gameObject.GetComponent<Image>().sprite = null;
            }
        }

        for(int i = 0; i < setting.firstRound.Length; i++)
        {
            if(setting.firstRound[i] != null)
            {
                for(int j = 0; j < allSprites.Length; j++)
                {
                    if (setting.firstRound[i].Equals(allSprites[j].name))
                    {
                        GameObject.FindGameObjectWithTag("1R" + (i + 1) + "T").transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1); 
                        GameObject.FindGameObjectWithTag("1R" + (i + 1) + "T").transform.GetChild(0).gameObject.GetComponent<Image>().sprite = allSprites[j];
                        GameObject.FindGameObjectWithTag("1R" + (i + 1) + "T").transform.GetChild(0).gameObject.GetComponent<Image>().SetNativeSize();
                    }
                    
                }
                
            }
            
        }

        for (int i = 0; i < setting.secondRound.Length; i++)
        {
            if (setting.secondRound[i] != null)
            {
                for (int j = 0; j < allSprites.Length; j++)
                {
                    if (setting.secondRound[i].Equals(allSprites[j].name))
                    {
                        GameObject.FindGameObjectWithTag("2R" + (i + 1) + "T").transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                        GameObject.FindGameObjectWithTag("2R" + (i + 1) + "T").transform.GetChild(0).gameObject.GetComponent<Image>().sprite = allSprites[j];
                        GameObject.FindGameObjectWithTag("2R" + (i + 1) + "T").transform.GetChild(0).gameObject.GetComponent<Image>().SetNativeSize();
                    }

                }

            }

        }

        savedText.gameObject.GetComponent<Text>().text = "Settings Loaded!";
        savedText.gameObject.SetActive(true);
        StartCoroutine("Wait");


    }

    //Get every task dropped onto the first round drop area
    private string[] getFirstRound()
    {
        string[] firstRound = new string[5];
        for(int i = 0; i < firstRound.Length; i++)
        {
            if (GameObject.FindGameObjectWithTag("1R" + (i + 1) + "T").transform.GetChild(0).gameObject.GetComponent<Image>().sprite)
            {
                firstRound[i] = GameObject.FindGameObjectWithTag("1R" + (i + 1) + "T").transform.GetChild(0).gameObject.GetComponent<Image>().sprite.name;
            }
            
        }
        //delete empty spaces between tasks
        List<string> temp = new List<string>();
        foreach(string s in firstRound)
        {
            if (!string.IsNullOrEmpty(s))
            {
                temp.Add(s);
            }
        }
        firstRound = temp.ToArray();
        return firstRound;
    }

    //Get every task dropped onto the second round drop area
    private string[] getSecondRound()
    {
        string[] secondRound = new string[5];
        for (int i = 0; i < secondRound.Length; i++)
        {
            if (GameObject.FindGameObjectWithTag("2R" + (i + 1) + "T").transform.GetChild(0).gameObject.GetComponent<Image>().sprite)
            {
                secondRound[i] = GameObject.FindGameObjectWithTag("2R" + (i + 1) + "T").transform.GetChild(0).gameObject.GetComponent<Image>().sprite.name;
            }

        }
        //delete empty spaces between tasks
        List<string> temp = new List<string>();
        foreach (string s in secondRound)
        {
            if (!string.IsNullOrEmpty(s))
            {
                temp.Add(s);
            }
        }
        secondRound = temp.ToArray();
        return secondRound;
    }

    /// <summary>
    /// Converts the gathered settings into a json objct and writes the object into a settingss file
    /// </summary>
    /// <param name="dropdowns"></param>
    /// <param name="firstRound"></param>
    /// <param name="secondRound"></param>
    private void ConvertSettings(Dropdown[] dropdowns, string[] firstRound, string[] secondRound, Toggle t)
    {
        bool oneAuditor;
        bool secondRoundAnnouncement;
        bool firstIsSecond;
        byte language;

        if (dropdowns[0].value.Equals(0))
        {
            oneAuditor = true;
        }
        else
        {
            oneAuditor = false;
        }

        if (dropdowns[1].value.Equals(0))
        {
            secondRoundAnnouncement = true;
        }
        else
        {
            secondRoundAnnouncement = false;
        }

        language = (byte)dropdowns[2].value;

        if(firstRound.Length == 0 && secondRound.Length != 0)
        {
            firstRound = secondRound;
            secondRound = null;
        }

        firstIsSecond = t.isOn;

        Setting currentSetting = new Setting(oneAuditor, secondRoundAnnouncement, firstRound, secondRound, language, firstIsSecond);
        string str = JsonUtility.ToJson(currentSetting);
        Directory.CreateDirectory(Application.dataPath + "/Settings");
        string path = Application.dataPath + "/Settings/Settings.txt";

        
        try
        {
            File.WriteAllText(path, str);
        }
        catch (IOException e)
        {
            Debug.Log("Fehler beim schreiben der Datei:" + e);
        }

        savedText.gameObject.GetComponent<Text>().text = "Settings Saved!";
        savedText.gameObject.SetActive(true);
        StartCoroutine("Wait");
    }

    /// <summary>
    /// disables the "Settings Saved!" text after two seconds
    /// </summary>
    /// <returns></returns>
    IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(2f);
        savedText.gameObject.SetActive(false);
    }
}
