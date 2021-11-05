using System.IO;
using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;

public class ChangePositionBack : AbstractTask {

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
    /// Setup all important values for this task
    /// </summary>
    [ContextMenu("Inactive Setup")]
    protected override void InactiveSetup()
    {
        _state = State.Inactive;
    }

    /// <summary>
    /// Initialize task by initializing all important values
    /// </summary>
    [ContextMenu("Init")]
    protected override void Init()
    {
        base.Init();
        _state = State.Initialized;
    }

    /// <summary>
    /// Next method used in tasks that have the virtual agent talking 
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
    /// Main Logic of task. Each call will go to the next state if the state has changed between calls
    /// </summary>
    /// <returns></returns>
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
                _officeRig.SetActive(false);
                _waitingRoomRig.SetActive(true);
                SteamVR_Fade.Start(Color.black, 0f);
                Door.Instance.Close();
                Timer.Instance.StartTimer(0.2f, Timer.Mode.Automatic);
                return true;
            case State.Change:
                _state = State.FadeIn;
                Fader.Instance.Fade(Color.clear, _fadingTime, true, Fader.VisualFadeType.Start);
                Timer.Instance.StartTimer(_fadingTime, Timer.Mode.Automatic);
                return true;
            case State.FadeIn:
                MainButtonSwitcher.Instance.All(true);
                _state = State.Inactive;
                return false;
            default:
                Debug.LogError("[ChangePositionBackTask] This should not have happend.");
                return false;
        }
    }

    /// <summary>
    /// Calls finish from base class
    /// </summary>
    [ContextMenu("Finish")]
    protected override void Finish()
    {
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

    /// <summary>
    /// Writes Log Entry for the specific task
    /// </summary>
    void WriteLogEntry()
    {
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameController");
        string path = gameManager.GetComponent<GameManager>().pathValue;

        string textToWrite = $"Current task: \"ChangePositionBack\"; Current state of Task: \"{_state.ToString()}\" at {System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}\r\n";

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
