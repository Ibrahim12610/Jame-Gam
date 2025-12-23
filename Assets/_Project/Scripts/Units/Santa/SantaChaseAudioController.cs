using System;
using UnityEngine;

public class SantaChaseAudioController : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        SantaChasing.SantaChasingStarted += HandleChaseAudio;
    }

    private void OnDisable()
    {
        SantaChasing.SantaChasingStarted -= HandleChaseAudio;
    }
    
    private void HandleChaseAudio()
    {
        _audioSource.Play();
    }
    
}
