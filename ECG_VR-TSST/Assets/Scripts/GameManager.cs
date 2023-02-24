using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static GameManager s_instance;
    public static GameManager Instance
    {
        get { return s_instance; }
    }

    [SerializeField]
    private WaitingRoomTask _waitingRoom;

    [SerializeField]
    private PositionChangeTask _positionChange;

    [SerializeField]
    private ChangePositionBack _changePositionBack;

    [SerializeField]
    private PresentationTask _presentation;

    [SerializeField]
    private SubtractionSeriesTask _subtraction;

    [SerializeField]
    private ComputeChainTask _computation;

    [SerializeField]
    private NumberSeriesTask _numberSeries;

    [SerializeField]
    private StroopTestTask _stroopTest;

    [SerializeField]
    private ResolveTermsTask _resolveTerms;

    [SerializeField]
    private NBackTest _nBackTest;

    [SerializeField]
    private SpeechDreamJob _speechDreamJob;

    [SerializeField]
    private SpeechProjectManager _speechProjectManager;

    [SerializeField]
    private SpeechChiefPhysician _speechChiefPhysician;

    [SerializeField]
    private float _fadingTime = 2f;

    [SerializeField] private string[] preRound = { "Uninitialized", "Start", "WaitingRoom", "PositionChange" };

    [SerializeField] private string[] preRoundTwo = {"ChangePositionBack" ,"Uninitialized", "Start", "WaitingRoom", "PositionChange" };

    [SerializeField] private string[] firstRound = null;

    [SerializeField] private string endRound = "End";

    [SerializeField] private string switchRound = "Switch";

    [SerializeField] private string participantNumber;

    [SerializeField] private bool secondRoundAvailable = false;

    [SerializeField] private bool secondRoundActive = false;

    [SerializeField] private bool uninitialized, start, waitingRoom, positionChange, changePositionBack, end, presentation, speechDreamJob, speechProjectManager, speechChiefPhysician, numberSeries, computeChain, subtraction, stroop, resolveTerms, nBackTest;

    [SerializeField] private Round round;

    [SerializeField] private Round secondRound;

    [SerializeField] private string path;

    /// <summary>
    /// Getter for path value
    /// </summary>
    public string pathValue
    {
        get { return path; }
    }

    /// <summary>
    /// Prepare the round Objects before Start() is called.
    /// Check if instance already exists
    /// </summary>
    private void Awake()
    {
        if (s_instance == null) s_instance = this;
        else
        {
            Debug.LogWarning("[GameManager] Instance already set. Destroying.");
            Destroy(this);
        }

        XRSettings.enabled = true;

        Directory.CreateDirectory(Application.dataPath + "/Logs");

        path = getWholePath();

        WriteLogEntry(false);
        uninitialized = start = waitingRoom = positionChange = changePositionBack = end = presentation = speechDreamJob = speechProjectManager = speechChiefPhysician = numberSeries = computeChain = subtraction = stroop = resolveTerms = nBackTest = true;
        GameObject NPCHandler = GameObject.FindGameObjectWithTag("NPCHandler");
        NPCHandler.GetComponent<NPCHandler>().setNPCs(LoadSettings());

        string[] firstRoundSettings = LoadSettings().firstRound;
        string[] secondRoundSettings = LoadSettings().secondRound;

        for(int i = 0; i < firstRoundSettings.Length; i++)
        {
            Debug.Log(firstRoundSettings[i]);
        }

        if (secondRoundSettings.Length > 0)
        {
            secondRoundAvailable = true;

            round = makeRound(firstRoundSettings, secondRoundAvailable, true);

            secondRound = makeRound(secondRoundSettings, secondRoundAvailable, false);
        }
        else
        {
            round = makeRound(firstRoundSettings, secondRoundAvailable, true);
        }

    }

    /// <summary>
    /// Start the Test by calling Next() for the first Time
    /// </summary>
    private void Start()
    {
        Next();
    }

    /// <summary>
    /// Main Logic of task. Each call will go to the next state if the state has changed between calls
    /// </summary>
    [ContextMenu("Next")]
    public void Next()
    {
        Timer.Instance.StopTimer();
        bool keepState = true;
        switch (round.tasks[round.currentTask])
        {
            case "Uninitialized":
                if (uninitialized)
                {
                    WriteLogEntry(round, false);
                    uninitialized = false;
                }
                Fader.Instance.Fade(Color.black, 0f, false, Fader.VisualFadeType.Start);
                OptionalButtonSwitcher.Instance.DisableAll();
                MainButtonSwitcher.Instance.Repeat(false);
                keepState = false;
                break;
            case "Start":
                if (start)
                {
                    WriteLogEntry(round, false);
                    start = false;
                }
                Fader.Instance.Fade(Color.clear, _fadingTime, true, Fader.VisualFadeType.Start);
                keepState = false;
                break;
            case "WaitingRoom":
                if (waitingRoom)
                {
                    WriteLogEntry(round, false);
                    waitingRoom = false;
                }
                MainButtonSwitcher.Instance.Repeat(true);
                keepState = _waitingRoom.Next();
                break;
            case "PositionChange":
                if (positionChange)
                {
                    WriteLogEntry(round, false);
                    positionChange = false;
                }
                keepState = _positionChange.Next();
                break;
            case "ChangePositionBack":
                if (changePositionBack)
                {
                    WriteLogEntry(round, false);
                    changePositionBack = false;
                }
                keepState = _changePositionBack.Next();
                break;
            case "Presentation":
                if (presentation)
                {
                    WriteLogEntry(round, false);
                    presentation = false;
                }
                keepState = _presentation.Next(round, secondRoundActive, secondRoundAvailable);
                break;
            case "SpeechDreamJob":
                if (speechDreamJob)
                {
                    WriteLogEntry(round, false);
                    speechDreamJob = false;
                }
                keepState = _speechDreamJob.Next(round, secondRoundActive, secondRoundAvailable);
                break;
            case "SpeechProjectManager":
                if (speechProjectManager)
                {
                    WriteLogEntry(round, false);
                    speechProjectManager = false;
                }
                keepState = _speechProjectManager.Next(round, secondRoundActive, secondRoundAvailable);
                break;
            case "SpeechChiefPhysician":
                Debug.Log("Here");
                if (speechChiefPhysician)
                {
                    WriteLogEntry(round, false);
                    speechChiefPhysician = false;
                }
                keepState = _speechChiefPhysician.Next(round, secondRoundActive, secondRoundAvailable);
                break;
            case "Subtraction":
                if (subtraction)
                {
                    WriteLogEntry(round, false);
                    subtraction = false;
                }
                keepState = _subtraction.Next(round, secondRoundActive, secondRoundAvailable);
                break;
            case "Computation":
                if (computeChain)
                {
                    WriteLogEntry(round, false);
                    computeChain = false;
                }
                keepState = _computation.Next(round, secondRoundActive, secondRoundAvailable);
                break;
            case "NumberSeries":
                if (numberSeries)
                {
                    WriteLogEntry(round, false);
                    numberSeries = false;
                }
                keepState = _numberSeries.Next(round, secondRoundActive, secondRoundAvailable);
                break;
            case "Stroop":
                if (stroop)
                {
                    WriteLogEntry(round, false);
                    stroop = false;
                }
                keepState = _stroopTest.Next(round, secondRoundActive, secondRoundAvailable);
                break;
            case "ResolveTerms":
                if (resolveTerms)
                {
                    WriteLogEntry(round, false);
                    resolveTerms = false;
                }
                keepState = _resolveTerms.Next(round, secondRoundActive, secondRoundAvailable);
                break;
            case "NBack":
                if (nBackTest)
                {
                    WriteLogEntry(round, false);
                    nBackTest = false;
                }
                keepState = _nBackTest.Next(round, secondRoundActive, secondRoundAvailable);
                break;
            case "End":
                    OptionalButtonSwitcher.Instance.DisableAll();
                    MainButtonSwitcher.Instance.Next(false);
                    MainButtonSwitcher.Instance.Repeat(true);
                    Fader.Instance.Fade(Color.black, _fadingTime, false, Fader.VisualFadeType.Start);
                    WriteLogEntry("End Study");
                    XRSettings.enabled = false;
                    StartCoroutine(this.LoadSceneWithDelay(1));
                break;
            case "Switch":
                WriteLogEntry(true);
                round = secondRound;
                secondRoundActive = true;
                uninitialized = start = waitingRoom = positionChange = changePositionBack = end = presentation = speechDreamJob = speechProjectManager = speechChiefPhysician = numberSeries = computeChain = subtraction = stroop = resolveTerms = nBackTest = true;
                Next();
                break;
            default:
                break;
        }

        if (!keepState)
        {
            round.currentTask++;
            if (round.tasks[round.currentTask] != "Start")
            {
                Next();
            }
               
        }

    }

    /// <summary>
    /// Here the calls from the singl tasks will result in a restart of said task
    /// </summary>
    [ContextMenu("Restart")]
    public void RestartTask()
    {
        WriteLogEntry(round, true);
        Timer.Instance.StopTimer();
        switch (round.tasks[round.currentTask])
        {
            case "Uninitialized":
                break;
            case "Start":
                break;
            case "PositionChange":
                break;
            case "ChangePositionBack":
                break;
            case "WaitingRoom":
                _waitingRoom.Restart();
                break;
            case "Presentation":
                _presentation.Restart(round, secondRoundActive, secondRoundAvailable);
                break;
            case "SpeechProjectManager":
                _speechProjectManager.Restart(round, secondRoundActive, secondRoundAvailable);
                break;
            case "SpeechDreamJob":
                _speechDreamJob.Restart(round, secondRoundActive, secondRoundAvailable);
                break;
            case "SpeechChiefPhysician":
                _speechChiefPhysician.Restart(round, secondRoundActive, secondRoundAvailable);
                break;
            case "Subtraction":
                _subtraction.Restart(round, secondRoundActive, secondRoundAvailable);
                break;
            case "Computation":
                _computation.Restart(round, secondRoundActive, secondRoundAvailable);
                break;
            case "NumberSeries":
                _numberSeries.Restart(round, secondRoundActive, secondRoundAvailable);
                break;
            case "Stroop":
                _stroopTest.Restart(round, secondRoundActive, secondRoundAvailable);
                break;
            case "ResolveTerms":
                _resolveTerms.Restart(round, secondRoundActive, secondRoundAvailable);
                break;
            case "NBack":
                _nBackTest.Restart(round, secondRoundActive, secondRoundAvailable);
                break;
            case "End":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
                break;
            default:
                break;
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

    /// <summary>
    /// Craete a Round object from the settings
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="secondRound"></param>
    /// <param name="IsFirstRound"></param>
    /// <returns></returns>
    private Round makeRound(string[] settings, bool secondRound, bool IsFirstRound)
    {
        List<string> orderOfEvents = new List<string>(settings);
        if (secondRound)
        {
            if (IsFirstRound)
            {
                orderOfEvents.InsertRange(0, preRound);
                orderOfEvents.Add(switchRound);
            }
            else
            {
                orderOfEvents.InsertRange(0, preRoundTwo);
                orderOfEvents.Add(endRound);
            }

        }
        else
        {
            orderOfEvents.InsertRange(0, preRound);
            orderOfEvents.Add(endRound);

        }

        return new Round(orderOfEvents.ToArray(), 0);
    }

    /// <summary>
    /// Log Entry mthod for starting and rstarting a task
    /// </summary>
    /// <param name="round"></param>
    /// <param name="restart"></param>
    void WriteLogEntry(Round round, bool restart)
    {
        string cutter = "\r\n--------------------------------------\r\n\r\n";
        string roundlog = "";

        if (restart)
        {
            roundlog = $"Restarted task \"{round.tasks[round.currentTask]}\" at {System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}\r\n";
        }
        else
        {
            roundlog = $"Started task \"{round.tasks[round.currentTask]}\" at {System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}\r\n";
        }

        string textToWrite = cutter + roundlog;

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
    /// Log Entry method for writing a wanted txt into the logfile
    /// </summary>
    /// <param name="text">wanted text to write into the log file</param>
    void WriteLogEntry(string text)
    {
        string cutter = "\r\n--------------------------------------\r\n\r\n";
        string textToWrite = cutter + text + " ; at " + System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

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
    /// Log Entry method for the beginning of the Test and the beginning of the second round
    /// </summary>
    /// <param name="secondRound"></param>
    void WriteLogEntry(bool secondRound)
    {
        string cutter = "";
        string textToWrite = "";

        if (secondRound)
        {
            cutter = "\r\n--------------------------------------\r\n\r\n";
            textToWrite = $"Begin second Round at: {System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}.\r\n";
        }
        else
        {
            Setting setting = LoadSettings();
            string language = "";
            switch (setting.language)
            {
                case 0:
                    language = "Deutsch";
                    break;
                case 1:
                    language = "English";
                    break;
                default:
                    language = "Language setting error";
                    break;

            }
            string first = "[";
            for(int i = 0; i < setting.firstRound.Length; i++)
            {
                if(i < setting.firstRound.Length - 1)
                {
                    first +=  setting.firstRound[i] + ", ";
                }
                else
                {
                    first += setting.firstRound[i];
                }
            }
            first += "]";

            string second = "[";
            for (int i = 0; i < setting.secondRound.Length; i++)
            {
                if (i < setting.secondRound.Length - 1)
                {
                    second += setting.secondRound[i] + ", ";
                }
                else
                {
                    second += setting.secondRound[i];
                }
            }
            second += "]";
            textToWrite = $"Begin Study at: {System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}.\r\n\r\nSettings used:\r\nOne Auditor?: {setting.oneAuditor}\r\nSecond round announcement?: {setting.secondRoundAnnouncement}\r\nLanguage: {language}\r\nFirst round: {first}\r\nSecond round: {second}\r\n\r\nParticipant Number: {participantNumber}\r\n";
        }

        string wholeText = cutter + textToWrite;

        try
        {
            File.AppendAllText(path, wholeText);
        }
        catch (IOException e)
        {
            Debug.Log("Fehler beim schreiben der Datei:" + e.Message);
        }
    }

    /// <summary>
    /// calculates the whole path for the logfile, including time, date and participant number
    /// </summary>
    /// <returns></returns>
    private string getWholePath()
    {
        string participantpath = Application.dataPath + "/ParticipantNumber" + "/PN.txt";
        string wholePath = "";
        participantNumber = "";
        string restOfPath = "/Logs" + "/Log" + System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
        try
        {
            participantNumber = File.ReadAllText(participantpath);
        }
        catch (IOException e)
        {
            Debug.Log("Fehler beim auslesen der Datei:" + e);
        }
        wholePath = $"{Application.dataPath}{restOfPath}_ParticipantNumber_{participantNumber}.txt";
        return wholePath;
    }

    IEnumerator<object> LoadSceneWithDelay(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        SceneManager.LoadScene(0);
    }

}
