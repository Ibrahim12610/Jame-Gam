using System;
using UnityEngine;

public class ElfDetectionController : MonoBehaviour
{
    [SerializeField] private GameObject elfParentObject;
    public static event Action OnElfDetectedPlayer;
    public static event Action OnElfNoLongerDetectedPlayer;

    public static event Action<bool, Vector2> ElfDetectedEvent;

    SantaAI ai;

    private void Start()
    {
        ai = GameObject.FindFirstObjectByType<SantaAI>();
    }
    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (!otherCollider.CompareTag("Player")) return;
        
        OnElfDetectedPlayer?.Invoke();
        ElfDetectedEvent?.Invoke(true, elfParentObject.transform.position);
        if(!ai.isChasing)
            CreateElfBellSound();
    }

    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (!otherCollider.CompareTag("Player")) return;
        
        OnElfNoLongerDetectedPlayer?.Invoke();
        ElfDetectedEvent?.Invoke(false, elfParentObject.transform.position);
    }
    void CreateElfBellSound()
    {
        SantaAI listener = PlayerManager.Instance.santaListener;
        SoundSignal signal =
            new SoundSignal(SoundType.ElfBell, transform.position, Time.time);
        listener.HearSound(signal);
    }
}
