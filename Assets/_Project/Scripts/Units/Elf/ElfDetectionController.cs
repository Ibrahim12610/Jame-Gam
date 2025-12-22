using System;
using UnityEngine;

public class ElfDetectionController : MonoBehaviour
{
    public static event Action OnElfDetectedPlayer;
    public static event Action OnElfNoLongerDetectedPlayer;

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (!otherCollider.CompareTag("Player")) return;
        
        OnElfDetectedPlayer?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (!otherCollider.CompareTag("Player")) return;
        
        OnElfNoLongerDetectedPlayer?.Invoke();
    }
}
