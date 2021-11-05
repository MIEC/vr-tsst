using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class NumberSeriesTask : AbstractTask
{

    private enum State
    {
        Inactive,
        Initialized,
        Welcome,
        TaskAction,
        RunSeries,
        EndSeries,
        ThanksAction
    }

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
    private string[] _sequence;

    [SerializeField]
    private Text _boardText;

    [SerializeField]
    private Text _solutionText;

    [SerializeField]
    private bool _showBoardTimer;

    [SerializeField]
    private float _timePerSeries = 15f;

    private State _state = State.Inactive;

    private byte _sequenceState = 0;

    private bool _paused;

    private bool firstTime;

    private byte language = 0;

    private Round round;

    private bool secondRoundActive;

    private bool secondRoundAvailable;

    private string statusOfTask = "";

    /// <summary>
    /// Setup all important values
    /// </summary>
    [ContextMenu("Inactive Setup")]
    protected override void InactiveSetup()
    {
        _sequenceState = 0;
        _paused = false;
        _boardText.enabled = false;
        _solutionText.enabled = false;
        _state = State.Inactive;
    }

    /// <summary>
    /// Initialize the Task
    /// </summary>
    [ContextMenu("Init")]
    protected override void Init()
    {
        base.Init();
        OptionalButtonSwitcher.Instance.DisableAll();
        _state = State.Welcome;
        _solutionText.enabled = true;
        _boardText.enabled = true;
        firstTime = true;
        statusOfTask = CalculateTaskStatus(round, secondRoundActive, secondRoundAvailable);
        _boardText.text = "";
        language = LoadSettings().language;
    }

    /// <summary>
    /// Main Logic of the task
    /// Go through every state one after another
    /// </summary>
    /// <returns>boolean for the calling class to go on</returns>
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
                WriteLogEntry();
                MainButtonSwitcher.Instance.Next(false);
                OptionalButtonSwitcher.Instance.DisableAll();
                ProfessorRitter.Instance.PerformAction(_task);
                Timer.Instance.StartTimer(_task.Clip[language].length, Timer.Mode.Automatic);
                _state = State.TaskAction;
                return true;
            case State.TaskAction:
                WriteLogEntry();
                ProfessorRitter.Instance.Overwrite = false;
                _state = State.RunSeries;
                MainButtonSwitcher.Instance.Next(true);
                OptionalButtonSwitcher.Instance.ReenableButtons();
                return true;
            case State.RunSeries:
                if (firstTime)
                {
                    firstTime = false;
                    WriteLogEntry();
                }
                BoardTimer.Instance.StopTimer();
                if (_paused)
                {
                    _paused = false;
                    _boardText.text = "";
                    return true;
                }
                else
                {
                    bool next = RunSequence();
                    if (next) return true;
                    else goto case State.EndSeries;
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
                return true;
            case State.ThanksAction:
                Finish();
                return false;
            default:
                Debug.LogError("[NumberSeriesTask] Should not be in this state");
                return false;
        }
    }

    /// <summary>
    /// Call Finish from base class
    /// </summary>
    [ContextMenu("Finish")]
    protected override void Finish()
    {
        base.Finish();
    }

    /// <summary>
    /// Call restart from base class
    /// </summary>
    [ContextMenu("Restart")]
    public override void Restart()
    {
        BoardTimer.Instance.StopTimer();
        base.Restart();
    }

    /// <summary>
    /// Ovreloaded Restart() method for tasks that require the auditor to talk
    /// </summary>
    /// <param name="round"></param>
    /// <param name="secondRoundActive"></param>
    /// <param name="secondRoundAvailable"></param>
    public override void Restart(Round round, bool secondRoundActive, bool secondRoundAvailable)
    {
        BoardTimer.Instance.StopTimer();
        base.Restart(round, secondRoundActive, secondRoundAvailable);
    }

    /// <summary>
    /// Shows the next text on the whiteboard
    /// </summary>
    /// <returns></returns>
    private bool RunSequence()
    {
        if (_sequenceState < _sequence.Length)
        {
            Timer.Instance.StartTimer(_timePerSeries, Timer.Mode.Automatic);
            if (_showBoardTimer) BoardTimer.Instance.StartTimer(_timePerSeries);
            _boardText.text = _sequence[_sequenceState];
            _sequenceState++;
            _paused = true;
            return true;
        }
        return false;
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
    /// Writes an Entry into the current Log-File
    /// </summary>
    void WriteLogEntry()
    {
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameController");
        string path = gameManager.GetComponent<GameManager>().pathValue;

        string textToWrite = $"Current task: \"NumberSeriesTask\"; Current state of Task: \"{_state.ToString()}\" at {System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}\r\n";

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
    /// Loads the settings for the settings file
    /// </summary>
    /// <returns>settings object</returns>
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
