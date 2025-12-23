using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SantaAttackController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (!otherCollider.CompareTag("Player")) return;

        StartCoroutine(HandleAttackSequence());
    }

    private IEnumerator HandleAttackSequence()
    {
        yield return new WaitForSeconds(0.5f);
        
        PlayerManager.Instance.DisablePlayerLogic("PlayerDeath");
        PlayerManager.Instance.HandleonKillSequence();
        SceneChangeManager.Instance.RestartScene(SceneManager.GetActiveScene().name);
    }
}
