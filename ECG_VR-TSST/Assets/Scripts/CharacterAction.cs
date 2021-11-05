using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "ChracterAction", menuName = "CharacterAction")]
public class CharacterAction : ScriptableObject
{

    [SerializeField]
    private AudioClip[] _clip;
    public AudioClip[] Clip
    {
        get
        {
            return _clip;
        }
    }

    [SerializeField]
    private string _animationTrigger;
    public string AnimationTrigger
    {
        get
        {
            return _animationTrigger;
        }
    }

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(_animationTrigger) && _clip != null)
        {
            _animationTrigger = _clip[LoadSettings().language].name;
        }
        if (string.IsNullOrEmpty(_animationTrigger) || _clip == null)
        {
            Debug.LogError("[" + this.GetType() + "] A value is null or empy");
        }
        name = _animationTrigger;
    }

    private Setting LoadSettings()
    {
        string path = Application.dataPath + "/Settings.txt";
        string str = "";
        Setting setting = new Setting();

        try
        {
            str = File.ReadAllText(path);
            Debug.Log("ausgelesen");
        }
        catch (IOException e)
        {
            Debug.Log("Fehler beim auslesen der Datei:" + e);
        }

        if (!str.Equals(""))
        {
            setting = JsonUtility.FromJson<Setting>(str);
        }

        return setting;
    }

}