using System;
using UnityEngine;

public class SoundSpawnerController : MonoBehaviour
{
    public AudioClip clip;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayClip(float volume = 0.5f)
    {
        _audioSource.PlayOneShot(clip);
        Destroy(gameObject, clip.length);
    }
}
