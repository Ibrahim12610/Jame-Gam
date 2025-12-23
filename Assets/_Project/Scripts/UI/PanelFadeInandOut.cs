using System.Collections;
using UnityEngine;

public class PanelFadeInandOut : MonoBehaviour
{
    [Header("Fade Settings")] [SerializeField]
    private float fadeDuration = 0.5f;

    [SerializeField] private float betweenDelay = 0f;
    [SerializeField] private float fadeDelay = 0f;

    private CanvasGroup _canvasGroup;
    private Coroutine _fadeRoutine;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0f;
    }

    private void Start()
    {
        StartCoroutine(FadeInAndOut());
    }

    private IEnumerator FadeInAndOut()
    {
        StartFade(0f, 1f);
        yield return new WaitForSeconds(betweenDelay);
        StartFadeOut(1f, 0f);
    }
    

    private void StartFade(float startAlpha, float targetAlpha )
    {
        if (_fadeRoutine != null)
            StopCoroutine(_fadeRoutine);

        _fadeRoutine = StartCoroutine(FadeRoutine(startAlpha, targetAlpha));
    }
    
    private void StartFadeOut(float startAlpha, float targetAlpha )
    {
        if (_fadeRoutine != null)
            StopCoroutine(_fadeRoutine);

        _fadeRoutine = StartCoroutine(FadeRoutine(startAlpha, targetAlpha));
    }

    private IEnumerator FadeRoutine(float startAlpha, float targetAlpha)
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