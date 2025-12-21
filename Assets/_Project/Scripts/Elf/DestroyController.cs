using System.Collections;
using UnityEngine;

public class DestroyController : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.3f;

    private SpriteRenderer _spriteRenderer;
    private Coroutine _fadeRoutine;
    private AudioSource _audioSource;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void DestroyWithFade()
    {
        if (_fadeRoutine != null)
            StopCoroutine(_fadeRoutine);

        StopNotifyAudio();
        _fadeRoutine = StartCoroutine(FadeAndDestroy());
    }

    private IEnumerator FadeAndDestroy()
    {
        if (_spriteRenderer == null)
        {
            Destroy(gameObject);
            yield break;
        }

        var color = _spriteRenderer.color;
        var startAlpha = color.a;
        var t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            var alpha = Mathf.Lerp(startAlpha, 0f, t / fadeDuration);
            _spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        Destroy(gameObject);
    }
    
    private void StopNotifyAudio()
    {
        if (_audioSource.isPlaying)
        {
            _fadeRoutine = StartCoroutine(
                GameUtility.FadeOutAndStop(_audioSource, 0.25f)
            );
        }
    }
}