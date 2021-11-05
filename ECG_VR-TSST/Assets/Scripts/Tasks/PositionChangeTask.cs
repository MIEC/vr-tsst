using System.IO;
using UnityEngine;
using Valve.VR;
public class PositionChangeTask : AbstractTask
{

    private enum State
    {
        Inactive,
        Initialized,
        FadeOut,
        Change,
        FadeIn
    }

    [SerializeField]
    private float _fadingTime;

    [SerializeField]
    private GameObject _waitingRoomRig;

    [SerializeField]
    private GameObject _officeRig;

    private State _state;

    /// <summary>
    /// Setup all important values
    /// </summary>
    [ContextMenu("Inactive Setup")]
    protected override void InactiveSetup()
    {
        _state = State.Inactive;
    }

    /// <summary>
    /// Initialize the Task
    /// </summary>
    [ContextMenu("Init")]
    protected override void Init()
    {
        base.Init();
        _state = State.Initialized;
    }

    /// <summary>
    /// Overloaded Next()-method to use in tasks that require the auditor to talk 
    /// </summary>
    /// <param name="round"></param>
    /// <param name="secondRoundActive"></param>
    /// <param name="secondRoundAvailable"></param>
    /// <returns></returns>
    public override bool Next(Round round, bool secondRoundActive, bool secondRoundAvailable)
    {
        throw new System.NotImplementedException();
    }


    /// <summary>
    /// Main Logic of the task
    /// Go through every state one after another
    /// </summary>
    /// <returns>boolean for the calling class to go on</returns>
    [ContextMenu("Next")]
    public override bool Next()
    {
        WriteLogEntry();
        switch (_state)
        {
            case State.Inactive:
                Init();
                return Next();
            case State.Initialized:
                _state = State.FadeOut;
	            MainButtonSwitcher.Instance.All(false);
                Door.Instance.Open();
	            Fader.Instance.Fade(Color.black, _fadingTime, false, Fader.VisualFadeType.Start);
                Timer.Instance.StartTimer(_fadingTime, Timer.Mode.Automatic);
                return true;
            case State.FadeOut:
                _state = State.Change;
                _waitingRoomRig.SetActive(false);
                _officeRig.SetActive(true);
                SteamVR_Fade.Start(Color.black, 0f);
                Door.Instance.Close();
                Timer.Instance.StartTimer(0.2f, Timer.Mode.Automatic);
                return true;
            case State.Change:
	            _state = State.FadeIn;
	            Fader.Instance.Fade(Color.clear, _fadingTime, true,Fader.VisualFadeType.Start);
                Timer.Instance.StartTimer(_fadingTime, Timer.Mode.Automatic);
                return true;
            case State.FadeIn:              
	            MainButtonSwitcher.Instance.All(true);
                _state = State.Inactive;
                return false;
            default:
                Debug.LogError("[PositionChangeTask] This should not have happend.");
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
        base.Restart();
    }

    /// <summary>
    /// Write log entry for current task
    /// </summary>
    void WriteLogEntry()
    {
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameController");
        string path = gameManager.GetComponent<GameManager>().pathValue;

        string textToWrite =  $"Current task: \"PositionChangeTask\"; Current state of Task: \"{_state.ToString()}\" at { System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}\r\n";

        try
        {
            File.AppendAllText(path, textToWrite);
        }
        catch (IOException e)
        {
            Debug.Log("Fehler beim schreiben der Datei:" + e);
        }
    }

}
