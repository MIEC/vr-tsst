using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UiLog : MonoBehaviour
{

    [SerializeField]
    private float _resetTime = 3f;

    private Text _text;
    private float _timer;

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
        _text = gameObject.GetComponent<Text>();
        _text.enabled = false;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
        _text.enabled = false;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        _text.enabled = true;
        switch (type)
        {
            case LogType.Assert:
                _text.color = Color.red;
                break;
            case LogType.Error:
                _text.color = Color.red;
                break;
            case LogType.Exception:
                _text.color = Color.red;
                break;
            case LogType.Log:
                _text.color = Color.white;
                break;
            case LogType.Warning:
                _text.color = Color.yellow;
                break;
            default:
                _text.color = Color.white;
                break;
        }
        _text.text = logString;
        if (_timer > 0) StartCoroutine(ResetRoutine());
        _timer = _resetTime;
    }

    private IEnumerator ResetRoutine()
    {
        while (_timer > 0)
        {
            _timer -= Time.deltaTime;
            yield return null;
        }
        _text.enabled = false;
    }

}
