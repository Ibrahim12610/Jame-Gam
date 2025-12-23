using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SantaAttackController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (!otherCollider.CompareTag("Player")) return;
        
        PlayerManager.Instance.DisablePlayerLogic("Death");
        PlayerManager.Instance.HandleonKillSequence();
        SceneChangeManager.Instance.RestartScene(SceneManager.GetActiveScene().name);
    }
    
}
