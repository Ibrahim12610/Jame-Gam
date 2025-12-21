using System;
using UnityEngine;

public class ElfDamageSoundController : MonoBehaviour
{
    [SerializeField] private AudioClip hurtSound;
    
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void OnHurt()
    {
        _audioSource.PlayOneShot(hurtSound);
    }
}
