using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;
using System.IO;

public class SubtractionSeriesTask : AbstractTask
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
    private Text _taskText;

    [SerializeField]
    private Text _solution;

    [SerializeField]
    private int _origin = 1022;

    [SerializeField]
    private int _subtrahend = 13;

    [SerializeField]
    private float _time = 300f;

    private State _state;

    private byte language = 0;

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
	    TaskTime,
	    ThanksAction
    }

    /// <summary>
    /// Generates the solution text for the experimenters view
    /// </summary>
    [ContextMenu("GenerateSolutionText")]
    private void GenerateSolutionText()
    {
        int minuend = _origin;
        StringBuilder sb = new StringBuilder();
        for (byte b = 0; minuend > 0; b++)
        {
            if (b % 5 == 0 && b != 0) sb.AppendLine();
            else if (b != 0) sb.Append("\t\t");
            sb.Append(minuend.ToString());
            minuend -= _subtrahend;
        }
        _solution.text = sb.ToString();
    }


    /// <summary>
    /// Setup all important values
    /// </summary>
    [ContextMenu("Inactive")]
    protected override void InactiveSetup()
    {
        _state = State.Inactive;
        _taskText.enabled = false;
        _solution.enabled = false;
    }

    /// <summary>
    /// Initialize the Task
    /// </summary>
    [ContextMenu("Init")]
    protected override void Init()
    {
        base.Init();
        _state = State.Welcome;
        _taskText.enabled = true;
        _taskText.text = _origin + " - " + _subtrahend + " ...";
        _solution.enabled = true;
        statusOfTask = CalculateTaskStatus(round, secondRoundActive, secondRoundAvailable);
        Debug.Log(statusOfTask);
        GenerateSolutionText();
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
        WriteLogEntry();
        switch (_state)
        {
            case State.Inactive:
                this.round = round;
                this.secondRoundActive = secondRoundActive;
                this.secondRoundAvailable = secondRoundAvailable;
                Init();
                return Next(round, secondRoundActive, secondRoundAvailable);
            case State.Welcome:
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
                return true;
            case State.TaskAction:
                ProfessorRitter.Instance.Overwrite = false;
                OptionalButtonSwitcher.Instance.ReenableButtons();
                MainButtonSwitcher.Instance.Next(true);
                _state = State.TaskTime;
                Timer.Instance.StartTimer(_time, Timer.Mode.Manual);
                return true;
            case State.TaskTime:
	            OptionalButtonSwitcher.Instance.DisableAll();
	            MainButtonSwitcher.Instance.Next(false);
	            _state = State.ThanksAction;
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
                return true;
            case State.ThanksAction:
	            Finish();
	            return false;
            default:
                Debug.LogError("[SubtractionSeriesTask] This state should not exist.");
                return false;
        }
    }

    /// <summary>
    /// Call Finish from base class
    /// </summary>
    [ContextMenu("Finish")]
    protected override void Finish()
	{
		MainButtonSwitcher.Instance.Next(true);
        base.Finish();
    }

    /// <summary>
    /// Call restart from main class
    /// </summary>
    [ContextMenu("Restart")]
    public override void Restart()
    {
        base.Restart();
    }

    /// <summary>
    /// Ovreloadeed Restart() method for tasks that require the auditor to talk
    /// </summary>
    /// <param name="round"></param>
    /// <param name="secondRoundActive"></param>
    /// <param name="secondRoundAvailable"></param>
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
    /// Writes an Entry into the current Log-File
    /// </summary>
    void WriteLogEntry()
    {
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameController");
        string path = gameManager.GetComponent<GameManager>().pathValue;

        string textToWrite = $"Current task: \"SubtractionTask\"; Current state of Task: \"{_state.ToString()}\" at {System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}\r\n";

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
    /// <returns>Sttings object</returns>
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
