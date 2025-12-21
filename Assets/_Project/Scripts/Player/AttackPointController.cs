using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackPointController : MonoBehaviour
{
    [SerializeField] private float damageAmount = 50f;
    
    private readonly HashSet<ElfHealthController> _hitTargets = new();

    private void OnEnable()
    {
        _hitTargets.Clear();
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (!otherCollider.TryGetComponent(out ElfHealthController health))
            return;
        
        if (_hitTargets.Contains(health))
            return;

        _hitTargets.Add(health);
        health.TakeDamage(damageAmount);
    }
}
