using System;
using UnityEngine;

public class ElfDetectionController : MonoBehaviour
{
    [SerializeField] private GameObject elfParentObject;
    public static event Action OnElfDetectedPlayer;
    public static event Action OnElfNoLongerDetectedPlayer;

    public static event Action<bool, Vector2> ElfDetectedEvent;

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (!otherCollider.CompareTag("Player")) return;
        
        OnElfDetectedPlayer?.Invoke();
        ElfDetectedEvent?.Invoke(true, elfParentObject.transform.position);
    }

    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (!otherCollider.CompareTag("Player")) return;
        
        OnElfNoLongerDetectedPlayer?.Invoke();
        ElfDetectedEvent?.Invoke(false, elfParentObject.transform.position);
    }
}
