using System.Collections;
using UnityEngine;

public class GameUtility : MonoBehaviour
{
    public static IEnumerator FadeOutAndStop(AudioSource source, float duration)
    {
        if (!source.isPlaying)
            yield break;

        var startVolume = source.volume;
        var t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }

        source.Stop();
        source.volume = startVolume;
    }
}
