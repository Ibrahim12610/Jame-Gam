using System;
using System.Collections;
using UnityEngine;

public class AudioSourceFader : MonoBehaviour
{
    [SerializeField] private float durationDelay;
    [SerializeField] private float durationOfFade;
    private AudioSource _audioSource;
    private Coroutine _fadeRoutine;
    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        StartCoroutine(HandleSceneChange());
    }
    
    private IEnumerator HandleSceneChange()
    {
        yield return new WaitForSeconds(durationDelay);
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

        while (time < durationDelay)
        {
            time += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(startVolume, 0f, time / durationOfFade);
            yield return null;
        }

        _audioSource.volume = 0f;
        _audioSource.Stop();
    }
}
