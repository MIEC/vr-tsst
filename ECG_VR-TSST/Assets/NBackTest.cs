using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class NBackTest : AbstractTask
{

    [SerializeField]
    private CharacterAction _greeting;

    [SerializeField]
    private CharacterAction _greetingSecondRound;

    [SerializeField]
    private CharacterAction _task;

    [SerializeField]
    private CharacterAction _thanksNextTask;

    [SerializeField]
    private CharacterAction _thanksNextRound;

    [SerializeField]
    private CharacterAction _endOfTest;

    [SerializeField]
    private Image cross;

    [SerializeField]
    private Text _solution;

    [SerializeField]
    private GameObject[] quads;

    [SerializeField]
    private float _timePerSeries = 2f;

    [SerializeField]
    private bool _showBoardTimer;

    [SerializeField]
    private int backCount;

    private State _state;

    private byte language = 0;

    private byte _sequenceState = 0;

    private bool _paused;

    private List<GameObject> prevQuads = new List<GameObject>();

    private string boardTask;

    private List<Boolean> bools = new List<Boolean>();

    private int _sequenceLength = 30;

    private int[] quadOrder = new int[] { 0, 1, 2, 1, 3, 2, 6, 4, 7, 5, 8, 6, 0, 8, 1, 7, 0, 3, 8, 0, 5, 7, 2, 3, 4, 5, 6, 4, 0, 8};

    private int count = 0;

    private bool firstInSequence;

    private Round round;

    private bool secondRoundActive;

    private bool secondRoundAvailable;

    private string statusOfTask = "";

    private enum State
    {
        Inactive,
        Welcome,
        Initialized,
        TaskAction,
        RunSeries,
        EndSeries,
        ThanksAction
    }


    /// <summary>
    /// Generates the cross for the test
    /// </summary>
    [ContextMenu("StartupTest")]
    private void StartupTest()
    {
        cross.gameObject.SetActive(true);
    }

    /// <summary>
    /// Setup all important values for this task
    /// </summary>
    [ContextMenu("Inactive")]
    protected override void InactiveSetup()
    {
        _state = State.Inactive;
        _sequenceState = 0;
        _paused = false;
        _solution.enabled = false;
        firstInSequence = true;
    }


    /// <summary>
    /// Initialize task by initializing all important values
    /// </summary>
    [ContextMenu("Init")]
    protected override void Init()
    {
        base.Init();
        _state = State.Welcome;
        statusOfTask = CalculateTaskStatus(round, secondRoundActive, secondRoundAvailable);
        StartupTest();
        language = LoadSettings().language;
    }

    /// <summary>
    /// Deactivates th last and activat the next square on th cross
    /// </summary>
    /// <returns></returns>
    private bool ShowNextQuad()
    {
        _solution.enabled = false;
        foreach(GameObject g in quads)
        {
            g.SetActive(false);
        } 

        prevQuads.Add(quads[quadOrder[count]]);
        quads[quadOrder[count]].SetActive(true);

        Debug.Log(_sequenceState + " : " + quads[quadOrder[count]]);

        if (prevQuads.Count > backCount)
        {
            if (prevQuads[prevQuads.Count - (backCount + 1)].Equals(quads[quadOrder[count]]))
            {
                _solution.enabled = true;
                bools.Add(true);
            }
            else
            {
                bools.Add(false);
            }
        }
        count++;
        return true;

    }


    /// <summary>
    /// Main Logic of task. Each call will go to the next state if the state has changed between calls
    /// </summary>
    /// <returns></returns> 

    public override bool Next()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Overloaded Next()-method to use in tasks that require the auditor to talk 
    /// </summary>
    /// <param name="round"></param>
    /// <param name="secondRoundActive"></param>
    /// <param name="secondRoundAvailable"></param>
    /// <returns></returns>
    [ContextMenu("Next")]
    public override bool Next(Round round, bool secondRoundActive, bool secondRoundAvailable)
    {
        switch (_state)
        {
            case State.Inactive:
                WriteLogEntry();
                this.round = round;
                this.secondRoundActive = secondRoundActive;
                this.secondRoundAvailable = secondRoundAvailable;
                Init();
                return Next(round, secondRoundActive, secondRoundAvailable);
            case State.Welcome:
                WriteLogEntry();
                if ((statusOfTask.Equals("FirstOfFirstRound") || statusOfTask.Equals("FirstRoundOnlyTask") || statusOfTask.Equals("FirstRoundOnlyTaskOnlyRound") || statusOfTask.Equals("FirstRoundOnlyTaskNoAnnouncement")) && !LoadSettings().firstIsSecond)
                {
                    ProfessorRitter.Instance.Overwrite = true;
                    ProfessorRitter.Instance.PerformAction(_greeting);
                    Timer.Instance.StartTimer(_greeting.Clip[language].length, Timer.Mode.Automatic);
                    OptionalButtonSwitcher.Instance.DisableAll();
                    MainButtonSwitcher.Instance.Next(false);
                    _state = State.Initialized;
                }
                else if (statusOfTask.Equals("FirstOfSecondRound") || statusOfTask.Equals("SecondRoundOnlyTask") || LoadSettings().firstIsSecond)
                {
                    ProfessorRitter.Instance.Overwrite = true;
                    ProfessorRitter.Instance.PerformAction(_greetingSecondRound);
                    Timer.Instance.StartTimer(_greetingSecondRound.Clip[language].length, Timer.Mode.Automatic);
                    OptionalButtonSwitcher.Instance.DisableAll();
                    MainButtonSwitcher.Instance.Next(false);
                    _state = State.Initialized;
                }
                else if (statusOfTask.Equals("InBetween") || statusOfTask.Equals("EndOfFirstRound") || statusOfTask.Equals("EndOfSecondRound") || statusOfTask.Equals("EndOfTest") || statusOfTask.Equals("EndOfFirstRoundNoAnnouncement"))
                {
                    ProfessorRitter.Instance.PerformAction(_task);
                    Timer.Instance.StartTimer(_task.Clip[language].length, Timer.Mode.Automatic);
                    OptionalButtonSwitcher.Instance.DisableAll();
                    MainButtonSwitcher.Instance.Next(false);
                    _state = State.TaskAction;
                }
                return true;
            case State.Initialized:
                _state = State.TaskAction;
                OptionalButtonSwitcher.Instance.DisableAll();
                MainButtonSwitcher.Instance.Next(false);
                ProfessorRitter.Instance.PerformAction(_task);
                Timer.Instance.StartTimer(_task.Clip[language].length, Timer.Mode.Automatic);
                WriteLogEntry();
                return true;
            case State.TaskAction:
                ProfessorRitter.Instance.Overwrite = false;
                OptionalButtonSwitcher.Instance.ReenableButtons();
                MainButtonSwitcher.Instance.Next(true);
                _state = State.RunSeries;
                WriteLogEntry();
                return true;
            case State.RunSeries:
                BoardTimer.Instance.StopTimer();
                if (firstInSequence)
                {
                    firstInSequence = false;
                    WriteLogEntry();
                }
                
                if (_paused)
                {
                    _paused = false;
                    return true;
                }
                else
                {
                    bool next = RunSequence();
                    if (next)
                    {
                        return Next(round, secondRoundActive, secondRoundAvailable);
                    }
                    else
                    {
                        _state = State.EndSeries;
                        return Next(round, secondRoundActive, secondRoundAvailable);
                    }
                }
            case State.EndSeries:
                MainButtonSwitcher.Instance.Next(false);
                OptionalButtonSwitcher.Instance.DisableAll();
                if (statusOfTask.Equals("FirstOfFirstRound") || statusOfTask.Equals("InBetween") || statusOfTask.Equals("FirstOfSecondRound"))
                {

                    ProfessorRitter.Instance.PerformAction(_thanksNextTask);
                    Timer.Instance.StartTimer(_thanksNextTask.Clip[language].length, Timer.Mode.Automatic);
                }
                else if (statusOfTask.Equals("EndOfFirstRound") || statusOfTask.Equals("FirstRoundOnlyTask"))
                {

                    ProfessorRitter.Instance.PerformAction(_thanksNextRound);
                    Timer.Instance.StartTimer(_thanksNextRound.Clip[language].length, Timer.Mode.Automatic);
                }
                else if (statusOfTask.Equals("EndOfTest") || statusOfTask.Equals("EndOfSecondRound")
                        || statusOfTask.Equals("FirstRoundOnlyTaskOnlyRound") || statusOfTask.Equals("SecondRoundOnlyTask")
                        || statusOfTask.Equals("FirstRoundOnlyTaskNoAnnouncement") || statusOfTask.Equals("EndOfFirstRoundNoAnnouncement"))
                {

                    ProfessorRitter.Instance.PerformAction(_endOfTest);
                    Timer.Instance.StartTimer(_endOfTest.Clip[language].length, Timer.Mode.Automatic);
                }
                _state = State.ThanksAction;
                cross.gameObject.SetActive(false);
                foreach(GameObject g in quads)
                {
                    g.SetActive(false);
                }
                WriteLogEntry();
                return true;
            case State.ThanksAction:
                Finish();
                WriteLogEntry();
                return false;
            default:
                Debug.LogError("[N-BackTask] This state should not exist.");
                return false;
        }
    }

    private bool RunSequence()
    {
        if (_sequenceState < _sequenceLength)
        {
        Timer.Instance.StartTimer(_timePerSeries, Timer.Mode.Automatic);
        if (_showBoardTimer) BoardTimer.Instance.StartTimer(_timePerSeries);
        bool cleared = ShowNextQuad();
        _sequenceState++;
        _paused = true;
        return cleared;
        }
        return false;
    }

/// <summary>
/// Calls finish from base class
/// </summary>
[ContextMenu("Finish")]
    protected override void Finish()
    {
        MainButtonSwitcher.Instance.Next(true);
        base.Finish();
    }


    /// <summary>
    /// Calls restart from base class
    /// </summary>
    [ContextMenu("Restart")]
    public override void Restart()
    {
        base.Restart();
    }

    public override void Restart(Round round, bool secondRoundActive, bool secondRoundAvailable)
    {
        base.Restart(round, secondRoundActive, secondRoundAvailable);
    }

    /// <summary>
    /// Checks the state of the task within the current round to choose the Character action needed accordingly
    /// </summary>
    /// <param name="round"></param>
    /// <param name="secondRoundActive"></param>
    /// <param name="secondRoundAvailable"></param>
    /// <returns>Status of current task</returns>
    private string CalculateTaskStatus(Round round, bool secondRoundActive, bool secondRoundAvailable)
    {
        Debug.Log("available? : " + secondRoundAvailable + "   ;   Current: " + round.currentTask + " ; Wanted: " + (round.tasks.Length - 2));
        //Is a second Round available?
        if (secondRoundAvailable)
        {
            //First or second round?
            if (!secondRoundActive)
            {
                //More than one task?
                if (round.tasks.Length == 6)
                {
                    //Second Round Announcement settings check
                    if (LoadSettings().secondRoundAnnouncement)
                    {
                        return "FirstRoundOnlyTask";
                    }
                    else
                    {
                        return "FirstRoundOnlyTaskNoAnnouncement";
                    }

                }
                else
                {
                    //Beginning, middle or end of first Round?
                    if (round.tasks[round.currentTask - 1].Equals("PositionChange"))
                    {
                        return "FirstOfFirstRound";
                    }
                    else if (round.currentTask == round.tasks.Length - 2)
                    {
                        //Second Round Announcement settings check
                        if (LoadSettings().secondRoundAnnouncement)
                        {
                            return "EndOfFirstRound";
                        }
                        else
                        {
                            return "EndOfFirstRoundNoAnnouncement";
                        }
                    }
                    else
                    {
                        return "InBetween";
                    }
                }


            }
            else
            {
                //More than one task?
                if (round.tasks.Length == 7)
                {
                    return "SecondRoundOnlyTask";
                }
                else
                {
                    //Beginning, middle or end of second Round?
                    if (round.tasks[round.currentTask - 1].Equals("PositionChange"))
                    {
                        return "FirstOfSecondRound";
                    }
                    else if (round.currentTask == round.tasks.Length - 2)
                    {
                        return "EndOfSecondRound";
                    }
                    else
                    {
                        return "InBetween";
                    }
                }
            }
        }
        else
        {
            //More than one task?
            if (round.tasks.Length == 6)
            {
                return "FirstRoundOnlyTaskOnlyRound";
            }
            else
            {
                //Beginning, middle or end of first Round?
                if (round.tasks[round.currentTask - 1].Equals("PositionChange"))
                {
                    return "FirstOfFirstRound";
                }
                else if (round.currentTask == round.tasks.Length - 2)
                {
                    return "EndOfTest";
                }
                else
                {
                    return "InBetween";
                }
            }
        }


    }

    /// <summary>
    /// Writes Log Entry for the specific task
    /// </summary>
    void WriteLogEntry()
    {
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameController");
        string path = gameManager.GetComponent<GameManager>().pathValue;

        string textToWrite = $"Current task: \"N-Back Task\"; Current state of Task: \"{_state.ToString()}\" at {System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}\r\n";

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
    /// Loads settings from file
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

}
