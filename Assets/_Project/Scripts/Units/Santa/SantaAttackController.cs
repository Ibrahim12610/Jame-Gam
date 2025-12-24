using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SantaAttackController : MonoBehaviour
{
    [SerializeField] private AudioClip deathSound;
    private AudioSource _audioSource;
    private SantaAI _santaAI;
    private bool _hasAttackSequenceStarted = false;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _santaAI =  GetComponentInParent<SantaAI>();
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
        PlayerManager.Instance.GetPlayerAnimator().TriggerDeath();
        PlayerManager.Instance.HandleDeathColliderLogic();
        _audioSource.PlayOneShot(deathSound);
        _santaAI.hasKilledPlayer = true;
        
        yield return new WaitForSeconds(5f);
        
        //PlayerManager.Instance.HandleonKillSequence();
        
        
        SceneChangeManager.Instance.RestartScene(SceneManager.GetActiveScene().name);
    }
}
