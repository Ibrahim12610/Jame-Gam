using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TextFader : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float fadeDelay = 0f;

    [Header("Interaction Control")]
    [SerializeField] private bool disableInteractionWhenHidden = true;

    private CanvasGroup _canvasGroup;
    private Coroutine _fadeRoutine;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0f;
    }

    private void Start()
    {
        FadeIn();
    }

    private void FadeIn()
    {
        StartFade(0f, 1f, true);
    }

    private void StartFade(float startAlpha, float targetAlpha, bool enableInteraction)
    {
        if (_fadeRoutine != null)
            StopCoroutine(_fadeRoutine);

        _fadeRoutine = StartCoroutine(FadeRoutine(startAlpha, targetAlpha, enableInteraction));
    }

    private IEnumerator FadeRoutine(float startAlpha, float targetAlpha, bool enableInteraction)
    {
        yield return new WaitForSeconds(fadeDelay);

        _canvasGroup.alpha = startAlpha;

        var timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / fadeDuration);
            yield return null;
        }

        _canvasGroup.alpha = targetAlpha;
    }
    
}