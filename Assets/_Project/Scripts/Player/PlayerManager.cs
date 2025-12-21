using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    
    private ImpulseController _impulseController;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        _impulseController = GetComponent<ImpulseController>();
    }

    public void SetPlayerTransform(Transform t)
    {
        transform.position = t.position;
    }

    public void HandleDestroy()
    {
        Destroy(gameObject);
    }

    public Transform GetPlayerTransform()
    {
        return gameObject.transform;
    }
    
    public bool IsInImpulse() => _impulseController.IsInImpulse();
}
