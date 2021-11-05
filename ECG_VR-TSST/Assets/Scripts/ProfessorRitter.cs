using System.IO;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class ProfessorRitter : MonoBehaviour
{

    private static ProfessorRitter s_instance;
    public static ProfessorRitter Instance
    {
        get
        {
            return s_instance;
        }
    }

    public bool Overwrite
    {
        get
        {
            return overwrite;
        }

        set
        {
            overwrite = value;
        }
    }

    private Animator _animator;
    private AudioSource _audio;
    private byte language = 0;

    private bool overwrite = false;

    /// <summary>
    /// Check if instance already exists.
    /// set important values
    /// </summary>
    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }
        else
        {
            Debug.LogWarning("[ProfessorRitter] This is not the first instance. Destroying.");
            Destroy(gameObject);
        }
        _audio = gameObject.GetComponent<AudioSource>();
        _animator = gameObject.GetComponent<Animator>();
        language = LoadSettings().language;
    }

    /// <summary>
    /// Play talking animation as long as the audio iss playing and/or the overwrite variable is set to true 
    /// </summary>
    private void Update()
    {
        if(overwrite)
        {
            _animator.SetBool("talk", true);
        }
        else
        {
            if (_audio.isPlaying)
            {
                _animator.SetBool("talk", true);
            }
            else
            {
                _animator.SetBool("talk", false);
            }
        }

    }

    /// <summary>
    /// Lets the auditor talk
    /// </summary>
    /// <param name="action"></param>
    public void PerformAction(CharacterAction action)
    {

        _audio.clip = action.Clip[language];
        _audio.Play();
        WriteLogEntry(action.name);
        //_animator.SetTrigger(action.AnimationTrigger);
        Debug.Log("[ProfesorRitter] Start action playback of " + action.name);
        
    }

    /// <summary>
    /// Log Entry method for each character action the auditor can perform
    /// </summary>
    /// <param name="action"></param>
    void WriteLogEntry(string action)
    {
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameController");
        string path = gameManager.GetComponent<GameManager>().pathValue;
        string textToWrite;


        switch (action)
        {
            case "TimeIsNotUp":
                textToWrite = $"Admin pressed button: \"Time is not up\" at {System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}\r\n";
                break;
            case "CannotUnderstand":
                textToWrite = $"Admin pressed button: \"Cannot understand\" at {System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}\r\n";
                break;
            case "LoudAndClearly":
                textToWrite = $"Admin pressed button: \"Loud and clearly\" at {System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}\r\n";
                break;
            case "FromTheBeginning":
                textToWrite = $"Admin pressed button: \"From the beginning\" at {System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}\r\n";
                break;
            case "Wrong":
                textToWrite = $"Admin pressed button: \"Wrong\" at {System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}\r\n";
                break;
            case "Correct":
                textToWrite = $"Admin pressed button: \"Correct\" at {System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}\r\n";
                break;
            case "Result":
                textToWrite = $"Admin pressed button: \"Result\" at {System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}\r\n";
                break;
            case "Greeting":
                textToWrite = $"NPC  says \"Welcome\" at {System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}\r\n";
                break;
            case "Greeting2ndRound":
                textToWrite = $"NPC  says \"Welcome to second round\" at {System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}\r\n";
                break;
            case "ThanksNextTask":
                textToWrite = $"NPC says \"Thank you! Next Task.\" at {System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}\r\n";
                break;
            case "ThanksNextRound":
                textToWrite = $"NPC says \"Thank you! Next round follows soon.\" at {System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}\r\n";
                break;
            case "EndOfTest":
                textToWrite = $"NPC says \"Thank you! This is the end of the Test.\" at {System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}\r\n";
                break;
            case "ComputeChain":
                textToWrite = $"NPC says \"Compute chain explanation\" at {System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}\r\n";
                break;
            case "SpeechChiefPhysician":
                textToWrite = $"NPC says \"Speech explanation for Chief Phisician\" at {System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}\r\n";
                break;
            case "SpeechProjectManager":
                textToWrite = $"NPC says \"Speech explanation for Project Manager\" at {System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}\r\n";
                break;
            case "SpeechDreamJob":
                textToWrite = $"NPC says \"Speech explanation Dream Job\" at {System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}\r\n";
                break;
            case "StroopTest":
                textToWrite = $"NPC says \"Stroop test explanation\" at {System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}\r\n";
                break;
            case "Subtraction":
                textToWrite = $"NPC says \"Subtraction series explanation\" at {System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}\r\n";
                break;
            case "NBack":
                textToWrite = $"NPC says \"NBack test explanation\" at {System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}\r\n";
                break;
            case "ResolveTerms":
                textToWrite = $"NPC says \"Resolve terms explanation\" at {System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}\r\n";
                break;
            case "NumberSeries":
                textToWrite = $"NPC says \"Number series explanation\" at {System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}\r\n";
                break;
            default:
                textToWrite = $"An error occured in the Log-System at {System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}\r\n";
                break;

        } 

        try
        {
            File.AppendAllText(path, textToWrite);
        }
        catch (IOException e)
        {
            Debug.Log("Fehler beim schreiben der Datei:" + e);
        }
    }

    /// <summary>
    /// Load the settings from the settings file
    /// </summary>
    /// <returns></returns>
    private Setting LoadSettings()
    {
        string path = Application.dataPath + "/Settings/Settings.txt";
        string str = "";
        Setting setting = new Setting();

        try
        {
            str = File.ReadAllText(path);
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
