using System;
using UnityEngine;

public class AttackPointController : MonoBehaviour
{
    [SerializeField] private float damageAmount = 50f;

    private void OnTriggerEnter2D(Collider2D otherCollder)
    {
        if (otherCollder.TryGetComponent(out ElfHealthController otherHealth))
        {
            otherHealth.TakeDamage(damageAmount);
        }
    }
}
