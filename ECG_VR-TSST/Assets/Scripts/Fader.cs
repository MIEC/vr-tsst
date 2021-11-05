using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Valve.VR;

public class Fader : MonoBehaviour
{

    public enum VisualFadeType
    {
        Start,
        View
    }

    private static Fader s_instance;
    public static Fader Instance
    {
        get
        {
            return s_instance;
        }
    }

    [SerializeField]
    private AudioMixer _audio;

    [SerializeField]
    private string _volumePropertyName = "MasterVolume";

    private float _initialAudioVolume;

    private void Awake()
    {
        _audio.GetFloat(_volumePropertyName, out _initialAudioVolume);
        if (!s_instance)
        {
            s_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(this);
    }

    public void Fade(Color color, float time, bool fadeIn = true, VisualFadeType type = VisualFadeType.Start)
    {
        if (type == VisualFadeType.Start) SteamVR_Fade.Start(color, time);
        else if (type == VisualFadeType.View) SteamVR_Fade.View(color, time);
        StartCoroutine(FadeAudio(time, fadeIn));
    }

    private IEnumerator FadeAudio(float time, bool fadeIn)
    {
        float timer, targetVolume, currentVolume, delta;
        timer = time;
        _audio.GetFloat(_volumePropertyName, out currentVolume);
        if (fadeIn)
        {
            targetVolume = _initialAudioVolume;
        }
        else
        {
            targetVolume = -50f;
            delta = currentVolume - targetVolume;
        }
        delta = targetVolume - currentVolume;
        while (timer > 0)
        {
            //_audio.SetFloat(_volumePropertyName, currentVolume);
            currentVolume += delta / time * Time.deltaTime;
            timer -= Time.deltaTime;
            yield return null;
        }
        _audio.SetFloat(_volumePropertyName, targetVolume);
    }

}
