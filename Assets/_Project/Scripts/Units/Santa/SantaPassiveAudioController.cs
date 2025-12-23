using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SantaPassiveAudioController : MonoBehaviour
{
    private Transform _player;

    [SerializeField]
    private float maxDistance = 10f;
    
    [SerializeField] private float maxVolume = 1f;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = true;
    }

    private void Start()
    {
        if (!_audioSource.isPlaying)
            _audioSource.Play();
    }

    private void Update()
    {
        if (!PlayerManager.Instance) return;
        _player = PlayerManager.Instance.GetPlayerTransform();

        float distance = Vector3.Distance(_player.position, transform.position);
        float volume = 1 - (distance / maxDistance);
        _audioSource.volume = Mathf.Clamp(volume, 0f, maxVolume);
        
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
#endif
}