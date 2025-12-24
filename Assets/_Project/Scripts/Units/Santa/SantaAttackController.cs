using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SantaAttackController : MonoBehaviour
{
    [SerializeField] private AudioClip deathSound;
    private AudioSource _audioSource;

    private bool _hasAttackSequenceStarted = false;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (!otherCollider.CompareTag("Player")) return;

        if (_hasAttackSequenceStarted) return;
        _hasAttackSequenceStarted = true;
        
        StartCoroutine(HandleAttackSequence());
    }

    private IEnumerator HandleAttackSequence()
    {
        yield return new WaitForSeconds(0.5f);
        
        PlayerManager.Instance.DisablePlayerLogic("PlayerDeath");
        PlayerManager.Instance.HandleonKillSequence();
        _audioSource.PlayOneShot(deathSound);
        SceneChangeManager.Instance.RestartScene(SceneManager.GetActiveScene().name);
    }
}
