using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class BoardTimer : MonoBehaviour
{

    private static BoardTimer _instance;
    public static BoardTimer Instance
    {
        get { return _instance; }
    }

    [SerializeField]
    private Text _timeText;

    [SerializeField]
    private AudioSource _audioSrc;

    private IEnumerator _timer;

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else
        {
            Debug.LogWarning("[BoardTimer] Instance already exists. Destroying");
            Destroy(this);
        }
    }
	
	private void OnEnable(){
		StopTimer();
	}
	
	private void OnDisable() {
		StopTimer();
	}

	public void StartTimer(float time)
    {
        _timer = StopWatchRoutine(time);
        StartCoroutine(_timer);
    }

	public void StopTimer()
    {
	    if (_timer != null) StopCoroutine(_timer);
        DisableText();
        StopAudio();
    }

    private void EnableText()
    {
        _timeText.enabled = true;
    }

    private void DisableText()
    {
        _timeText.enabled = false;
    }

    private void PlayAudio()
    {
        _audioSrc.Play();
    }

    private void StopAudio()
    {
        _audioSrc.Stop();
    }

    private IEnumerator StopWatchRoutine(float time)
    {
        EnableText();
        PlayAudio();
        while (time > 0)
        {
            _timeText.text = time.ToString("0.00") + "s";
            time -= Time.deltaTime;
            yield return null;
        }
        DisableText();
        StopAudio();
    }

}
