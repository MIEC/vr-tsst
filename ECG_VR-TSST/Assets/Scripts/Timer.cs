using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class Timer : MonoBehaviour
{

    public enum Mode
    {
        Manual,
        Automatic,
        NotRunning
    }

    [SerializeField]
    private Text _timeText;

    [SerializeField]
    private Text _modeText;

    [SerializeField]
    private UnityEvent _timeIsUp = new UnityEvent();

    private IEnumerator _runningTimer;

    private static Timer s_instance;
    public static Timer Instance
    {
        get { return s_instance; }
    }

    private void Awake()
    {
        if (s_instance == null) s_instance = this;
        else Destroy(gameObject);
        DisableTexts();
    }

    private void DisableTexts()
    {
        _timeText.enabled = false;
        _modeText.enabled = false;
    }

    private void EnableTexts()
    {
        _timeText.color = Color.white;
        _timeText.enabled = true;
        _modeText.enabled = true;
    }

    public void StartTimer(float time, Mode mode)
    {
        if (mode == Mode.NotRunning) return;
        if (_runningTimer != null) StopCoroutine(_runningTimer);
        if (mode == Mode.Automatic) _runningTimer = AutoTimer(time);
        else _runningTimer = ManualTimer(time);
        StartCoroutine(_runningTimer);
        EnableTexts();
        _modeText.text = mode.ToString();
    }

    public void StopTimer()
    {
        if (_runningTimer != null) StopCoroutine(_runningTimer);
        DisableTexts();
    }

    private IEnumerator AutoTimer(float time)
    {
        while (time >= 0)
        {
            _timeText.text = SecondsToMinuteString(time);
            time -= Time.deltaTime;
            yield return null;
        }
        DisableTexts();
        _timeIsUp.Invoke();
    }


    private IEnumerator ManualTimer(float time)
    {
        float t = 0;
        while (true)
        {
            _timeText.text = SecondsToMinuteString(t);
            t += Time.deltaTime;
            if (t > time) _timeText.color = Color.red;
            yield return null;
        }
    }

    private string SecondsToMinuteString(float time)
    {
        int mins = Mathf.FloorToInt(time / 60);
        int secs = Mathf.FloorToInt(time) % 60;
        int milis = (int)((time % 1) * 100);
        return mins.ToString("00") + ":" + secs.ToString("00") + "." + milis.ToString("00");
    }

}
