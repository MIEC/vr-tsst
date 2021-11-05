using System;
using UnityEngine;
using UnityEngine.UI;

public class TextSequencer : MonoBehaviour
{

    private enum State
    {
        Inactive,
        Initialized,
        Sequencing
    }

    [SerializeField]
    private string[] _sequence;

    [SerializeField]
    private bool _autoSequence = true;
	
	[SerializeField]
	private bool _showBoardTimer = false;
	
    [SerializeField]
    private float _autoNextTime = 3f;

    [SerializeField]
	private Text _uiText;
	
    private byte _sequenceState;
    private State _state = State.Inactive;

    private void OnEnable()
    {
        InactiveSetup();
    }

    private void OnDisable()
    {
        InactiveSetup();
    }

    [ContextMenu("Inactive")]
    public void InactiveSetup()
    {
        if (_uiText)
        {
            _uiText.enabled = false;
        }
        _state = State.Inactive;
    }

    [ContextMenu("Init")]
    protected void Init()
    {
        _uiText.enabled = true;
        _uiText.text = "";
        _sequenceState = 0;
        _state = State.Initialized;
    }

    [ContextMenu("Next")]
    public bool Next()
    {
        switch (_state)
        {
            case State.Inactive:
                Init();
                return Next();
            case State.Initialized:
                _state = State.Sequencing;
                return Next();
            case State.Sequencing:
                bool keepState = RunSequence();
                if (!keepState)
                {
                    Finish();
                }
                return keepState;
            default:
                Debug.LogError("[TextSequencer] This state should not be reached.");
                return false;
        }
    }

    private bool RunSequence()
	{
		 if (_sequenceState < _sequence.Length)
		 {
			if (_showBoardTimer) BoardTimer.Instance.StartTimer(_autoNextTime);  
            _uiText.text = _sequence[_sequenceState];
            if (_autoSequence) Timer.Instance.StartTimer(_autoNextTime, Timer.Mode.Automatic);
            _sequenceState++;
            return true;
        }
        return false;
    }

    [ContextMenu("Finish")]
    protected void Finish()
    {
        InactiveSetup();
    }

    [ContextMenu("Restart")]
    public void Restart()
    {
        InactiveSetup();
        Init();
        Next();
    }



}
