using System;
using System.Collections;
using UnityEngine;

public class OnButtonAudioFader : MonoBehaviour
{
    
    private AudioSource _audioSource;
    private Coroutine _fadeRoutine;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void OnButtonClicked()
    {
        StartAudioFade();
    }

    private void StartAudioFade()
    {
        if (_fadeRoutine != null)
            StopCoroutine(_fadeRoutine);

        _fadeRoutine = StartCoroutine(FadeOutCoroutine());
    }
    
    private IEnumerator FadeOutCoroutine()
    {
        var startVolume = _audioSource.volume;
        var time = 0f;

        while (time < 5f)
        {
            time += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(startVolume, 0f, time / 1f);
            yield return null;
        }

        _audioSource.volume = 0f;
        _audioSource.Stop();
    }
}
